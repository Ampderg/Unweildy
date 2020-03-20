using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityMoveDrift : BaseAttackUtility
{
    private Coroutine c;
    [SerializeField]
    private MoveController move;
    [SerializeField]
    private float driftSpeed = 0.5f;
    [SerializeField]
    private float accelSpeed = 1f;
    [SerializeField]
    private float decelSpeed = 1f;
    [SerializeField]
    private AttackNode node;

    public override void Trigger()
    {
        if(c == null)
            c = StartCoroutine(DriftCoroutine());
    }

    private IEnumerator DriftCoroutine()
    {
        BaseMove.SpeedModifier sm = new BaseMove.SpeedModifier();
        BaseMove.SpeedModifier am = new BaseMove.SpeedModifier();
        BaseMove.SpeedModifier dm = new BaseMove.SpeedModifier();

        sm.multiplier = driftSpeed;
        move.speedMods.Add(sm);
        if (accelSpeed != 1f)
        {
            am.multiplier = accelSpeed;
            move.speedAccelMods.Add(am);
        }
        if (decelSpeed != 1f)
        {
            dm.multiplier = decelSpeed;
            move.speedDecelMods.Add(dm);
        }
        while (!node.IsAttackFinished && node.controller.IsAlive)
        {
            Vector2 inp = move.rawInput;
            inp.y = 0;
            move.moveInput = inp;
            yield return null;
        }
        move.speedMods.Remove(sm);

        if (accelSpeed != 1f)
            move.speedAccelMods.Remove(am);
        if (decelSpeed != 1f)
            move.speedDecelMods.Remove(dm);

        c = null;
    }
}
