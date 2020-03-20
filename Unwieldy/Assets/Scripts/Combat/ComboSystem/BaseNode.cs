using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : IGetNode
{

    [SerializeField]
    private IGetNode lightPress;
    [SerializeField]
    private IGetNode lightRelease;
    [SerializeField]
    private IGetNode preparedPress;
    [SerializeField]
    private IGetNode preparedRelease;
    [SerializeField]
    private IGetNode specialPress;
    [SerializeField]
    private IGetNode specialRelease;
    [SerializeField]
    private IGetNode throwPress;
    [SerializeField]
    private IGetNode throwRelease;
    [SerializeField]
    protected IGetNode continueAttack;

    [SerializeField]
    protected BaseAttackUtility[] utilities;

    [SerializeField]
    protected bool active;
    public bool Active { get { return active; } }
    [SerializeField]
    protected bool isAttackFinished;

    [SerializeField]
    protected BaseEffect effect;

    public virtual void Trigger()
    {
        active = true;
        isAttackFinished = false;

        if (effect != null)
            effect.Trigger();

        if(utilities != null)
        {
            for(int i = 0; i < utilities.Length; i++)
            {
                utilities[i].Trigger();
            }
        }
    }

    override public IGetNode GetNode()
    {
        return this;
    }

    override public IGetNode GetNextNode(InputType type, AttackTrigger trigger)
    {
        switch (type)
        {
            case InputType.Light:
                if (trigger == AttackTrigger.Press)
                    return (lightPress != null) ? lightPress.GetNode() : null;
                else
                    return (lightRelease != null) ? lightRelease.GetNode() : null;
            case InputType.Prepared:
                if (trigger == AttackTrigger.Press)
                    return (preparedPress != null) ? preparedPress.GetNode() : null;
                else
                    return (preparedRelease != null) ? preparedRelease.GetNode() : null;
            case InputType.Special:
                if (trigger == AttackTrigger.Press)
                    return (specialPress != null) ? specialPress.GetNode() : null;
                else
                    return (specialRelease != null) ? specialRelease.GetNode() : null;
            case InputType.Throw:
                if (trigger == AttackTrigger.Press)
                    return (throwPress != null) ? throwPress.GetNode() : null;
                else
                    return (throwRelease != null) ? throwRelease.GetNode() : null;
        }
        return null;
    }

    public virtual void Kill()
    {
        
        active = false;
        isAttackFinished = true;
        //Debug.Log(Time.frameCount + " - Killed attack: " + gameObject.name);

    }

    protected virtual void Update() { }
}
