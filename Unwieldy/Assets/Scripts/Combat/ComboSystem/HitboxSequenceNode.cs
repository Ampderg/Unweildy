using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitboxSequenceNode : AttackNode
{
    [Serializable]
    private struct SequencePartTime
    {
        public BaseHitbox hitbox;
        public int frameTime;
    }

    [SerializeField]
    private SequencePartTime[] sequence;

    public List<IDamageable> previousHitEntities { get; private set; }

    private int index;

    public override void Trigger()
    {
        base.Trigger();
        previousHitEntities = new List<IDamageable>();
        index = 0;
    }

    protected override void Update()
    {
        base.Update();
        if(active)
        {
            while(index < sequence.Length && sequence[index].frameTime == elapsedFrames)
            {
                sequence[index].hitbox.Trigger();
                index++;
            }
        }
    }

    public override void Kill()
    {
        base.Kill();
        for(int i = 0; i < index; i++)
        {
            sequence[i].hitbox.Kill();
        }
    }
}

