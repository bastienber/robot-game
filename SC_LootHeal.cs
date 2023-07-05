using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LootHeal : MonoBehaviour
{
    [SerializeField] int HealValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<SC_PlayerDamages>().Heal(HealValue);
            Destroy(gameObject);
        }
    }
}
