using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Rotator : MonoBehaviour
{
    [SerializeField]
    private float speed = 25;
	private Transform cachedTransform;

	private void Awake()
	{
		cachedTransform = transform;
	}

	private void Update()
    {
		cachedTransform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
