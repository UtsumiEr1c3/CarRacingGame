using System;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float moveSpeed = 30f;

    private void Update()
    {
        transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
    }
}
