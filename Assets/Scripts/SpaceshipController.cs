using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] float turnSensitivity;
    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 rightDirection = transform.right;
        rightDirection.y = 0.0f;
        Vector3 movementDirection = transform.forward * v + rightDirection * h;
        transform.position += movementDirection.normalized * speed * Time.deltaTime;
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float distance = Vector3.Distance(cam.transform.position, transform.position);
        Vector3 targetPoint = ray.GetPoint(distance);
        Vector3 relativeDirection = transform.InverseTransformPoint(targetPoint);
        float turnDir = Mathf.Atan2(relativeDirection.x, relativeDirection.z) * Mathf.Rad2Deg * turnSensitivity;
        turnDir = Mathf.Clamp(turnDir, -1, 1);
        Turn(turnDir);
    }

    public void Turn(float direction)
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += direction * turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
