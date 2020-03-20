using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    [SerializeField]
    private Collider2D[] colliders;
    [SerializeField]
    private Transform groundedPoint;
    [SerializeField]
    private LayerMask groundedMask;

    public List<SpeedModifier> speedMods = new List<SpeedModifier>();
    public List<SpeedModifier> speedAccelMods = new List<SpeedModifier>();
    public List<SpeedModifier> speedDecelMods = new List<SpeedModifier>();

    [Serializable]
    public class SpeedModifier
    {
        public float multiplier;
    }

    public void ForceFlipFacing()
    {
        
        Vector3 s = transform.localScale;
        s.x = -s.x;
        transform.localScale = s;
        
    }

    [SerializeField]
    protected new Rigidbody2D rigidbody;

    public Vector2 moveInput = Vector2.zero;
    public Vector2 rawInput = Vector2.zero;

    [SerializeField]
    public int Layer
    {
        get
        {
            return (Grounded) ? 11 : 12;
        }
    }

    public virtual bool Grounded
    {

        get
        {
            return Physics2D.OverlapPoint(groundedPoint.transform.position, groundedMask) != null;
        }
    }
    private int lastLayer;
    private void Start()
    {
        lastLayer = Layer;
    }
    private void Update()
    {
        if (Layer != lastLayer)
        {
            foreach (Collider2D col in colliders)
            {
                col.gameObject.layer = Layer;
            }
            lastLayer = Layer;
        }
    }

    [SerializeField]
    private float repelPushForce = 15f;
    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 11 || col.gameObject.layer == 12)
        {
            Vector3 rel = rigidbody.position - (Vector2)col.transform.position;
            rel.y = 0;
            if (rel.magnitude < 0.3f) ;
                rel.x = 0.3f * Mathf.Sign(rel.x);
            rel.Normalize();

            if (col.gameObject.layer == 11)
                rigidbody.AddForce(rel * repelPushForce);
            else if (col.gameObject.layer == 12)
                rigidbody.AddForce(rel * repelPushForce / 2);
        }
    }

    public virtual void CheckFacing() { }

    public virtual void ResetMoveSystem() {
        speedMods.Clear();
        speedAccelMods.Clear();
        speedDecelMods.Clear();
    }
}