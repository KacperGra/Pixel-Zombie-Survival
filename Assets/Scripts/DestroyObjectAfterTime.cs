using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfterTime : MonoBehaviour
{
    public float destroyTime = 0.5f;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, destroyTime);
    }
}
