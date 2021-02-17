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

	[SerializeField]
	private bool hasChargedThisFrame;
	[SerializeField]
	private bool hasDrainedThisFrame;
	[SerializeField]
	private bool isZero;
	[SerializeField]
	private bool isMax;

	private void Update()
	{
		batteryHandler.ResetCounts();
	}

	private void LateUpdate()
	{ 
		hasChargedThisFrame = Battery.Power > batteryPrevious.Power;
		hasDrainedThisFrame = Battery.Power < batteryPrevious.Power;

		if (Battery.Power > Battery.MaxPower)
		{
			batteryHandler.SetBattery(new Battery(Battery.MaxPower, Battery.MaxPower));
		}

		if (Battery.Power < 0)
		{
			batteryHandler.SetBattery(new Battery(0, Battery.MaxPower));
		}

		batteryPrevious = Battery;


		isZero = Battery.Power == 0;
		isMax = Battery.MaxPower <= Battery.Power;
	}
}
