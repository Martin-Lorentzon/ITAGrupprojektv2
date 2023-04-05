using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceScaling : MonoBehaviour
{

    public Transform cameraTransform;

    public float scaleAmount;
    public float scaleOffset;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, cameraTransform.position);
        float scale = Mathf.Clamp(scaleAmount * distance + 1, 1, Mathf.Infinity);
        transform.localScale = originalScale * scale * scaleOffset;
    }
}
