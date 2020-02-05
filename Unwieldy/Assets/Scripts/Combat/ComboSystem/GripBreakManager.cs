using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum GripBreak
{
    Disabled, //never
    None, //400-600
    Weak, //200-400
    Medium, //150-300
    Strong, //90-150
    Extreme, //60-100
    Ultra, //40-50
    Perfect, //15-25
    Guarenteed //0
}
public static class GripBreakManager
{
    public static int GetGripBreak(GripBreak gripBreak)
    {
        switch(gripBreak)
        {
            default:
            case GripBreak.Disabled:
                return CombatEntity.MAX_GRIP + 1;
            case GripBreak.None:
                return UnityEngine.Random.Range(4000, 6000);
            case GripBreak.Weak:
                return UnityEngine.Random.Range(2000, 4000);
            case GripBreak.Medium:
                return UnityEngine.Random.Range(1500, 3000);
            case GripBreak.Strong:
                return UnityEngine.Random.Range(900, 1500);
            case GripBreak.Extreme:
                return UnityEngine.Random.Range(600, 1000);
            case GripBreak.Ultra:
                return UnityEngine.Random.Range(400, 500);
            case GripBreak.Perfect:
                return UnityEngine.Random.Range(150, 250);
            case GripBreak.Guarenteed:
                return 0;
        }
    }

    public static int GetRandomRange(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
}

