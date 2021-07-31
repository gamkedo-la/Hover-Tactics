using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Portal Ability of Wyvern will be implemented using ProjectileWeapon
//Think of Portal as a Projectile that activates on hitting something

public class ProjectileWeapon : Weapon
{
    [Header("Projectile")]
    [SerializeField] private string projectileTag;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    protected override void Fire()
    {
        ObjectPooler.instance.SpawnFromPool(projectileTag, GetShootingPoint().position, GetShootingPoint().rotation);
    }
}
