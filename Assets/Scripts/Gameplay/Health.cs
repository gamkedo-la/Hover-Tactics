using UnityEngine;

public class Health : MonoBehaviour
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

    public void ChangeBy(float value)
    {
        current = Mathf.Clamp(current + value, 0.0f, current);
    }

    public bool IsZero()
    {
        return current <= 0.0f;
    }

    public float Get()
    {
        return current;
    }

    public void SetToFull()
    {
        current = max;
    }
}
