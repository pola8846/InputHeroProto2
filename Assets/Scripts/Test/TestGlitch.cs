using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using URPGlitch.Runtime.AnalogGlitch;
using URPGlitch.Runtime.DigitalGlitch;
using System;
using DG.Tweening;

public class TestGlitch : MonoBehaviour
{
    public enum GlitchType
    {
        NONE,
        Hurt,
        Death
    }

    [Serializable]
    public struct GlitchOptionSet
    {
        public float changeDuration;

        // analog
        public float scanLineJitter;
        public float verticalJump;
        public float horizontalShake;
        public float colorDrift;

        // digital
        public float digitalGlitchIntensity;
    }

    [Serializable]
    public class Glitch
    {
        public GlitchType type;
        public GlitchOptionSet options;
    }

    private AnalogGlitchVolume analog;
    private DigitalGlitchVolume digital;

    public List<Glitch> glitches = new List<Glitch>();
    public List<Tweener> tweeners = new List<Tweener>();

    private GlitchType cache;
    public GlitchType currentGlitch;

    void Start()
    {
        GetComponent<Volume>().profile.TryGet(out analog);
        GetComponent<Volume>().profile.TryGet(out digital);

        GlitchOptionSet? nullableOption = GetGlitch(currentGlitch);

        if (nullableOption.HasValue)
        {
            GlitchOptionSet currentOption = nullableOption.Value;
            ApplyGlitch(currentOption);
        }
        GameManager.SetGlitchEffect(this);
        
    }

    GlitchOptionSet? GetGlitch(GlitchType type)
    {
        foreach (Glitch glitch in glitches)
        {
            if (type == glitch.type)
            {
                return glitch.options;
            }
        }
        return null;
    }

    void ApplyGlitch(GlitchOptionSet options)
    {
        foreach (Tweener tweener in tweeners)
        {
            tweener.Kill();
        }
        tweeners.Clear();

        tweeners.Add(DOTween.To(() => analog.scanLineJitter.value, x => analog.scanLineJitter.value = x, options.scanLineJitter, options.changeDuration));
        tweeners.Add(DOTween.To(() => analog.verticalJump.value, x => analog.verticalJump.value = x, options.verticalJump, options.changeDuration));
        tweeners.Add(DOTween.To(() => analog.horizontalShake.value, x => analog.horizontalShake.value = x, options.horizontalShake, options.changeDuration));
        tweeners.Add(DOTween.To(() => analog.colorDrift.value, x => analog.colorDrift.value = x, options.colorDrift, options.changeDuration));
        tweeners.Add(DOTween.To(() => digital.intensity.value, x => digital.intensity.value = x, options.digitalGlitchIntensity, options.changeDuration));
        //analog.scanLineJitter.value = options.scanLineJitter;
        //analog.verticalJump.value = options.verticalJump;
        //analog.horizontalShake.value = options.horizontalShake;
        //analog.colorDrift.value = options.colorDrift;
        //digital.intensity.value = options.digitalGlitchIntensity;
    }

    void Update()
    {
        if (currentGlitch != cache)
        {
            GlitchOptionSet? nullableOption = GetGlitch(currentGlitch);

            if (nullableOption.HasValue)
            {
                GlitchOptionSet currentOption = nullableOption.Value;
                ApplyGlitch(currentOption);
            }
        }

        cache = currentGlitch;
    }
}
