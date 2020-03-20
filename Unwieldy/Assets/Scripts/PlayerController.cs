using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerController : BaseEntityController
{

    [SerializeField]
    private XboxController controller;

    [SerializeField]
    private int jumpBufferLength = 10;
    private int jumpBuffer = 0;

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
        jumpBuffer--;
        if (!Stunned)
        {
            Vector2 mv = Vector2.zero;

            if (controller == XboxController.Keyboard)
            {
                //keyboard input
                mv = new Vector2(Input.GetAxisRaw("Horizontal"), (Input.GetButton("Jump")) ? 1 : 0);
                
                direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                direction.Normalize();

                if (Input.GetButtonDown("Fire1")) combo.Attack(InputType.Light, AttackTrigger.Press);
                else if (Input.GetButtonDown("Fire2")) combo.Attack(InputType.Prepared, AttackTrigger.Press);
                else if (Input.GetButtonDown("Fire3")) combo.Attack(InputType.Special, AttackTrigger.Press);
                else if (Input.GetButtonDown("Throw")) combo.Attack(InputType.Throw, AttackTrigger.Press);
                else if (Input.GetButtonUp("Fire1")) combo.Attack(InputType.Light, AttackTrigger.Release);
                else if (Input.GetButtonUp("Fire2")) combo.Attack(InputType.Prepared, AttackTrigger.Release);
                else if (Input.GetButtonUp("Fire3")) combo.Attack(InputType.Special, AttackTrigger.Release);
                else if (Input.GetButtonUp("Throw")) combo.Attack(InputType.Throw, AttackTrigger.Release);
            }
            else
            {
                mv = new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX), (XCI.GetButton(XboxButton.A)) ? 1 : 0);

                direction = new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX), XCI.GetAxisRaw(XboxAxis.LeftStickY));
                direction.Normalize();

                if (XCI.GetButtonDown(XboxButton.X)) combo.Attack(InputType.Light, AttackTrigger.Press);
                else if (XCI.GetButtonDown(XboxButton.B)) combo.Attack(InputType.Prepared, AttackTrigger.Press);
                else if (XCI.GetButtonDown(XboxButton.Y)) combo.Attack(InputType.Special, AttackTrigger.Press);
                else if (XCI.GetButtonDown(XboxButton.RightBumper)
                    || XCI.GetButtonDown(XboxButton.LeftBumper)) combo.Attack(InputType.Throw, AttackTrigger.Press);
                else if (XCI.GetButtonUp(XboxButton.X)) combo.Attack(InputType.Light, AttackTrigger.Release);
                else if (XCI.GetButtonUp(XboxButton.B)) combo.Attack(InputType.Prepared, AttackTrigger.Release);
                else if (XCI.GetButtonUp(XboxButton.Y)) combo.Attack(InputType.Special, AttackTrigger.Release);
                else if (XCI.GetButtonUp(XboxButton.RightBumper)
                    || XCI.GetButtonUp(XboxButton.LeftBumper)) combo.Attack(InputType.Throw, AttackTrigger.Release);
            }


            Vector2 raw = mv;

            if (combo.ActionLock)
            {
                if (move.Grounded)
                {
                    mv = Vector2.zero;
                }
                else
                {
                    if (mv.y > 0)
                    {
                        jumpBuffer = jumpBufferLength;
                        mv.y = 0;
                    }
                }
            }
            else
            {
                if (jumpBuffer > 0)
                    mv.y = 1;
            }

            

            move.moveInput = mv;
            move.rawInput = raw;

            if (move.moveInput.x != 0 && ((MoveController)move).FacingUnlock)
                facing = Mathf.Sign(move.moveInput.x);

        }
    }
}
