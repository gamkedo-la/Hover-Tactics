using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Touch")]
    public bool touch = false;
    [SerializeField] private GameObject touchControls;
    [SerializeField] private Sprite[] basicWeaponImages;
    [SerializeField] private Sprite[] specialWeaponImages;
    [Header("Movement Mode")]
    public bool twinShooterMovementMode = false; //Default is set is BaseScreen.cs
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
    [SerializeField] private GameObject waypointGroup;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject lowShieldWarning;
    [SerializeField] private GameObject progressDisplay;

    static public GameManager instance;

    [HideInInspector] public float useSpecial = 0.0f;
    public void UseSpecial() { useSpecial = 0.25f; }

    private CameraController camController;
    private RadialBars playerBars;

    private GameObject mechDisplayGroup;

    private float abilityDisplayTimer = 0.0f;

    private int totalBuildingsLeft = 0;
    private int maxBuildings = 0;

    public void IncrementBuildingsLeft()
    {
        totalBuildingsLeft++;
        maxBuildings++;
    }

    public void DecrementBuildingsLeft()
    {
        totalBuildingsLeft--;
    }

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
        if(touch)
        {
            touchControls.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = basicWeaponImages[index];
            touchControls.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = specialWeaponImages[index];
        }
    }

    void DeactivateHoverMech(int index)
    {
        hoverMechs[index].transform.GetChild(1).gameObject.SetActive(true); //Shield
        hoverMechs[index].GetComponent<MechController>().enabled = false;
        hoverMechs[index].GetComponent<MechController>().StopMovement();
    }

    public void SwitchHoverMech()
    {
        SoundFXManager.PlayOneShot(switchSound);
        DeactivateHoverMech(activeIndex++);
        if(activeIndex >= hoverMechs.Length) activeIndex = 0;
        ActivateHoverMech(activeIndex);
        abilityDisplayTimer = abilityDisplayDelay;
    }

    void Awake()
    {
        instance = this;
        touchControls.SetActive(touch);
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

        Portal.ClearPortals();

        progressDisplay.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/2, 8.0f);
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
        CheckWin();
        LowShieldWarning();
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
        if(!touch)
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
        else
        {
            mechAbilityDisplay.SetActive(false);
        }
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
        gameOverPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }

    void CheckWin()
    {
        if(Mathf.FloorToInt(Time.time * 2.0f) % 2 == 0)
        {
            int remainingVitalPoints = 0;
            for(int i = 0; i < waypointGroup.transform.childCount; i++)
            {
                if(waypointGroup.transform.GetChild(i).gameObject.activeSelf)
                {
                    remainingVitalPoints++;
                }
            }

            progressDisplay.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = ((float)totalBuildingsLeft / (float)maxBuildings);
            progressDisplay.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Vitals left: " + remainingVitalPoints.ToString();

            if(remainingVitalPoints <= 0)
            {
                hoverMechs[activeIndex].GetComponent<MechController>().enabled = false;
                Time.timeScale = 0.0f;
                SoundFXManager.PlayOneShot(SoundFxKey.WIN);
                winPanel.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    void LowShieldWarning()
    {
        bool isShieldLow = false;
        for(int i = 0; i < hoverMechs.Length; i++)
        {
            if(hoverMechs[i].GetComponent<Shield>().Get() < 0.15f && !winPanel.activeSelf && !gameOverPanel.activeSelf)
            {
                isShieldLow = true;
                break;
            }
        }
        lowShieldWarning.SetActive(isShieldLow);

        if(Mathf.FloorToInt(Time.time * 2.0f) % 2 == 0)
            lowShieldWarning.transform.GetChild(0).localScale = lowShieldWarning.transform.GetChild(1).localScale = Vector3.one * 1.12f;
        
        lowShieldWarning.transform.GetChild(0).localScale = lowShieldWarning.transform.GetChild(1).localScale = Vector3.Lerp(lowShieldWarning.transform.GetChild(0).localScale, Vector3.one, Time.deltaTime * 4.0f);

        if(lowShieldWarning.activeSelf) lowShieldWarning.GetComponent<AudioSource>().enabled = SoundFXManager.state;
    }
}
