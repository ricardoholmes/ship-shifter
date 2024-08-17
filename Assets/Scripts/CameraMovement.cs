using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    private Vector2 velocity = Vector2.zero;

    private void FixedUpdate()
    {
        Vector3 newPos = Vector2.SmoothDamp(transform.position, target.position, ref velocity, smoothSpeed);
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
