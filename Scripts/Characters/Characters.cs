using UnityEngine;

public class Characters : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rigidbody;
    [SerializeField] protected AudioSource audio;

    [Header("Movement")]
    [SerializeField] protected float speedRun = 5f;
    [SerializeField] protected float walkSpeed = 12f;
    [SerializeField] protected float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask groundLayer;

    protected bool isGrounded = false;
    protected CapsuleCollider capsuleCollider;

    protected Vector3 characterPosition;
    protected float oneStep;

    public enum LaneState
    {
        Right,
        Mid,
        Left
    }

    public LaneState laneState;

    public virtual void Start()
    {
        oneStep = LevelManager.Instance.oneStep;
    }

    public virtual void FixedUpdate()
    {
        CharacterRunning();
    }

    private void CharacterRunning()
    {
        // Move the character continuously forward
        rigidbody.linearVelocity = new Vector3(
            0f,
            rigidbody.linearVelocity.y,
            speedRun * Time.fixedDeltaTime);
    }

    protected void Jump(bool playAnimation)
    {
        if (!IsGrounded())
            return;

        if (playAnimation)
        {
            AudioManager.PlaySound(audio, AudioManager.Instance.jumpPlayer);
            animator.SetTrigger(GameStrings.IsJump);
        }

        rigidbody.linearVelocity = new Vector3(
            rigidbody.linearVelocity.x,
            jumpForce,
            rigidbody.linearVelocity.z);
    }

    protected bool IsGrounded()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            0.1f,
            groundLayer);

        return isGrounded;
    }
}

public interface CharacterState
{
    void Win();
    void Lose();
}