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
    [Space]
    [SerializeField] private FloatingJoystick rotationStick = null;

    public Vector3 lastAimPoint { get; private set; }
    private Camera cam;
    private Vector3 position = Vector3.zero;
    private Vector2 effectorPosition = Vector3.zero;

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

        position = transformToFollow.position + offset;
    }

    void Update()
    {
        if(!rotationStick.transform.parent.gameObject.activeSelf)
        {
            effectorPosition = Input.mousePosition;
            Cursor.visible = Time.timeScale <= 0.1f;
        }
        else if(rotationStick.Horizontal > 0.01f
        || rotationStick.Horizontal < -0.01f
        || rotationStick.Vertical > 0.01f
        || rotationStick.Vertical < -0.01f)
        {
            effectorPosition = new Vector2(
                (rotationStick.Horizontal + 1.0f) * (Screen.width/2.0f),
                (rotationStick.Vertical + 1.0f) * (Screen.height/2.0f)
            );
            Cursor.visible = true;
        }

        position = Vector3.Lerp(
            transformToFollow.position + offset,
            cam.ScreenToWorldPoint(effectorPosition),
            mouseFactor
        );

        transform.position = Vector3.Lerp(transform.position, position, lerpFactor * Time.unscaledDeltaTime);

        Ray ray = Camera.main.ScreenPointToRay(effectorPosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 160, cursorLayers))
        {
            lastAimPoint = hitData.point;
        }

        if(Time.timeScale > 0.1f) cursor.transform.position = lastAimPoint;
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
