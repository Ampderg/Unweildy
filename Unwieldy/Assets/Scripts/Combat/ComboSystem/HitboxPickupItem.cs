using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPickupItem : BaseHitbox
{
    [SerializeField]
    protected new Collider2D collider;
    private ContactFilter2D contactFilter;

    [SerializeField]
    private LayerMask layerMask;

    private void Awake()
    {
        contactFilter.SetLayerMask(layerMask);

    }

    public override void Trigger()
    {
        CheckHit();
    }

    private void CheckHit()
    {
        List<Collider2D> hits = new List<Collider2D>();
        int n = collider.OverlapCollider(contactFilter, hits);
        for (int i = 0; i < n; i++)
        {
            BaseItem h = hits[i].gameObject.GetComponent<BaseItem>();
            if (h != null)
            {
                h.Pickup(parentNode.controller);
                return;
            }
        }
    }
}
