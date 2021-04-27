using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Controller : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] _samples;

    public int QSamples = 1024;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 3599, AudioSettings.outputSampleRate);
        while(!(Microphone.GetPosition(Microphone.devices[0]) > 0)) { }
        audioSource.Play();
        _samples = new float[1024]; 
    }

    private void Update()
    {
        audioSource.GetOutputData(_samples, 0);
        foreach (var s in _samples)
        {
            Debug.Log(s);
        }
    }
}
