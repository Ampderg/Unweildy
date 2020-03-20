using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAudioFinish : MonoBehaviour
{
    public AudioSource source;

    public float delay = 0.5f;
    private float timer;

    private void Awake()
    {
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
                Destroy(gameObject);
        }
        else
        {
            timer = delay;
        }
    }
}
