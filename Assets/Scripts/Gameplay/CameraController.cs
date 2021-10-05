using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController singleton;
    [SerializeField] private Transform transformToFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float lerpFactor;
    [SerializeField] private float mouseFactor;
    [SerializeField] private GameObject cursor;
    [SerializeField] private LayerMask cursorLayers;
    [SerializeField] private int totalInstances = 6;

    public Vector3 lastAimPoint { get; private set; }
    private Camera cam;


    public void SetTransformToFollow(Transform tr)
    {
        transformToFollow = tr;
    }

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        Cursor.visible = false;
        cam = GetComponent<Camera>();

        CameraShake.instances = new List<CameraShake>();
        while(totalInstances > 0)
        {
            CameraShake.instances.Add(gameObject.AddComponent<CameraShake>());
            totalInstances--;
        }

        QualitySettingsEffect();
    }

    void Update()
    {
        Vector3 position = Vector3.Lerp(
            transformToFollow.position + offset,
            cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)),
            mouseFactor
        );
        transform.position = Vector3.Lerp(transform.position, position, lerpFactor * Time.unscaledDeltaTime);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 160, cursorLayers))
        {
            lastAimPoint = hitData.point;
        }

        Cursor.visible = !GameManager.instance.gameObject.activeSelf;
        if(!Cursor.visible) cursor.transform.position = lastAimPoint;
    }

    void QualitySettingsEffect()
    {
        if(QualitySettings.GetQualityLevel() == 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }
}
