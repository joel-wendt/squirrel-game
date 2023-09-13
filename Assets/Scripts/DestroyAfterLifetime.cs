using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
    private float lifeTime = 1.0f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}
