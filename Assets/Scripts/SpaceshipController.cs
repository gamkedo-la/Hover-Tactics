using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float turnSpeed;
    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movementDirection = transform.forward * v + transform.right * h;
        transform.position += movementDirection.normalized * speed * Time.deltaTime;
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float distance = Vector3.Distance(cam.transform.position, transform.position);
        Vector3 targetPoint = ray.GetPoint(distance);
        float turnDir = Vector3.Dot(transform.right, targetPoint - transform.position);
        Turn(turnDir);
    }

    public void Turn(float direction)
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += direction * turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
