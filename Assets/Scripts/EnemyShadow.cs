using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadow : MonoBehaviour
{
    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - transform.parent.position;
    }

    private void FixedUpdate()
    {
        transform.position = transform.parent.position + offset;
    }
}
