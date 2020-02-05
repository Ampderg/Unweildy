using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {

    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private BaseEntityController controller;
    [SerializeField]
    private float stopSnap = 1f;
    [SerializeField]
    private Transform groundedPoint;
    [SerializeField]
    private LayerMask groundedMask;

    public Vector2 moveInput;

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

    public bool Grounded
    {
        get
        {
            return Physics2D.OverlapPoint(groundedPoint.transform.position, groundedMask) != null;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!controller.Stunned)
        {
            if (moveInput.x != 0)
            {
                Vector2 v = rigidbody.velocity;
                v.x = Mathf.Clamp(moveInput.x, -1f, 1f)
                    * ((Grounded) ? runSpeed : airSpeed);
                if(v.x > rigidbody.velocity.x)
                {
                    rigidbody.velocity += Vector2.right * ((Grounded) ? groundAccel : airAccel) * Time.fixedDeltaTime;
                    if(rigidbody.velocity.x > v.x)
                    {
                        Vector2 newV = rigidbody.velocity;
                        newV.x = v.x;
                        rigidbody.velocity = newV;

                    }
                }
                else if (v.x < rigidbody.velocity.x)
                {
                    rigidbody.velocity += Vector2.left * ((Grounded) ? groundAccel : airAccel) * Time.fixedDeltaTime;
                    if (rigidbody.velocity.x < v.x)
                    {
                        Vector2 newV = rigidbody.velocity;
                        newV.x = v.x;
                        rigidbody.velocity = newV;

                    }
                }

                if ((controller.facing > 0 && transform.localScale.x < 0)
                    || (controller.facing < 0 && transform.localScale.x > 0))
                {
                    Vector3 s = transform.localScale;
                    s.x = -s.x;
                    transform.localScale = s;
                }
            }
            else
            {
                rigidbody.AddForce(new Vector2(-rigidbody.velocity.x * stopSnap * runSpeed * (Grounded ? 1 : 0.2f), 0));
            }

            if (Grounded)
            {
                jumps = maxJumps;
            }

            if (moveInput.y > 0.1f)
            {

                if (!jumpHeld && jumps > 0)
                {
                    Vector2 v = rigidbody.velocity;
                    v.y = 0;
                    if (moveInput.x != 0 && Mathf.Sign(moveInput.x) != Mathf.Sign(rigidbody.velocity.x))
                    {
                        v.x = 0;
                    }
                    rigidbody.velocity = v;

                    rigidbody.AddForce(Vector2.up * jumpForce * 50);
                    jumpHeld = true;
                    jumps--;
                }
                else if (jumpHeld && rigidbody.velocity.y > 0)
                {
                    rigidbody.AddForce(Vector2.up * jumpHoldForce);
                }

            }
            else
            {
                jumpHeld = false;
            }
        }
	}
}
