using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ComboSystem : MonoBehaviour {

    

    [SerializeField]
    private AttackNode currentNode;
    [SerializeField]
    private AttackNode bufferNode;

    [SerializeField]
    private BaseNode treeTop;
    //public const int BUFFER_FRAMES = 10;

    public bool ActionLock
    {
        get { return !(currentNode == null || currentNode.IsAttackFinished); }
    }

    public void Attack(InputType type, AttackTrigger trigger)
    {
        AttackNode nextNode = null;
        if (currentNode != null)
            nextNode = (AttackNode)currentNode.GetNextNode(type, trigger);
        if(nextNode == null)
            nextNode = (AttackNode)treeTop.GetNextNode(type, trigger);
        if (nextNode != null)
        {
            if(currentNode == null || currentNode.IsAttackFinished)
            {
                Trigger(nextNode);
            }
            else if(currentNode.GetFramesRemainingInAttack() <= currentNode.bufferFrames)
            {
                bufferNode = nextNode;
            }
        }   
    }

    private void Trigger(AttackNode node)
    {
        //string log = (currentNode != null).ToString() + " " + (bufferNode != null).ToString() 
        //    + " | " + ((currentNode != null) ? currentNode.IsAttackFinished + " " + node.GetFramesRemainingInAttack() + " | "
        //    : "no current | ");
        DetachAttack();
        currentNode = node;
        node.Trigger();
        node.OnAttackFinish += AttackFinish;
        node.OnAttackDispose += AttackDispose;
        //log += currentNode.IsAttackFinished + " " + currentNode.GetFramesRemainingInAttack();
        //Debug.Log(log);
    }

    private void AttackFinish()
    {
        if(bufferNode != null)
        {
            DetachAttack();
            Trigger(bufferNode);
            bufferNode = null;
        }
    }

    private void AttackDispose()
    {
        AttackNode continueAttack = currentNode.GetContinueNode();
        DetachAttack();
        if (continueAttack != null)
        {
            Trigger(continueAttack);
        }
    }

    private void DetachAttack()
    {
        if (currentNode != null)
        {
            currentNode.Kill();
            currentNode.OnAttackDispose -= AttackDispose;
            currentNode.OnAttackFinish -= AttackFinish;
        }
        currentNode = null;
    }

    public void Equip(BaseNode weapon)
    {
        //treeTop.gameObject.SetActive(false);
        treeTop = weapon;
        //treeTop.gameObject.SetActive(true);
    }
}

public enum InputType
{
    Light,
    Prepared,
    Special,
    Throw
}

public enum AttackTrigger
{
    Press,
    Release
}
