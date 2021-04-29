using UnityEngine;

namespace LCHelp
{
    public class LCUtil
    {
        public static float GetAnimClipTime(Animator animator,string name)
        {
            if (animator == null)
            {
                return 0;
            }
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == name)
                {
                    return clip.length;
                }
            }
            return 0;
        }
    }
}