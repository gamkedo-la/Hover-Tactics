using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform transformToFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float lerpFactor;
    [SerializeField] private float mouseFactor;

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
    }
}
