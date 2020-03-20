using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxIgnorePushbox : BaseHitbox
{
    [SerializeField]
    private GameObject[] colliders;

    [SerializeField]
    private GameObject[] toDisable;

    [SerializeField]
    private int ignoreTime = 30;
    private int timer;

    [SerializeField]
    private MoveController move;

    [SerializeField]
    private int swapLayer = 12;
    private int oldLayer;
    public override void Trigger()
    {
        timer = ignoreTime;
        foreach(GameObject collider in colliders)
            collider.layer = swapLayer;
        foreach (GameObject collider in toDisable)
            collider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer--;
            if(timer <= 0)
            {
                foreach (GameObject collider in colliders)
                {
                    if (move != null)
                        collider.layer = move.Layer;
                    else
                        collider.layer = oldLayer;
                    
                }
                foreach (GameObject c in toDisable)
                    c.SetActive(true);
            }
        }
    }
}
