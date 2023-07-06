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

            float DifX = gameObject.transform.position.x - Player.transform.position.x; //horizontal delta
            float DifY = gameObject.transform.position.y - Player.transform.position.y; //vertical delta

            if (DifX < 0) //Player is on the right of the ennemy
            {
                Player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * (Value / 4) + Vector3.right * Value);
            }
            else  //Player is on the left of the ennemy
            {
                Player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * (Value / 4) + Vector3.left * Value);
            }
        }
    }
}
