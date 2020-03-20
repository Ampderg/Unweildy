using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxReverseFacing : BaseHitbox
{
    public override void Trigger()
    {
        this.parentNode.controller.facing = -this.parentNode.controller.facing;
        this.parentNode.controller.move.ForceFlipFacing();
    }
}
