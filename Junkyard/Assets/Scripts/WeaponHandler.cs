using System;
using UnityEngine;
using Weapons;

[Serializable]
public sealed class WeaponHandler
{
	public IInventory Inventory { get; private set ; }
	public IBatteryHandler BatteryHandler { get; private set; }
	public Vector3 Position { get; set; }
	public Vector3 Direction { get; set; }
	public Vector3 Target { get; set; }

	[SerializeReference]
	private IWeapon weapon;

	public WeaponHandler(IBatteryHandler batteryHandler = null, IInventory inventory = null)
	{
		BatteryHandler = batteryHandler ?? new BatteryHandlerNull();
		Inventory = inventory ?? new InventoryInfinit();
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

	public void UseAmmo(int amount) => Inventory.UseAmmo(amount);

	public bool HasAmmo(int amount) => Inventory.HasAmmo(amount);
}
