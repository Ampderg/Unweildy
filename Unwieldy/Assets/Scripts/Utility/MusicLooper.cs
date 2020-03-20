using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLooper : MonoBehaviour
{

    [SerializeField]
    private GameObject loopSourcePrefab;

    [SerializeField]
    private AudioClip[] musicElements;

    // Start is called before the first frame update
    void Awake()
    {
        float delay = 0f;
        for(int i = 0; i < musicElements.Length; i++)
        {
            AudioSource s = Instantiate(loopSourcePrefab, transform).GetComponent<AudioSource>();
            s.clip = musicElements[i];
            s.PlayDelayed(delay);
            delay += musicElements[i].length;
            if (i == musicElements.Length - 1)
                s.loop = true;
        }

    }

    // Update is called once per frame
    //void Update()
    //{
    //    if(clipIndex >= 0 && !musicSource.isPlaying && clipIndex < musicElements.Length)
    //    {
    //        if (musicElements[clipIndex].loopCount <= clipLoopCount)
    //        {
    //            if (musicElements[clipIndex].loopCount == -1)
    //            {
    //                Debug.Log("2");
    //                musicSource.loop = true;
    //                musicSource.clip = musicElements[clipIndex].clip;
    //                musicSource.Play();
    //                clipIndex = -1;
    //                return;
    //            }

    //            clipIndex++;
    //            clipLoopCount = 0;

    //            Debug.Log("1");

                
    //        }

    //        musicSource.clip = musicElements[clipIndex].clip;
    //        clipLoopCount++;
    //        musicSource.Play();
    //    }
    //}
}
