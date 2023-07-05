using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SoundsEffect : MonoBehaviour
{

    private int nSounds;

    private void Start()
    {
        nSounds = transform.childCount;
    }

    public void Play()
    {
        int rng;

        rng = Random.Range(0, nSounds);
        transform.GetChild(rng).GetComponent<AudioSource>().Play();
    }
}
