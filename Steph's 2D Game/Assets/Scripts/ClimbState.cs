using System;
using DefaultNamespace;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class ClimbState : MonoBehaviour, IState
{
    private PlayerState _state;
    private Rigidbody2D _rb;
    private int _numClimbables;
    public bool IsClimbing
    {
        get { return _state.isClimbing;}
        set { _state.isClimbing = value; }
    }

    public float climbSpeed => _state.Config.climbSpeed;
    private float _defaultGravityScale;

    private bool HasClimbable() => _numClimbables > 0;
    private void Awake()
    {
        _state = GetComponent<PlayerState>();
        _rb = GetComponent<Rigidbody2D>();
        _defaultGravityScale = _rb.gravityScale;
    }

    public void Update()
    {
        if (HasClimbable() )
        {
            if(_state.YInput>0.5f)
                IsClimbing = true;
        }
        else
        {
            IsClimbing = false;
        }
    }

    public void Tick()
    {
        if (_state.JumpPressed)
        {
            
        }
    }

    public void FixedTick()
    {
        float y = _state.YInput * climbSpeed * Time.fixedDeltaTime;
        var vel = _rb.velocity;
        vel.y = y;
        _rb.velocity = vel;
    }

    public void OnEnter()
    {
        _rb.gravityScale = 0;
    }

    public void OnExit()
    {
        _rb.gravityScale = _defaultGravityScale;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TriggerIsLadder(other))
        {
            _numClimbables++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (TriggerIsLadder(other))
        {
            _numClimbables--;
            _numClimbables = Mathf.Max(0, _numClimbables);
        }
    }

    private static bool TriggerIsLadder(Collider2D other)
    {
        return other.CompareTag("Ladder");
    }
}