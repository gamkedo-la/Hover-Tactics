using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MechController))]
public class Power : MonoBehaviour
{
    [SerializeField] private float replenishPerSecond = 0.1f;

    private MechController mechController;
    private float value = 1.0f;

    private void Start()
    {
        mechController = GetComponent<MechController>();

        Assert.IsNotNull(mechController, "Mech Controller is null!");

        SetToFull();
    }

    private void Update()
    {
        ChangeBy(replenishPerSecond * Time.deltaTime);

        if(mechController.enabled)
            GameManager.instance.GetPlayerBars().UpdateMP(value);
    }

    public void ChangeBy(float value)
    {
        this.value = Mathf.Clamp(this.value + value, 0.0f, 1.0f);
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
