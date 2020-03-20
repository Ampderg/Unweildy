using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeaponPickup : BaseItem
{
    public string weapon;

    public override void Pickup(BaseEntityController c)
    {
        Debug.Log("test");

        c.combo.Equip(weapon);
        c.entity.FixGrip();

        Destroy(gameObject);

    }
}
