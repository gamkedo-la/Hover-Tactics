using UnityEngine;

public class Shield : MonoBehaviour
{
    private float value = 1.0f;

    private void Start()
    {
        SetToFull();
    }

    public void ChangeBy(float value)
    {
        if(value < 0.0f) value /= AssistPanel.GetShield();
        else value *= AssistPanel.GetShield();

        this.value = Mathf.Clamp(this.value + value, 0.0f, 1.0f);
    }

    public bool IsZero()
    {
        return value <= 0.0f;
    }

    public float Get()
    {
        return value;
    }

    public void SetToFull()
    {
        value = 1.0f;
    }
}
