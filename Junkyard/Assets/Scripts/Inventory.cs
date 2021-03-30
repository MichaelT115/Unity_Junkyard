using System;
using UnityEngine;

[Serializable]
public sealed class Inventory : IInventory
{
	[SerializeField]
	private int ammo = 5000;

	public void AddAmmo(int amount) => ammo += amount;
	public void UseAmmo(int amount) => ammo -= amount;
	public bool HasAmmo(int amount) => amount <= ammo;
}
