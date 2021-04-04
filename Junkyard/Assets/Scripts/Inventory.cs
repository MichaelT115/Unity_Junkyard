using System;
using UnityEngine;

[Serializable]
public sealed class Inventory : IInventory
{
	[SerializeField]
	private int minigunAmmo = 5000;

	[SerializeField]
	private int shotgunAmmo = 20;

	public void AddMinigunAmmo(int amount) => minigunAmmo += amount;
	public void UseMinigunAmmo(int amount) => minigunAmmo -= amount;
	public bool HasMinigunAmmo(int amount) => amount <= minigunAmmo;

	public void AddShotgunAmmo(int amount) => shotgunAmmo += amount;
	public bool HasShotgunAmmo(int amount) => amount <= shotgunAmmo;
	public void UseShotgunAmmo(int amount) => shotgunAmmo -= amount;
	public int ShotgunAmmoCount => shotgunAmmo;
}
