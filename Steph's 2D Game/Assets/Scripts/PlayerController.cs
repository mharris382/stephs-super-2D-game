using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GroundCheck), typeof(PlayerState))]
public class PlayerController : MonoBehaviour
{
     public float jumpForce => _state.Config.jumpForce;

     public float maxMoveSpeed => _state.Config.maxMoveSpeed;

    private Rigidbody2D rb;
    private GroundCheck groundCheck;
  

    private PlayerState _state;

    public StateMachine PlayerStateMachine = new StateMachine();
    
    public float XInput
    {
        get => _state.XInput;
        set => _state.XInput = value;
    }

    public float YInput
    {
        get => _state.YInput;
        set => _state.YInput = value;
    }

    public UnityEvent JumpFeedback;
    public UnityEvent LandFeedback;



    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        this.groundCheck = this.GetComponent<GroundCheck>();
        _state = this.GetComponent<PlayerState>();
        
        
        var airState = new AerialState(this);
        var gndState = new GroundedState(this);
        var cliState = GetComponent<ClimbState>();
        PlayerStateMachine = new StateMachine();
        PlayerStateMachine.AddState(airState);
        PlayerStateMachine.AddState(gndState);
        PlayerStateMachine.AddState(cliState);
        PlayerStateMachine.AddTransition(airState, gndState, () => groundCheck.IsGrounded==true  && cliState.IsClimbing==false);
        PlayerStateMachine.AddTransition(gndState, airState, () => groundCheck.IsGrounded==false && cliState.IsClimbing==false);
       
        PlayerStateMachine.AddTransition(airState, cliState, () => cliState.IsClimbing==true);
        PlayerStateMachine.AddTransition(gndState, cliState, () => cliState.IsClimbing==true);
        PlayerStateMachine.AddTransition(cliState, airState, () => cliState.IsClimbing==false);
        
        PlayerStateMachine.ChangeState(gndState);
    }

    private void Update()
    {
        this.YInput = Input.GetAxis("Vertical");
        this.XInput = Input.GetAxis("Horizontal");
        this._state.JumpPressed = Input.GetButtonDown("Jump");
        PlayerStateMachine.Update();
    }

    void FixedUpdate()
    {
        MoveHorizontal(XInput);
        PlayerStateMachine.FixedUpdate();
    }

    private void MoveHorizontal(float inputX)
    {
        var vMove = GetMoveVelocity(inputX);
        rb.velocity = vMove;
    }

    private Vector2 GetMoveVelocity(float inputX)
    {
        var x = inputX * maxMoveSpeed * Time.fixedDeltaTime;
        Vector2 vMove = new Vector2(x, rb.velocity.y);
        return vMove;
    }
    

    private void JumpUp()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;
        Gizmos.DrawRay(transform.position, GetMoveVelocity(XInput));
    }
    
    
    
    
        
    private class AerialState : StateBase
    {
        private PlayerController _Controller;

        public AerialState(PlayerController controller)
        {
            _Controller = controller;
        }

        public override void OnExit()
        {
            if (_Controller._state.isClimbing) return;
            _Controller.LandFeedback.Invoke();
        }
    }
    
    
    private class GroundedState : StateBase
    {
        private PlayerController _Controller;

        public GroundedState(PlayerController controller)
        {
            _Controller = controller;
        }

        public override void Tick()
        {
            if (_Controller._state.JumpPressed)
            {
                _Controller.JumpFeedback.Invoke();
                _Controller.JumpUp();
            }
        }
    }

}