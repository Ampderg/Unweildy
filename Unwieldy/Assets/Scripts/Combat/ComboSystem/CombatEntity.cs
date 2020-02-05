using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class IDamageable : MonoBehaviour
{
    public abstract void Damage(int gripDamage, int hpDamage);
    public abstract void Stun(int stunFrames);
    public abstract void Launch(Vector2 minVector, Vector2 maxVector, bool allowLaunchStun);
    public abstract void Break(int gripBreak);
    public abstract string GetEntityName();
    public abstract BaseEntityController GetController();

}

class CombatEntity : IDamageable
{
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private new Rigidbody2D rigidbody;
    public static float launchMultiplier = 80f;
    public static float gravityScale = 4f;
    [SerializeField]
    private BaseEntityController controller;
    public float PercentHP
    {
        get { return (float)currentHp / maxHp; }
    }

    public const int MAX_GRIP = 9999;

    [SerializeField]
    private int currentGrip;
    public float Grip
    {
        get {
            if (GripBroken)
                return -1;
            return currentGrip * 0.1f;
        }
    }
    [SerializeField]
    private int stunFrames = 0;
    public bool Stunned
    {
        get { return stunFrames > 0; }
    }
    public bool GripBroken
    {
        get { return currentGrip == -1; }
    }

    public float LaunchFactor
    {
        get
        {
            if (GripBroken)
                return 1f;
            return Mathf.Clamp(currentGrip / 3000f, 0f, 1f);
        }
    }

    private void Awake()
    {
        currentHp = maxHp;
        rigidbody.gravityScale = gravityScale;
    }
    private void Update()
    {
        if (stunFrames > 0)
            stunFrames--;
    }

    public override void Break(int gripBreak)
    {
        if(gripBreak <= currentGrip)
        {
            //BREAK GRIP
            Debug.Log("Grip Broken!");
            currentGrip = -1;
            if(controller != null)
                controller.combo.Equip(controller.weapons.unarmed);
        }
    }

    public override void Damage(int gripDamage, int hpDamage)
    {
        if (hpDamage < 0) //healing hp should happen even if grip isn't broken
        {
            currentHp -= hpDamage;
            if (currentHp > maxHp) currentHp = maxHp;
        }

        if (!GripBroken)
        {
            currentGrip += gripDamage;
            if (currentGrip < 0) currentGrip = 0; //in case of grip healing
        }
        else if(hpDamage > 0)
        {
            currentHp -= hpDamage;
            if (currentHp <= 0)
                Kill();
        }
    }

    private const float BIG_LAUNCH_RANDOM = 0.07f;
    public override void Launch(Vector2 minVector, Vector2 maxVector, bool allowLaunchStun)
    {
        Vector2 v = Vector2.Lerp(minVector, maxVector, LaunchFactor) * launchMultiplier;
        v.y *= gravityScale;
        rigidbody.velocity = Vector2.zero;
        if (allowLaunchStun && v.magnitude > 10 * launchMultiplier)
            StartCoroutine(BigLaunch(v));
        else
            rigidbody.AddForce(v);
    }

    private IEnumerator BigLaunch(Vector2 v)
    {
        Vector3 pos = transform.position;
        for(float t = 0; t < Mathf.Min(0.2f, Mathf.Pow(v.magnitude / (60 * launchMultiplier), 2)); t += Time.deltaTime)
        {
            //rigidbody.AddForce(-rigidbody.velocity);
            rigidbody.MovePosition(pos + 
                new Vector3(UnityEngine.Random.Range(-BIG_LAUNCH_RANDOM, BIG_LAUNCH_RANDOM), 
                UnityEngine.Random.Range(-BIG_LAUNCH_RANDOM, BIG_LAUNCH_RANDOM), 0));
            yield return null;
        }
        yield return null;
        rigidbody.AddForce(v);
    }

    public override void Stun(int stunFrames)
    {
        if (stunFrames > this.stunFrames)
            this.stunFrames = stunFrames;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public override string GetEntityName()
    {
        return gameObject.name;
    }

    public override BaseEntityController GetController()
    {
        return controller;
    }
}

