using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChargedFloor : MonoBehaviour
{
	[SerializeField]
	private Polarity polarity;
	[SerializeField]
	private float charge;

	[SerializeField]
	private List<BatteryComponent> batteryComponents = new List<BatteryComponent>();
	public List<BatteryComponent> BatteryComponents => batteryComponents;

	private void FixedUpdate()
	{
		for (int i = 0; i < batteryComponents.Count; ++i)
		{
			batteryComponents[i].BatteryHandler.Charge(charge * Time.fixedDeltaTime, polarity);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var batteryComponent = other.GetComponent<BatteryComponent>();

		if (batteryComponent && !batteryComponents.Contains(batteryComponent)) { 
			batteryComponents.Add(batteryComponent); 
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var batteryComponent = other.GetComponent<BatteryComponent>();

		if (batteryComponent && batteryComponents.Contains(batteryComponent))
		{
			batteryComponents.Remove(batteryComponent);
		}
	}
}
