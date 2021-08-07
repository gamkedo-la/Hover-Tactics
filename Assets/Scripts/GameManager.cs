using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hoverMechs;
    [SerializeField] private int activeIndex = 0;

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
        DeactivateHoverMech(activeIndex++);
        if(activeIndex >= hoverMechs.Length) activeIndex = 0;
        ActivateHoverMech(activeIndex);
    }

    void Start()
    {
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
        UpdateAllMechDisplays();
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
            //shield.fillAmount = playerBars.GetShield();
        }
    }
}
