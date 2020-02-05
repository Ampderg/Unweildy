using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSelfEffect : BaseHitbox
{
    [SerializeField]
    private IDamageable target;

    public override void Trigger()
    {
        target.Damage(gripDamage, hpDamage);
        target.Stun(stunFrames);

        Vector2 m = minLaunch;
        m.x *= parentNode.controller.facing;
        Vector2 m2 = maxLaunch;
        m2.x *= parentNode.controller.facing;
        target.Launch(m, m2, allowLaunchStun);

        target.Break(GripBreakManager.GetGripBreak(gripBreak));
    }


}
