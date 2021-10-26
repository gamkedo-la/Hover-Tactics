using UnityEngine;
using UnityEngine.UI;

public class AssistPanel : MonoBehaviour
{
    //Defaults -> 1 (Hard) for PC, 2 (Medium) for Touch Devices
    static public int movement = 2;
    static public int damage = 2;
    static public int health = 2;
    static public int power = 2;
    static public int shield = 2;

    //Easy = 3, Medium = 2, Hard = 1

    public static float GetMovement()
    {
        switch(movement)
        {
            case 1: return 1.0f;
            case 2: return 1.5f;
            case 3: return 2.0f;
        }
        return 1.0f;
    }

    public static float GetDamage()
    {
        switch(damage)
        {
            case 1: return 1.0f;
            case 2: return 1.5f;
            case 3: return 2.0f;
        }
        return 1.0f;
    }

    public static float GetHealth()
    {
        switch(health)
        {
            case 1: return 1.0f;
            case 2: return 1.25f;
            case 3: return 1.5f;
        }
        return 1.0f;
    }

    public static float GetPower()
    {
        switch(power)
        {
            case 1: return 1.0f;
            case 2: return 1.25f;
            case 3: return 1.5f;
        }
        return 1.0f;
    }

    public static float GetShield()
    {
        switch(shield)
        {
            case 1: return 1.0f;
            case 2: return 1.25f;
            case 3: return 1.5f;
        }
        return 1.0f;
    }

    bool DifficultyIndexCheck(int value, int i)
    {
        if((value == 3 && i == 0)
        || (value == 2 && i == 1)
        || (value == 1 && i == 2))
            return true;
        return false;
    }

    bool GetToggleValue(int p, int i)
    {
        switch(p)
        {
            case 3: return DifficultyIndexCheck(movement, i);
            case 4: return DifficultyIndexCheck(damage, i);
            case 5: return DifficultyIndexCheck(health, i);
            case 6: return DifficultyIndexCheck(power, i);
            case 7: return DifficultyIndexCheck(shield, i);
        }
        return false;
    }

    void OnEnable()
    {
        for(int p = 3; p <= 7; p++)
        {
            for(int i = 0; i < 3; i++)
            {
                transform.GetChild(p).GetChild(i).GetComponent<Toggle>().isOn = GetToggleValue(p, i);
            }
        }
    }
}