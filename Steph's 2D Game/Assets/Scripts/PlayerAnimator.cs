using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator anim;
    
    private PlayerState _state;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    
    [SerializeField] private float climbSpeed = 1f;
    public PlayerState state
    {
        get
        {
            if (_state == null)
            {
                _state = GetComponentInParent<PlayerState>();
            }
            return _state;
        }
    }

    #region Animator Hashes
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");
    private static readonly int XSpeed = Animator.StringToHash("xSpeed");
    private static readonly int Grounded = Animator.StringToHash("IsGrounded");
    private static readonly int ClimbSpeed = Animator.StringToHash("ClimbSpeed");
    #endregion
    

    private bool IsClimbing
    {
        get { return state.isClimbing; }
    }
    private bool IsGrounded
    {
        get { return state.GroundCheck.IsGrounded; }
    }


    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponentInParent<Rigidbody2D>();
    }

    
    void Update()
    {
        UpdateFacingDirection();
        anim.SetBool(Grounded, IsGrounded);
        anim.SetLayerWeight(1, IsClimbing ? 1 : 0);
        anim.SetFloat(XSpeed, Mathf.Abs(state.XInput));
        anim.SetFloat(YVelocity, _rb.velocity.y);
        anim.SetFloat(ClimbSpeed, state.YInput * climbSpeed);
    }

    private void UpdateFacingDirection()
    {
        _sr.flipX = state.XInput < 0;
    }
}
