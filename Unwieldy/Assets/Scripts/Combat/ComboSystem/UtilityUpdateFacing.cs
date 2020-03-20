using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityUpdateFacing : BaseAttackUtility
{
    [SerializeField]
    private BaseEntityController c;

    public override void Trigger()
    {
        if (c.direction.x != 0)
        {
            c.facing = Mathf.Sign(c.direction.x);

            if (((c.facing > 0 && c.move.transform.localScale.x < 0)
                    || (c.facing < 0 && c.move.transform.localScale.x > 0)))
            {
                Vector3 s = c.move.transform.localScale;
                s.x = -s.x;
                c.move.transform.localScale = s;
            }
        }
    }
}
