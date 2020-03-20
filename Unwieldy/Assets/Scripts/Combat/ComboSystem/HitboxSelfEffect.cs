using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSelfEffect : BaseHitbox
{
    [SerializeField]
    private IDamageable target;

    [SerializeField]
    private bool reverseBasedOnFacing = true;

    public override void Trigger()
    {
        target.Damage(gripDamage, hpDamage);
        target.Stun(stunFrames);

        Vector2 m = minLaunch;
        Vector2 m2 = maxLaunch;
        if (reverseBasedOnFacing)
        {
            m.x *= parentNode.controller.facing;
            m2.x *= parentNode.controller.facing;
        }
        target.Launch(m, m2, allowLaunchStun);

        target.Break(GripBreakManager.GetGripBreak(gripBreak));
    }


}
