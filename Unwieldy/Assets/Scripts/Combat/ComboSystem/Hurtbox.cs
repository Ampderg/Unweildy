using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Hurtbox : IDamageable
{

    public IDamageable hitParent;

    public override void Damage(int gripDamage, int hpDamage)
    {
        hitParent.Damage(gripDamage, hpDamage);
    }
    public override void Stun(int stunFrames)
    {
        hitParent.Stun(stunFrames);
    }
    public override void Launch(Vector2 minVector, Vector2 maxVector, bool allowLaunchStun)
    {
        hitParent.Launch(minVector, maxVector, allowLaunchStun);
    }

    public override void Break(int gripBreak)
    {
        hitParent.Break(gripBreak);
    }

    internal IDamageable GetHitParent()
    {
        return hitParent;
    }

    public override string GetEntityName()
    {
        return hitParent.GetEntityName();
    }

    public override BaseEntityController GetController()
    {
        return hitParent.GetController();
    }
}

