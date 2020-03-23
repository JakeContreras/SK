using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip Smashhitsound, Deathsound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        Smashhitsound = Resources.Load<AudioClip>("Woosh");
        Deathsound = Resources.Load<AudioClip>("Death");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch (clip) {
            case "Woosh":
                audioSrc.PlayOneShot (Smashhitsound);
                break;
            case "Death":
                audioSrc.PlayOneShot (Deathsound);
                break;
        }
    }
}
