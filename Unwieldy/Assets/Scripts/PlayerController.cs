using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : BaseEntityController
{

    [SerializeField]
    private XboxController controller;

    

    public override bool Stunned
    {
        get
        {
            return entity.Stunned;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Stunned)
        {
            if (controller == XboxController.Keyboard)
            {
                //keyboard input
                Vector2 mv = new Vector2(Input.GetAxisRaw("Horizontal"), (Input.GetButton("Jump")) ? 1 : 0);
                if (combo.ActionLock)
                {
                    if(move.Grounded)
                    {
                        mv = Vector2.zero;
                    }
                    else
                    {
                        mv.y = 0;
                    }
                }
                move.moveInput = mv;
                if (move.moveInput.x != 0)
                    facing = Mathf.Sign(move.moveInput.x);

                if (Input.GetButtonDown("Fire1")) combo.Attack(InputType.Light, AttackTrigger.Press);
                else if (Input.GetButtonDown("Fire2")) combo.Attack(InputType.Prepared, AttackTrigger.Press);
                else if (Input.GetButtonDown("Fire3")) combo.Attack(InputType.Special, AttackTrigger.Press);
                else if (Input.GetButtonDown("Throw")) combo.Attack(InputType.Throw, AttackTrigger.Press);
                else if (Input.GetButtonUp("Fire1")) combo.Attack(InputType.Light, AttackTrigger.Release);
                else if (Input.GetButtonUp("Fire2")) combo.Attack(InputType.Prepared, AttackTrigger.Release);
                else if (Input.GetButtonUp("Fire3")) combo.Attack(InputType.Special, AttackTrigger.Release);
                else if (Input.GetButtonUp("Throw")) combo.Attack(InputType.Throw, AttackTrigger.Release);
            }
        }
    }
}
