using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MechController))]
public class Power : MonoBehaviour
{
    [SerializeField] private float replenishPerSecond = 0.1f;
    [Range(0,3)] [SerializeField] private int specials = 3;

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

    public void ChangeBy_Special(int value)
    {
        this.specials = Mathf.Clamp(this.specials + value, 0, 3);
    }

    public float Get()
    {
        return value;
    }

    public int GetSpecials()
    {
        return specials;
    }

    public void SetToFull()
    {
        value = 1.0f;
    }

    public void SetToFull_Specials()
    {
        specials = 3;
    }
}
