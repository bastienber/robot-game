﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DetectEnemyLayer : MonoBehaviour
{
    public List<GameObject> EnemiesDetected = new List<GameObject>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemiesDetected.Add(collision.gameObject);
        }
    }    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemiesDetected.Remove(collision.gameObject);
        }
    }
}
