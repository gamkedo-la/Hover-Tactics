using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform transformToFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float lerpFactor;
    [SerializeField] private float mouseFactor;
    [Space]
    [SerializeField] private GameObject cursor;

    private Camera cam;

    public void SetTransformToFollow(Transform tr)
    {
        transformToFollow = tr;
    }

    void Start()
    {
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

        Cursor.visible = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        if(Physics.Raycast(ray, out hitData, 200))
        {
            cursor.transform.position = hitData.point;
        }
    }
}
