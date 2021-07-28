using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public HPBar bar;

    // Start is called before the first frame update
    void Start()
    {
        if (bar != null)
        {
            bar.UpdateFill(currentHP / maxHP);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float Damage)
    {
        currentHP -= Damage;

        if(bar != null)
        {
            bar.UpdateFill(currentHP / maxHP);
        }

        if(currentHP <=0)
        {
            //death 
        }
    }

}
