﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseHitbox : MonoBehaviour
{

    [SerializeField]
    protected int gripDamage;
    [SerializeField]
    protected GripBreak gripBreak;
    [SerializeField]
    protected int hpDamage;
    [SerializeField]
    protected int stunFrames;
    [SerializeField]
    protected Vector2 minLaunch;
    [SerializeField]
    protected Vector2 maxLaunch;
    [SerializeField]
    protected HitboxSequenceNode parentNode;
    [SerializeField]
    protected bool allowLaunchStun = true;
    public abstract void Trigger();

    public void SetLaunch(Vector2 min, Vector2 max)
    {
        minLaunch = min;
        maxLaunch = max;
    }

    public virtual void Kill() { }

}
public class HitboxSequencePart : BaseHitbox
{

    public enum DetectionType
    {
        InitialHit,
        ContinuousDetection
    }

    protected int remainingFrames;
    [SerializeField]
    protected int frameDuration;
    [SerializeField]
    protected new Collider2D collider;
    
    [SerializeField]
    protected DetectionType detection;

    [SerializeField]
    protected bool useExplosionLaunch = false;
    [SerializeField]
    protected Transform explosionLaunchPoint;

    

    //between how many frames should continuos detection check?
    public const int FRAME_EFFICIENCY = 3;

    private ContactFilter2D contactFilter;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    protected new Renderer renderer;
    [SerializeField]
    private bool reverseBasedOnUserFacing = true;

    private void Awake()
    {
        contactFilter.SetLayerMask(layerMask);

        if (renderer != null) renderer.enabled = false;
    }

    public override void Trigger()
    {
        if(detection == DetectionType.InitialHit)
        {
            CheckHit();
        }
        else if(detection == DetectionType.ContinuousDetection)
        {
            remainingFrames = frameDuration;
        }
        if(renderer != null)
        {
            renderer.enabled = true;
        }
    }

    protected virtual void Update()
    {
        if(detection == DetectionType.ContinuousDetection && remainingFrames > 0)
        {
            if(remainingFrames % FRAME_EFFICIENCY == 0)
                CheckHit();

        }

        if(remainingFrames <= 0)
        {
            if (renderer != null) renderer.enabled = false;
        }

        remainingFrames--;

    }

    private void CheckHit()
    {
        List<Collider2D> hits = new List<Collider2D>();
        int n = collider.OverlapCollider(contactFilter, hits);
        for(int i = 0; i < n; i++)
        {
            Hurtbox h = hits[i].gameObject.GetComponent<Hurtbox>();
            if (h != null 
                && h.hitParent != this.parentNode.controller.entity && (detection == DetectionType.InitialHit 
                || (detection == DetectionType.ContinuousDetection && !parentNode.previousHitEntities.Contains(h.GetHitParent()))))
            {

                h.Damage(gripDamage, hpDamage);
                h.Stun(stunFrames);

                if (useExplosionLaunch)
                {
                    Vector2 m = (h.transform.position - explosionLaunchPoint.position);
                    if(m.magnitude < 0.4f)
                    {
                        m = Vector2.up;
                    }
                    m.Normalize();
                    Vector2 m2 = m;

                    m *= minLaunch;
                    m2 *= maxLaunch;

                    h.Launch(m, m2, allowLaunchStun);
                }
                else
                {
                    Vector2 m = minLaunch;
                    Vector2 m2 = maxLaunch;
                    if (reverseBasedOnUserFacing)
                    {
                        m.x *= parentNode.controller.facing;
                        m2.x *= parentNode.controller.facing;
                    }
                    h.Launch(m, m2, allowLaunchStun);
                }

                h.Break(GripBreakManager.GetGripBreak(gripBreak));

                if (detection == DetectionType.ContinuousDetection)
                    parentNode.previousHitEntities.Add(h.GetHitParent());
            }
        }
    }

    public override void Kill()
    {
        remainingFrames = 0;
    }


}

