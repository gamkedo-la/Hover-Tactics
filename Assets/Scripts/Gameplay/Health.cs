using UnityEngine;

public class Health : AbstractTakeDamage
{
    [SerializeField] private float max = 1.0f;
    private float current = 1.0f;

    private void OnEnable()
    {
        SetToFull();
    }

    private void Start()
    {
        SetToFull();
    }

    public override void TakeDamage(Damage damage)
    {
        ChangeBy(damage.Value);
    }

    public void ChangeBy(float value)
    {
        current = Mathf.Clamp(current + value, -0.05f, max);
    }

    public bool IsZero()
    {
        return current <= 0.0f;
    }

    public float Get()
    {
        return current;
    }

    public float GetMax()
    {
        return max;
    }

    public void SetToFull()
    {
        current = max;
    }
}
