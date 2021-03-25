﻿using System;
using UnityEngine;

[Serializable]
public class WeaponHandler
{
	private Weapon weapon;
	[SerializeField]
	private Inventory inventory;
	private BatteryHandler batteryHandler;
	[SerializeField]
	private Transform weaponTransform;

	public void Equip(Weapon weapon)
	{
		this.weapon = weapon;
		this.weapon.Equip(this);
	}

	public void Activate() => weapon.Activate();

	public void Deactivate() => weapon.Deactivate();

	public void Update(float deltaTime) => weapon.Update(deltaTime);

	public void DrainBattery(float power) => BatteryHandler.Drain(power);

	public Vector3 Position => weaponTransform.position;
	public Vector3 Direction => weaponTransform.forward;

	public BatteryHandler BatteryHandler { get => batteryHandler; set => batteryHandler = value; }
}
