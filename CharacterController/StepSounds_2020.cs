using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSounds : MonoBehaviour
{
    public AudioClip[] step;
    public AudioClip jump;
    public AudioClip land;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void PlayStep()
    {
        AudioClip clip = GetRandomClipStep();
        source.PlayOneShot(clip);
    }

    void PlayJump()
    {
        source.PlayOneShot(jump);
    }

    void PlayLand()
    {
        source.PlayOneShot(land);
    }

    private AudioClip GetRandomClipStep()
    {
        return step[UnityEngine.Random.Range(0, step.Length)];
    }
}
