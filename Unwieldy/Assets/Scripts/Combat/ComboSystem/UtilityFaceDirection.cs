using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackUtility : MonoBehaviour
{
    public abstract void Trigger();
}

public class UtilityFaceDirection : BaseAttackUtility
{
    public BaseEntityController pc;
    public Transform targetTransform;

    [SerializeField]
    protected BaseHitbox[] launchHitboxes;

    [SerializeField]
    protected float minLaunchMagnitude = 5;
    [SerializeField]
    protected float maxLaunchMagnitude = 10;

    [SerializeField]
    protected bool fixedLaunchAngle = true;

    //angles lower than this will spike or be adjusted upwards
    [SerializeField]
    [Range(-1f, 1f)]
    protected float liftLimit = 0;

    [SerializeField]
    [Range(-1f, 1f)]
    protected float juggleCap = 0.6f;

    //angles lower than this will be set to the spike value
    [SerializeField]
    [Range(-1f, 1f)]
    protected float spikeLimit = -0.85f;
    [SerializeField]
    protected float spikeMultiplier = 1;



    public override void Trigger()
    {
        if(targetTransform != null)
            targetTransform.up = pc.direction;

        if (launchHitboxes != null)
        {
            for (int i = 0; i < launchHitboxes.Length; i++)
            {
                Vector2 launch = pc.direction;
                if(fixedLaunchAngle)
                {

                    if (launch.y <= spikeLimit)
                    {
                        launch = Vector2.down * spikeMultiplier;
                    }
                    else if (launch.y < liftLimit)
                    {
                        launch.y = liftLimit;
                        //launch.Normalize();
                    }
                    else if (launch.y > juggleCap)
                        launch.y = juggleCap;
                }
                launchHitboxes[i].SetLaunch(launch * minLaunchMagnitude, launch * maxLaunchMagnitude);
            }
        }
    }
}
