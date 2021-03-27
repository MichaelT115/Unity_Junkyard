using System;
using UnityEngine;
using Weapons;

[Serializable]
public sealed class WeaponHandler
{
	public Inventory Inventory { get; private set ; }
	public IBatteryHandler BatteryHandler { get;  set; }
	public Vector3 Position { get; set; }
	public Vector3 Direction { get; set; }

	[SerializeReference]
	private IWeapon weapon;

	public WeaponHandler(IBatteryHandler batteryHandler = null, Inventory inventory = null)
	{
		BatteryHandler = batteryHandler ?? new BatteryHandlerNull();
		Inventory = inventory;
	}

	public void Equip(IWeapon weapon)
	{
		this.weapon = weapon;
		this.weapon.Equip(this);
	}

	public void Activate() => weapon.Activate();

	public void Deactivate() => weapon.Deactivate();

	public void Update(float deltaTime) => weapon.Update(deltaTime);

	public void DrainBattery(float power) => BatteryHandler.Drain(power);
}
