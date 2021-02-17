using UnityEngine;

[RequireComponent(typeof(BatteryComponent))]
public sealed class BatteryDrainer : MonoBehaviour
{
	private BatteryComponent batteryComponent;
	[SerializeField]
	private float drainPerSecond = 1;

	private void Awake() => batteryComponent = GetComponent<BatteryComponent>();

	private void Update() => batteryComponent.BatteryHandler.Drain(drainPerSecond * Time.deltaTime);
}
