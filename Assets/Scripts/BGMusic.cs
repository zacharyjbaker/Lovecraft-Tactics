using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    [SerializeField] AudioClip[] bgmusic;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 1);
        this.GetComponent<AudioSource>().clip = bgmusic[rand];
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<AudioSource>().loop = true;
    }
}
