using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Anim2D", menuName = "AnimationData", order = 1)]
public class AnimationData : ScriptableObject
{
    public List<AnimFrame> DownAnimationFrames;
    public List<AnimFrame> UpAnimationFrames;
    public List<AnimFrame> SideAnimationFrames;
    public bool Looping;
    public int LoopStart;
    public int LoopEnd;

}

