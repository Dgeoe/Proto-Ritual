using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
    public string animationName;
    public float speeds;

    void Start()
    {
        Animation anim = GetComponent<Animation>();
        anim[animationName].speed = speeds;
        anim.Play(animationName);
    }
}
