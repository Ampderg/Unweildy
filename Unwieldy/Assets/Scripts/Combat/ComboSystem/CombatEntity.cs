using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public static float gravityScale = 3.7f;
    public static float hitGravityCompensation = 2f;

    [SerializeField]
    private GameObject weaponDropPrefab;

    private bool gripBroken;

    public UnityEvent OnRespawn;

    [SerializeField]
    private float respawnDelay = 3;

    [SerializeField]
    private BaseEntityController controller;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text hpText;
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

    [SerializeField]
    private float verticalBounceFactor = 0.6f;

    private bool inactive = false;

    public bool Stunned
    {
        get { return stunFrames > 0 || inactive; }
    }
    public bool GripBroken
    {
        get { return gripBroken; }
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

    public bool IsDead { get { return killCo != null; } }

    private void Awake()
    {
        currentHp = maxHp;
        rigidbody.gravityScale = gravityScale;
    }
    private void Update()
    {
        if (!inactive)
        {
            if(gripBroken && currentGrip > 500 && UnityEngine.Random.Range(500, MAX_GRIP) < currentGrip)
            {
                currentGrip--;
            }

            if (stunFrames > 0)
                stunFrames--;

            if (healthText != null)
            {
                if (GripBroken)
                {
                    healthText.text = "BROKEN";
                }
                else
                {
                    healthText.text = string.Format("{0:0.0}%", Grip);
                }
            }
            if(hpText != null)
            {
                if(gripBroken || currentHp != maxHp)
                {
                    hpText.text = "HP: " + string.Format("{0:0.0}%", currentHp * 0.1f);
                }
                else
                {
                    hpText.text = "";
                }
            }
        }
        else
        {
            if(hpText != null)
                hpText.text = "";
        }
    }

    public override void Break(int gripBreak)
    {
        if(!gripBroken && gripBreak <= currentGrip)
        {
            //BREAK GRIP
            //Debug.Log("Grip Broken!");
            gripBroken = true;
            if (controller != null && controller.combo != null)
            {
                GameObject o = Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);
                o.GetComponent<Rigidbody2D>().AddForce(GetWeaponDropForce());
                o.GetComponent<ItemWeaponPickup>().weapon = controller.combo.currentWeapon;

                controller.combo.Equip("unarmed");
            }
        }
    }

    private Vector2 GetWeaponDropForce()
    {
        return new Vector2(UnityEngine.Random.Range(0.1f, 1) * Mathf.Sign(UnityEngine.Random.Range(-1, 1)), UnityEngine.Random.Range(-0.1f, 1.5f)).normalized * 600;
    }

    public void FixGrip()
    {
        gripBroken = false;
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
        v.y *= hitGravityCompensation;
        if (v.y < 0 && controller.move.Grounded)
        {
            v.y *= -verticalBounceFactor;
        }
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
        if(killCo == null) killCo = StartCoroutine(KillCoroutine());
    }

    private Coroutine killCo;

    private IEnumerator KillCoroutine()
    {
        if(healthText != null)
            healthText.text = "DEAD";
        rigidbody.transform.position = Vector3.down * 100;
        inactive = true;
        rigidbody.simulated = false;
        stunFrames = 0;

        yield return new WaitForSeconds(respawnDelay);

        rigidbody.simulated = true;
        rigidbody.velocity = Vector3.zero;

        currentHp = maxHp;
        currentGrip = 0;
        OnRespawn.Invoke();
        transform.position = StageHandler.instance.GetRespawnPosition();
        gripBroken = false;

        inactive = false;
        killCo = null;
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

