using TMPro;
using UnityEngine;

public sealed class BatteryDisplay : MonoBehaviour
{
	[SerializeField]
	private BatteryComponent batteryComponent;
	[SerializeField]
	private TMP_Text text;
	private Transform cameraTransform;

	public void Start()
	{
		cameraTransform = Camera.main.transform;
	}

	public void Update()
	{
		DisplayBatteryPower(batteryComponent.Battery);
		transform.forward = transform.position - cameraTransform.position;
	}

	private void DisplayBatteryPower(Battery battery) => text.text = $"{battery.Power:0.00}%";
}
