using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveController : BaseMove
{

    
    [SerializeField]
    private BaseEntityController controller;
    [SerializeField]
    private float stopSnap = 1f;
   


    [SerializeField]
    private float runSpeed = 5f;
    [SerializeField]
    private float airSpeed = 5f;
    [SerializeField]
    private float groundAccel = 10f;
    [SerializeField]
    private float airAccel = 1f;
    private bool jumpHeld = false;

    [SerializeField]
    private float jumpForce = 100f;
    [SerializeField]
    private float jumpHoldForce = 50f;

    [SerializeField]
    private int maxJumps = 2;
    private int jumps;

    public bool FacingUnlock { get { return Grounded || facingOverride > 0; } }

    private int facingOverride = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void FixedUpdate () {
        if(facingOverride > 0) facingOverride--;

        

        if (!controller.Stunned)
        {
            if (moveInput.x != 0)
            {
                float mult = 1f;
                foreach (BaseMove.SpeedModifier sm in speedMods)
                {
                    mult *= sm.multiplier;
                }


                Vector2 v = rigidbody.velocity;
                v.x = Mathf.Clamp(moveInput.x, -1f, 1f)
                    * ((Grounded) ? runSpeed : airSpeed) * mult;


                float accmult = 1f;
                foreach (BaseMove.SpeedModifier sm in speedAccelMods)
                {
                    accmult *= sm.multiplier;
                }

                if (v.x > rigidbody.velocity.x)
                {
                    rigidbody.velocity += Vector2.right * ((Grounded) ? groundAccel : airAccel) * Time.fixedDeltaTime * accmult;
                    if(rigidbody.velocity.x > v.x)
                    {
                        Vector2 newV = rigidbody.velocity;
                        newV.x = v.x;
                        rigidbody.velocity = newV;

                    }
                }
                else if (v.x < rigidbody.velocity.x)
                {
                    rigidbody.velocity += Vector2.left * ((Grounded) ? groundAccel : airAccel) * Time.fixedDeltaTime * accmult;
                    if (rigidbody.velocity.x < v.x)
                    {
                        Vector2 newV = rigidbody.velocity;
                        newV.x = v.x;
                        rigidbody.velocity = newV;

                    }
                }


            }
            else if(overlapFrames == 0)
            {
                float decmult = 1f;
                foreach (BaseMove.SpeedModifier sm in speedDecelMods)
                {
                    decmult *= sm.multiplier;
                }
                rigidbody.AddForce(new Vector2(-rigidbody.velocity.x * stopSnap * runSpeed * (Grounded ? 1 : 0.2f), 0) * rigidbody.mass * decmult);
            }
            else
            {
                overlapFrames--;
            }

            if (Grounded)
            {
                CheckFacing();

                jumps = maxJumps;
            }

            if (moveInput.y > 0.1f)
            {
                if (!Grounded && jumps == maxJumps)
                    jumps--;

                if (!jumpHeld && jumps > 0)
                {
                    Vector2 v = rigidbody.velocity;
                    v.y = 0;
                    if (moveInput.x != 0 && Mathf.Sign(moveInput.x) != Mathf.Sign(rigidbody.velocity.x))
                    {
                        v.x = 0;
                    }
                    rigidbody.velocity = v;

                    rigidbody.AddForce(Vector2.up * jumpForce * rigidbody.mass);
                    jumpHeld = true;
                    jumps--;
                    facingOverride = 1;

                    StartCoroutine(DelayCheckFacing());
                }
                else if (jumpHeld && rigidbody.velocity.y > 0)
                {
                    rigidbody.AddForce(Vector2.up * jumpHoldForce * rigidbody.mass);
                }

            }
            else
            {
                jumpHeld = false;
            }
        }
	}
    private IEnumerator DelayCheckFacing()
    {
        yield return null;
        CheckFacing();
    }
    public override void CheckFacing()
    {
        if (moveInput.x != 0 && ((controller.facing > 0 && transform.localScale.x < 0)
                    || (controller.facing < 0 && transform.localScale.x > 0)))
        {
            Vector3 s = transform.localScale;
            s.x = -s.x;
            transform.localScale = s;
        }
    }

    private int overlapFrames = 0;

    protected override void OnTriggerStay2D(Collider2D col)
    {
        base.OnTriggerStay2D(col);

        if (col.gameObject.layer == 11)
        {
            overlapFrames = 5;

            Vector2 v = rigidbody.velocity;
            v.x *= 0.7f;
            rigidbody.velocity = v;
        }
    }

    public override void ResetMoveSystem()
    {
        base.ResetMoveSystem();
        rigidbody.velocity = Vector3.zero;
    }
}
