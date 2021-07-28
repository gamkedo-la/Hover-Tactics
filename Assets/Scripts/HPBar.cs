using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public GameObject Fill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(45, 45, 0);
    }

    public void UpdateFill(float percent)
    {
        Vector3 scale = new Vector3(percent, 1, 1);
        Fill.transform.localScale = scale;
    }

}
