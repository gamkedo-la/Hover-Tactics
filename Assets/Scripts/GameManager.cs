using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] hoverMechs;
    [SerializeField] private int activeIndex = 0;

    static public GameManager instance;

    private CameraController camController;

    void Start()
    {
        instance = this;
        camController = Camera.main.GetComponent<CameraController>();
        hoverMechs[activeIndex].transform.GetChild(3).gameObject.SetActive(false); //Shield
        hoverMechs[activeIndex].GetComponent<SpaceshipController>().enabled = true;
        camController.SetTransformToFollow(hoverMechs[activeIndex].transform);
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire3"))
        {
            hoverMechs[activeIndex].transform.GetChild(3).gameObject.SetActive(true); //Shield
            hoverMechs[activeIndex++].GetComponent<SpaceshipController>().enabled = false;
            if(activeIndex >= hoverMechs.Length) activeIndex = 0;
            hoverMechs[activeIndex].transform.GetChild(3).gameObject.SetActive(false); //Shield
            hoverMechs[activeIndex].GetComponent<SpaceshipController>().enabled = true;
            camController.SetTransformToFollow(hoverMechs[activeIndex].transform);
        }
    }
}
