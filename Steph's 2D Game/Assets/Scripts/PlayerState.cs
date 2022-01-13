using System;
using DefaultNamespace;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public PlayerConfig Config;
    public GroundCheck GroundCheck;
    public bool isClimbing;
    public float XInput { get; set; }
    public float YInput { get; set; }

    public bool JumpPressed { get; set; }

}