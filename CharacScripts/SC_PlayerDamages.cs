using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_PlayerDamages : MonoBehaviour
{

    public int MaxHP, HP;
    public float InvincFrame;
    public float InvincFrameDuration;
    private Animator Playeranim;    
    [SerializeField] Material mat;
    private bool InvulFrameAnimisActive;
    public bool isDead = false;
    public bool isSlow = false;
    private float oldspeed;

    //UI
    public List<Image> hearts = new List<Image>();   
    //private float SpriteTransTime = 2f;


    void Start()
    {
        HP = MaxHP;
        InvincFrameDuration = 0;        
        Playeranim = transform.GetChild(0).GetComponent<Animator>();
        oldspeed = GetComponent<SC_Character>().speed;

        //UI
        for (int i = 0 ; i < GameObject.Find("HealthBar").transform.childCount ; i++)
        {
            hearts.Add(GameObject.Find("HealthBar").transform.GetChild(i).GetComponent<Image>());            
        }

    }

    void Update()
    {
        if (InvincFrameDuration > 0)
        {
            InvincFrameDuration -= Time.deltaTime;
            if (InvulFrameAnimisActive == false)
            {
                mat.color = new Color(1f, 0.5f, 0.5f);
                StartCoroutine(InvulnerabFrame());
            }
        }
        else
        {
            mat.SetColor("_Color", Color.white);
        }

        //UI
        for (int i = 0; i < GameObject.Find("HealthBar").transform.childCount; i++)
        {
            if (i < HP)
            {
                if (HP != 1) { hearts[i].GetComponent<Animator>().SetTrigger("Full"); } //Coeur remplit //{ hearts[i].sprite = fullHeart; }
                else { hearts[i].GetComponent<Animator>().SetTrigger("Last"); }

            }            
            else { hearts[i].GetComponent<Animator>().SetTrigger("Empty"); }

            //Nombre de coeur à afficher
            if (i < MaxHP) { hearts[i].enabled = true; }
            else { hearts[i].enabled = false; }
        }
    }

    public void Damage(int Value)
    {
        if (InvincFrameDuration <= 0)
        {
            if (HP - Value >= 0)
            {
                HP = HP - Value;                            
                Playeranim.Play("DAMAGE");
                
            }
            else { HP = 0; }

            if (HP == 0)
            {
                Death();
            }
            InvincFrameDuration = InvincFrame;
        }
    }

    public void Slow(float Value,float Duration)
    {
        if (isSlow == false)
        {
            GetComponent<SC_Character>().speed = oldspeed * Value;
            isSlow = true;
            StartCoroutine(SlowCor(Duration));
        }              
    }

    private IEnumerator SlowCor(float Duration)
    {
        yield return new WaitForSeconds(Duration);
        GetComponent<SC_Character>().speed = oldspeed;
        isSlow = false;
    }

    public void Heal(int Value)
    {
        HP = HP + Value;
        if (HP > MaxHP) { HP = MaxHP; }
    }

    private void Death()
    {
        isDead = true;
        Heal(MaxHP);
        gameObject.transform.position = GetComponent<SC_Character>().Spawn.transform.position;
        isDead = false;               
    }

    private IEnumerator InvulnerabFrame()
    {
        InvulFrameAnimisActive = true;

        foreach(GameObject GO in gameObject.transform.GetComponent<SC_Character>().PlayerMeshes)
        {
            GO.transform.GetComponent<MeshRenderer>().enabled = false;
        }
        yield return new WaitForSeconds(0.1f);

        foreach (GameObject GO in gameObject.transform.GetComponent<SC_Character>().PlayerMeshes)
        {
            GO.transform.GetComponent<MeshRenderer>().enabled = true;
        }
        yield return new WaitForSeconds(0.15f);
        InvulFrameAnimisActive = false;
    }
}

