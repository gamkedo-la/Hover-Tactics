using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Mech Switch")]
    [SerializeField] private GameObject[] hoverMechs;
    [SerializeField] private int activeIndex = 0;
    [SerializeField] private SoundFxKey switchSound;
    [Header("Shield Rates")]
    [SerializeField] private float shieldDepletionInSeconds = 60.0f;
    [SerializeField] private float shieldRecoveryInSeconds = 45.0f;
    [Space]
    [SerializeField] private GameObject gameOverPanel;

    static public GameManager instance;

    private CameraController camController;
    private RadialBars playerBars;

    private GameObject mechDisplayGroup;

    public GameObject GetActiveHoverMech()
    {
        return hoverMechs[activeIndex];
    }

    public RadialBars GetPlayerBars()
    {
        return playerBars;
    }

    public void AddShieldToAll(float value)
    {
        for(int i = 0; i < hoverMechs.Length; i++)
            hoverMechs[i].GetComponent<Shield>().ChangeBy(value);
    }

    void ActivateHoverMech(int index)
    {
        hoverMechs[index].transform.GetChild(1).gameObject.SetActive(false); //Shield
        hoverMechs[index].GetComponent<MechController>().enabled = true;
        camController.SetTransformToFollow(hoverMechs[index].transform);
    }

    void DeactivateHoverMech(int index)
    {
        hoverMechs[index].transform.GetChild(1).gameObject.SetActive(true); //Shield
        hoverMechs[index].GetComponent<MechController>().enabled = false;
        hoverMechs[index].GetComponent<MechController>().StopMovement();
    }

    void SwitchHoverMech()
    {
        SoundFXManager.PlayOneShot(switchSound);
        DeactivateHoverMech(activeIndex++);
        if(activeIndex >= hoverMechs.Length) activeIndex = 0;
        ActivateHoverMech(activeIndex);
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        
        instance = this;
        camController = Camera.main.GetComponent<CameraController>();
        playerBars = transform.GetChild(0).GetComponent<RadialBars>();
        mechDisplayGroup = transform.GetChild(2).gameObject;

        Assert.IsNotNull(camController, "Camera Controller is null!");
        Assert.IsNotNull(playerBars, "Player Bars (RadialBars) is null!");
        Assert.IsNotNull(mechDisplayGroup, "Mech Display Group object is null!");

        ActivateHoverMech(activeIndex);
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire3")) SwitchHoverMech();
        UpdateHP();
        //MP bar is updated inside the power script itself
        UpdateShield();
        UpdateAllMechDisplays();
        UpdateSpecials();
    }

    void UpdateAllMechDisplays()
    {
        for(int i = 0; i < mechDisplayGroup.transform.childCount; i++)
        {
            Transform mechDisplayTransform = mechDisplayGroup.transform.GetChild(i);

            if(i == activeIndex) mechDisplayTransform.gameObject.SetActive(false);
            else mechDisplayTransform.gameObject.SetActive(true);

            Image HP = mechDisplayTransform.GetChild(0).GetComponent<Image>();
            Image MP = mechDisplayTransform.GetChild(1).GetComponent<Image>();
            Image shield = mechDisplayTransform.GetChild(2).GetComponent<Image>();

            HP.fillAmount = hoverMechs[i].GetComponent<Health>().Get();
            MP.fillAmount = hoverMechs[i].GetComponent<Power>().Get();
            shield.fillAmount = hoverMechs[i].GetComponent<Shield>().Get();
        }
    }

    void UpdateHP()
    {
        playerBars.UpdateHP(hoverMechs[activeIndex].GetComponent<Health>().Get());

        for(int i = 0; i < hoverMechs.Length; i++)
        {
            if(hoverMechs[i].GetComponent<Health>().Get() <= 0.0f)
            {
                SetLoseReason(GetMechName(i) + "'s health got depleted to zero!");
                Lose();
            }
        }
    }

    void UpdateShield()
    {
        for(int i = 0; i < hoverMechs.Length; i++)
        {
            if(activeIndex == i) hoverMechs[i].GetComponent<Shield>().ChangeBy(Time.deltaTime / shieldRecoveryInSeconds);
            else hoverMechs[i].GetComponent<Shield>().ChangeBy(-Time.deltaTime / (shieldDepletionInSeconds * (activeIndex == 0 ? 2.0f : 1.0f)));

            if(hoverMechs[i].GetComponent<Shield>().Get() <= 0.0f)
            {
                SetLoseReason(GetMechName(i) + "'s shield got depleted to zero!");
                Lose();
            }
        }
        playerBars.UpdateShield(hoverMechs[activeIndex].GetComponent<Shield>().Get());
    }

    void UpdateSpecials()
    {
        playerBars.UpdateSpecials(hoverMechs[activeIndex].GetComponent<Power>().GetSpecials());
    }

    void Lose()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0.0f;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    string GetMechName(int i)
    {
        switch(i)
        {
            case 0: return "Chronos";
            case 1: return "Wyvern";
            case 2: return "Calamity";
        }
        return "";
    }

    void SetLoseReason(string text)
    {
        gameOverPanel.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = text;
    }
}
