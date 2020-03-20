using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectDict : SerializableDictionary<string, GameObject> { }

public class Weapons : MonoBehaviour
{

    [SerializeField]
    protected ObjectDict weapons;

    public BaseNode GetWeapon(string v)
    {
        return weapons[v].GetComponent<BaseNode>();
    }

    public BaseNode EquipWeapon(string v, ComboSystem combo)
    {
        GameObject o = Instantiate(weapons[v], combo.transform);
        return o.GetComponent<BaseNode>();
    }
}
