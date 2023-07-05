using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_InvisibleObjects : MonoBehaviour
{

    void Start()
    {
        foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Invisible"))
        {
            Go.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
