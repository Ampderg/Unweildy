using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackNode : BaseNode {

    //[SerializeField]
    protected int elapsedFrames;

    [SerializeField]
    protected int frameDuration;

    //How many frames after the attack finishes can the user press a button to extend the combo?
    [SerializeField]
    protected int comboFrames;

    //[SerializeField]
    public BaseEntityController controller;
    public int bufferFrames = 10;

    public bool IsAttackFinished { get { return isAttackFinished; } }

    public delegate void _onAttackFinish();
    public event _onAttackFinish OnAttackFinish;
    public delegate void _onAttackDispose();
    public event _onAttackDispose OnAttackDispose;

    public override void Trigger()
    {
        base.Trigger();
        elapsedFrames = 0;

        //Debug.Log(Time.frameCount + " - Started attack: " + gameObject.name);
    }

    public int GetFramesRemainingInAttack()
    {
        return frameDuration - elapsedFrames;
    }

    protected override void Update()
    {
        if(active)
        {
            elapsedFrames++;
            if(elapsedFrames > frameDuration)
            {
                if (!isAttackFinished)
                {
                    isAttackFinished = true;

                    OnAttackFinish.Invoke();
                    //Debug.Log(Time.frameCount + " " + elapsedFrames + " - Finished attack: " + gameObject.name);
                }
                if (elapsedFrames > frameDuration + comboFrames)
                {
                    OnAttackDispose.Invoke();
                    active = false;
                    //Debug.Log(Time.frameCount + " - Disposed attack: " + gameObject.name);
                }
            }
        }
    }

    internal AttackNode GetContinueNode()
    {
        if (continueAttack == null) return null;
        return (AttackNode)this.continueAttack.GetNode();
    }
}
