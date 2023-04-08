using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceScaling : MonoBehaviour
{

    public Transform targetTransform;

    public float scaleAmount;
    public float scaleOffset;

    public bool targetMainCamera;

    private Vector3 originalScale;

    void Start()
    {
        if (targetMainCamera)
            targetTransform = GameObject.Find("Main Camera").transform;

        originalScale = transform.localScale;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        float scale = Mathf.Clamp(scaleAmount * distance + 1, 1, Mathf.Infinity);
        transform.localScale = originalScale * scale * scaleOffset;
    }
}
