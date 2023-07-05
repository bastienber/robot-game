using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DetectCollision : MonoBehaviour
{
    public bool isCollided, playerDetected;
    
    public List<GameObject> GameObjectsCollided = new List<GameObject>();
    private List<string> TagToIgnore = new List<string>() { "Player", "Trigger", "Projectile", "Loot" };

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (TagToIgnore.Contains(collision.tag) == false)
        {
            GameObjectsCollided.Add(collision.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (TagToIgnore.Contains(collision.tag) == false)
        {
            isCollided = true;
        }
        if (collision.tag == "Player")
        {
            playerDetected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {      
        if (collision.tag == "Player")
        {
            playerDetected = false;
        }
        else
        {
            isCollided = false;
            GameObjectsCollided.Remove(collision.gameObject);
        }        
    }
}
