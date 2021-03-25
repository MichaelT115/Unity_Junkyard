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

	public void SetBattery(in Battery battery) => this.battery = battery;

	public void Drain(float power) => battery = battery.WithPower(battery.Power - power);

	public void Charge(float power) => battery = battery.WithPower(battery.Power + power);

	public void ClampAtMax() => battery = battery.WithPower(Math.Min(battery.Power, battery.MaxPower));
}
