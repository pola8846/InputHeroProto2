using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class TestSoundChanger : MonoBehaviour
{
    public StudioEventEmitter emitter;
    [Range(0,3)]
    public int i;

    // Update is called once per frame
    void Update()
    {
        emitter.SetParameter("Parameter 3", i);
    }
}
