using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Mech Switch")]
    [SerializeField] private GameObject[] hoverMechs;
    [SerializeField] private int activeIndex = 0;
    [SerializeField] private SoundFxKey switchSound;
    [SerializeField] private Camera cam;
    [SerializeField] private float[] camSizes;
    [Header("Shield Rates")]
    [SerializeField] private float shieldDepletionInSeconds = 60.0f;
    [SerializeField] private float shieldRecoveryInSeconds = 45.0f;
    [Header("Abilities Display")]
    [SerializeField] private GameObject mechAbilityDisplay;
    [SerializeField] private float abilityDisplayDelay = 6.0f;
    [Space]
    [SerializeField] private GameObject gameOverPanel;

    static public GameManager instance;

    private CameraController camController;
    private RadialBars playerBars;

    private GameObject mechDisplayGroup;

    private float abilityDisplayTimer = 0.0f;

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
        abilityDisplayTimer = abilityDisplayDelay;
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

        abilityDisplayTimer = abilityDisplayDelay;
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire3")) SwitchHoverMech();
        UpdateHP();
        //MP bar is updated inside the power script itself
        UpdateShield();
        UpdateAllMechDisplays();
        UpdateSpecials();
        AbilityDisplay();
        UpdateCameraSize();
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
                if(Time.timeScale > 0.0f)
                {
                    hoverMechs[i].GetComponent<MechController>().enabled = false;
                    hoverMechs[i].transform.GetChild(0).gameObject.SetActive(false); //Mesh
                    hoverMechs[i].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false; //Shield
                    CameraShake.Shake(6f, 15, 0.1f, 0.75f);
                    ObjectPooler.instance.SpawnFromPool("BuildingExplosion", hoverMechs[i].transform.position, Quaternion.identity);
                    SetLoseReason(GetMechName(i) + "'s health got depleted to zero!");
                    StartCoroutine("Lose");
                }
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
                if(Time.timeScale > 0.0f)
                {
                    hoverMechs[i].GetComponent<MechController>().enabled = false;
                    hoverMechs[i].transform.GetChild(0).gameObject.SetActive(false); //Mesh
                    hoverMechs[i].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false; //Shield
                    CameraShake.Shake(6f, 15, 0.1f, 0.75f);
                    ObjectPooler.instance.SpawnFromPool("BuildingExplosion", hoverMechs[i].transform.position, Quaternion.identity);
                    SetLoseReason(GetMechName(i) + "'s shield got depleted to zero!");
                    StartCoroutine("Lose");
                }
            }
        }
        playerBars.UpdateShield(hoverMechs[activeIndex].GetComponent<Shield>().Get());
    }

    void UpdateSpecials()
    {
        playerBars.UpdateSpecials(hoverMechs[activeIndex].GetComponent<Power>().GetSpecials());
    }

    void AbilityDisplay()
    {
        Transform tr = mechAbilityDisplay.transform;
        GameObject activeObj = tr.GetChild(activeIndex).gameObject;
        Color col = tr.GetChild(activeIndex).GetComponent<Image>().color;

        if(abilityDisplayTimer > 0.0f)
        {
            for(int i = 0; i < 3; i++)
            {
                tr.GetChild(i).gameObject.SetActive(false);
            }
            activeObj.SetActive(true);
            col = Color.Lerp(col, Color.white, 4.0f * Time.deltaTime);

            abilityDisplayTimer -= Time.deltaTime;
        }
        else
        {
            if(activeObj.activeSelf)
            {
                col = Color.Lerp(col, Color.clear, 2.0f * Time.deltaTime);
                if(col == Color.clear) activeObj.SetActive(false);
            }
        }

        tr.GetChild(activeIndex).GetComponent<Image>().color = col;
        tr.GetChild(3).gameObject.GetComponent<Image>().color = col; //MoreControls
    }

    void UpdateCameraSize()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, camSizes[activeIndex], 2.0f * Time.deltaTime);
    }

    IEnumerator Lose()
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(1.0f);
        gameOverPanel.SetActive(true);
        gameObject.SetActive(false);
        yield return null;
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
        gameOverPanel.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = text;
    }
}
