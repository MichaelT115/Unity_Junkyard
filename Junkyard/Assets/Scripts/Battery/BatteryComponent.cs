using UnityEngine;

public sealed class BatteryComponent : MonoBehaviour
{
	[SerializeField]
	private BatteryHandler batteryHandler = new BatteryHandler();
	public ref readonly Battery Battery => ref batteryHandler.Battery;

	private Battery batteryPrevious;

	public BatteryHandler BatteryHandler => batteryHandler;

	public bool HasChargedThisFrame => hasChargedThisFrame;
	public bool HasDrainedThisFrame => hasDrainedThisFrame;
	public bool IsZero => isZero;
	public bool IsMax => isMax;
	public bool IsOverchraged => isOvercharged;

	[SerializeField]
	private bool hasChargedThisFrame;
	[SerializeField]
	private bool hasDrainedThisFrame;
	[SerializeField]
	private bool isZero;
	[SerializeField]
	private bool isMax;
	[SerializeField]
	private bool isOvercharged;

	private void LateUpdate()
	{ 
		hasChargedThisFrame = Battery.Power > batteryPrevious.Power;
		hasDrainedThisFrame = Battery.Power < batteryPrevious.Power;

		batteryPrevious = Battery;

		isZero = Battery.Power <= 0;
		isMax = Battery.MaxPower <= Battery.Power;
		isOvercharged = Battery.MaxPower < Battery.Power;
	}

	public void Charge(float power) => batteryHandler.Charge(power);
	public void Drain(float power) => batteryHandler.Drain(power);
	public void ClampAtMax() => batteryHandler.ClampAtMax();
}
