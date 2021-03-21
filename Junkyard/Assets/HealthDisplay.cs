using TMPro;
using UnityEngine;

public sealed class HealthDisplay : MonoBehaviour
{
	[SerializeField]
	private HealthComponent healthComponent;
	[SerializeField]
	private TMP_Text text;
	private Transform cameraTransform;

	public void Start()
	{
		DisplayHealth(healthComponent.Health);
		healthComponent.OnHealthUpdate += DisplayHealth;
		cameraTransform = Camera.main.transform;
	}

	public void Update() => transform.forward = transform.position - cameraTransform.position;

	private void DisplayHealth(float health) => text.text = $"{health}";
}
