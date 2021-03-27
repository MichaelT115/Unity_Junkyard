using System;
using UnityEngine;

[Serializable]
public sealed class BatteryHandler : IBatteryHandler
{
	[SerializeField]
	private Battery battery;
	public ref readonly Battery Battery => ref battery;

	public BatteryHandler(Battery battery = new Battery()) => this.battery = battery;

	public void SetBattery(in Battery battery) => this.battery = battery;
	public void Drain(float power) => battery = battery.WithPower(battery.Power - power);
	public void Charge(float power) => battery = battery.WithPower(battery.Power + power);
	public void ClampAtMax() => battery = battery.WithPower(Math.Min(battery.Power, battery.MaxPower));
}

[Serializable]
public sealed class BatteryHandlerNull : IBatteryHandler
{
	private static Battery battery = new Battery();
	public ref readonly Battery Battery => ref battery;

	public void SetBattery(in Battery battery) { }
	public void Drain(float power) { }
	public void Charge(float power) { }
	public void ClampAtMax() { }
}