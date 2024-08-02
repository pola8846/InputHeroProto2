using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "spriteAnimationClip", menuName = "Create Animation Clip")]
public class SpriteAnimationClip : ScriptableObject
{
    public string keycode;
    public List<Sprite> sprites;
    public AnimationClipType clipType = AnimationClipType.Manual;
    public float framePerSec = 8f;

    public enum AnimationClipType
    {
        Manual,
        Timed,
        Single,
    }
}
