using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Enemy : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Knockback(1, collision.gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<SC_PlayerDamages>().Damage(1);           
        }
    }

    private void Knockback(float Value,GameObject Player)
    {
        if (Player.GetComponent<SC_PlayerDamages>().InvincFrameDuration <= 0)
        {
            Value *= 1500;

            float DifX = gameObject.transform.position.x - Player.transform.position.x; //Différence horizontale
            float DifY = gameObject.transform.position.y - Player.transform.position.y; //Différence verticale

            if (DifX < 0)     //Joueur à droite de l'ennemi
            {
                Player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * (Value / 4) + Vector3.right * Value);
            }
            else  //Joueur à gauche de l'ennemi
            {
                Player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * (Value / 4) + Vector3.left * Value);
            }
        }
    }
}
