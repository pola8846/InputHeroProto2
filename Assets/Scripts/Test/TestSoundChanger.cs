using FMODUnity;
using UnityEngine;

public class TestSoundChanger : MonoBehaviour
{
    public StudioEventEmitter emitter;
    [Range(0, 3)]
    public int i;

    // Update is called once per frame
    void Update()
    {
        emitter.SetParameter("Parameter 3", i);
    }
}
