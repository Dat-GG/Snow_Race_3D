using UnityEngine;
public class PlayerCharacter : MonoBehaviour
{
    public Animator animator;
    private readonly int _idle = Animator.StringToHash("Idle");
    private readonly int _small = Animator.StringToHash("Small");
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly int _stun = Animator.StringToHash("Stun");
    private readonly int _run = Animator.StringToHash("Run");
    private readonly int _lift = Animator.StringToHash("Lift");
    private readonly int _kick = Animator.StringToHash("Kick");
    private readonly int _lava = Animator.StringToHash("Lava");
    private readonly int _water = Animator.StringToHash("Water");
    private readonly int _slide = Animator.StringToHash("Slide");
    private readonly int _fly = Animator.StringToHash("Fly");
    private readonly int _ski = Animator.StringToHash("Ski");
    
    internal void AnimateIdle()
    {
        animator.SetTrigger(_idle);
    }
    
    internal void AnimateWalkSmall()
    {
        animator.SetTrigger(_small);
    }

    internal void AnimateWalkPush()
    {
        animator.SetTrigger(_walk);
    }
    
    internal void AnimateStun()
    {
        animator.SetTrigger(_stun);
    }
    
    internal void AnimateRunBack()
    {
        animator.SetTrigger(_run);
    }
    
    internal void AnimateLiftUp()
    {
        animator.SetTrigger(_lift);
    }
    
    internal void AnimateKick()
    {
        animator.SetTrigger(_kick);
    }
    
    internal void AnimateWalkLava()
    {
        animator.SetTrigger(_lava);
    }
    
    internal void AnimateWalkWater()
    {
        animator.SetTrigger(_water);
    }
    
    internal void AnimateSlide()
    {
        animator.SetTrigger(_slide);
    }
    
    internal void AnimateFly()
    {
        animator.SetTrigger(_fly);
    }

    internal void AnimateSki()
    {
        animator.SetTrigger(_ski);
    }
}
