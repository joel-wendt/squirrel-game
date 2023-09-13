using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    [SerializeField] private float smoothing = 1.0f;

    // Note to self: L�s mer om Lerp (Linj�r interpolering. Flytta fr�n en punkt till en annan �ver en viss tid)
    void LateUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
        transform.position = newPosition;
    }
}
