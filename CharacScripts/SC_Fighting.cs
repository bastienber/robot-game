using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Fighting : MonoBehaviour
{
    private int ComboCount;
    public int Damage,maxCombo;
    public float AtkSpeed;
    private float timeInput, basetimeInput;
    private bool firstatk;
    private Rigidbody2D r2d;
    private List<GameObject> Targets;
    private LayerMask enemyLayer;
    private Transform attackPoint;

    private Animator anim;

    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        firstatk = true;
        ComboCount = 1;
        basetimeInput = 2/AtkSpeed;
        timeInput = 0;
        Targets = transform.Find("TRIGGER/Melee_Trigger").gameObject.GetComponent<SC_DetectEnemyLayer>().EnemiesDetected;
    }


    void Update()
    {        
        if (timeInput > 0)
        {
            timeInput -= Time.deltaTime;
        }
        else
        {
        ComboCount = 1;
        }

        if (Input.GetButtonDown("MeleeAttack"))
        {
            Attack();
        }      
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("MELEE_C1") || anim.GetCurrentAnimatorStateInfo(0).IsName("MELEE_C2") || anim.GetCurrentAnimatorStateInfo(0).IsName("MELEE_C3"))
        {
            anim.SetBool("isAttacking", true);
        }
        else { anim.SetBool("isAttacking", false); }


    }

    private void Attack()
    {   
        if((ComboCount == 1 && timeInput <= 0) || (ComboCount > 1 && timeInput <= basetimeInput/2))
        {            
            StartCoroutine(inertia());            
            anim.Play("MELEE_C" + ComboCount.ToString());

            if (Targets.Count > 0)
            {
                for (int i = 0; i < Targets.Count; i++)
                {
                    if (Targets[i].tag == "Enemy")
                    {                        
                        if (ComboCount != 3)
                        {
                            Targets[i].GetComponent<SC_EnemyDamages>().Damage(Damage);
                        }
                        else { Targets[i].GetComponent<SC_EnemyDamages>().Damage(Damage*2); }                        
                    }
                }
            }
            if (ComboCount < maxCombo)
            {                
                ComboCount += 1;
                timeInput = basetimeInput;
            }
            else
            {
                ComboCount = 1;
                timeInput = basetimeInput * 2;
            }
        }            
    }

    private IEnumerator inertia()
    {
        GetComponent<SC_Character>().speed = 3.5f;
        r2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        r2d.gravityScale = 1f;  

        yield return new WaitForSeconds(basetimeInput/2f);
        
        GetComponent<SC_Character>().speed = 7f;        
        r2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        r2d.gravityScale = 5f;
    }
}
