using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxIgnorePushbox : BaseHitbox
{
    [SerializeField]
    private new GameObject collider;

    [SerializeField]
    private float ignoreTime = 0.5f;
    private float timer;

    [SerializeField]
    private int swapLayer = 12;
    private int oldLayer;
    public override void Trigger()
    {
        oldLayer = collider.layer;
        timer = ignoreTime;
        collider.layer = swapLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                collider.layer = oldLayer;
            }
        }
    }
}
