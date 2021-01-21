using System;
using UnityEngine;

[Serializable]
public sealed class BatteryHandler
{
	[SerializeField]
	private Battery battery;

	private int powerInceases = 0;
	private int powerDecreases = 0;
	private float totalIncrease = 0;
	private float totalDecrease = 0;

	public ref readonly Battery Battery => ref battery;

	public BatteryHandler(Battery battery = new Battery()) => this.battery = battery;

	public void ResetCounts()
	{
		powerInceases = 0;
		powerDecreases = 0;
	}

	public void SetBattery(in Battery battery)
	{
		this.battery = battery;
	}

	public void Drain(float power)
	{
		battery = battery.WithPower(battery.Power - power);
	}

	public void Charge(float power, Polarity polarity = Polarity.NUETRAL)
	{
		if (polarity == Polarity.NUETRAL || battery.Polarity == Polarity.NUETRAL)
		{
			battery = battery.WithPower(battery.Power + power);
		}
		else
		{
			float delta = polarity == Battery.Polarity ? power : -power;
			battery = battery.WithPower(battery.Power + delta);
		}
	}
}
