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
    [Space]
    [SerializeField] private GameObject cursor;

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
    }

    void Update()
    {
        Vector3 position = Vector3.Lerp(
            transformToFollow.position + offset,
            cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)),
            mouseFactor
        );
        transform.position = Vector3.Lerp(transform.position, position, lerpFactor * Time.deltaTime);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 200))
            lastAimPoint = hitData.point;

        cursor.transform.position = lastAimPoint;
    }
}