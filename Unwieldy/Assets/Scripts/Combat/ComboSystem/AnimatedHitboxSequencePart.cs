using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedHitboxSequencePart : HitboxSequencePart
{
    [SerializeField]
    protected new Animation animation;

    public override void Trigger()
    {
        base.Trigger();
        animation.Play();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    
}
