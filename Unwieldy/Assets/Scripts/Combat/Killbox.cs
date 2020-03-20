using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<CombatEntity>() != null)
        {
            collision.GetComponentInParent<CombatEntity>().Kill();
            Debug.Log("Killed " + collision.gameObject.name + " in killbox");
        }
        else if(collision.GetComponent<BaseItem>())
        {
            Destroy(collision.gameObject);
        }
    }
}
