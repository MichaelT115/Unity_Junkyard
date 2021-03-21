using UnityEngine;

public sealed class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private Transform player;
	private new Transform transform;

	private Vector3 offset;

	private void Awake()
	{
		transform = gameObject.transform;
		offset = transform.position - player.position;
	}

	private void Update() => transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime);
}
