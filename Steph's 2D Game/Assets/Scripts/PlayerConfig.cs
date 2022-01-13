using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Config", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public float jumpForce = 2000;
    public float maxMoveSpeed = 300;
    public float climbSpeed = 200;
}
