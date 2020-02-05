using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGetNode : MonoBehaviour
{
    public abstract IGetNode GetNextNode(InputType input, AttackTrigger trigger);

    public abstract IGetNode GetNode();
}

public class SplitterGrounded : IGetNode
{

    [SerializeField]
    private IGetNode groundedAttack;
    [SerializeField]
    private IGetNode aerialAttack;
    [SerializeField]
    private MoveController cc;

    override public IGetNode GetNextNode(InputType input, AttackTrigger trigger)
    {
        return null;
    }

    override public IGetNode GetNode()
    {
        return cc.Grounded ? groundedAttack.GetNode() : aerialAttack.GetNode(); 
    }
}
