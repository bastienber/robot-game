using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_EnemyDamages : MonoBehaviour
{

    public int MaxHP, HP;
    public float InvincFrame;
    public float InvincFrameDuration;
    ParticleSystem FXDamage, FXDeath;
    public bool isDead = false;
    
    void Start()
    {
        HP = MaxHP;
        InvincFrameDuration = 0;
        FXDamage = transform.Find("FX/P_Damage").GetComponent<ParticleSystem>();
        FXDeath = transform.Find("FX/P_Death").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (InvincFrameDuration > 0)
        {
            InvincFrameDuration -= Time.deltaTime;
        }
    }

    public void Damage(int Value)
    {
        if (InvincFrameDuration <= 0)
        {
            if (HP - Value >= 0)
            {
                HP = HP - Value;
                FXDamage.Play();

            }
            else { HP = 0; }

            if (HP == 0)
            {
                StartCoroutine(Death());
            }
            InvincFrameDuration = InvincFrame;
        }
    }



    public void Heal(int Value)
    {
        if (HP + Value >= MaxHP)
        {
            HP = HP + Value;
        }
        else { HP = MaxHP; }
    }

    private IEnumerator Death()
    {
        isDead = true;
        FXDeath.Play();


        yield return new WaitForSeconds(0.01f);
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        
        GameObject.Find("Melee_Trigger").GetComponent<SC_DetectEnemyLayer>().EnemiesDetected.Remove(gameObject);
        Destroy(gameObject, FXDeath.main.duration);

    }
}






