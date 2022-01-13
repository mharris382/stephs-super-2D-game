using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform origin;

    public float length = 0.5f;

    public LayerMask groundMask = 1;
    public bool IsGrounded { get; private set; }

    // Update is called once per frame
    void Update()
    {
        Vector2 o = origin.position;
        var dir = Vector2.down;
        var hit = Physics2D.Raycast(o, dir, length, groundMask);
        if (hit)
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
    

    private void OnDrawGizmos()
    {
        if(origin == null)
            return;
        if (Application.isPlaying)
        {
            Vector3 pos = origin.position;
            Vector3 size = new Vector3(0.2f, 0.1f, 0);
            if(IsGrounded)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.yellow;
            Gizmos.DrawCube(pos, size);
        
        }
        else
        {
            var o = origin.position;
            var dir = Vector2.down * length;
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(o, dir);
        }
    }
}
