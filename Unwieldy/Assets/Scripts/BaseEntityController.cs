using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntityController : MonoBehaviour
{

    public float facing = 1f;
    [SerializeField]
    internal ComboSystem combo;
    [SerializeField]
    internal MoveController move;
    [SerializeField]
    internal CombatEntity entity;
    [SerializeField]
    internal Weapons weapons;

    public abstract bool Stunned { get; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
