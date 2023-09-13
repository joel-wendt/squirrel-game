using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    [SerializeField] private float smoothing = 1.0f;

    // Note to self: Läs mer om Lerp (Linjär interpolering. Flytta från en punkt till en annan över en viss tid)
    void LateUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
        transform.position = newPosition;
    }
}
