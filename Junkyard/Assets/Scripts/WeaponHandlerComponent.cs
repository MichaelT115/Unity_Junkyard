﻿using UnityEngine;
using Weapons.Data;

public sealed class WeaponHandlerComponent : MonoBehaviour
{
	[SerializeField]
	private WeaponSO weaponData;
	[SerializeField]
	private WeaponHandler weaponHandler;
	[SerializeField]
	private Inventory inventory;

	private BatteryComponent batteryComponent;

	[SerializeField]
	private Transform weaponTransform;

	private void Awake()
	{
		batteryComponent = GetComponent<BatteryComponent>();
		weaponHandler = new WeaponHandler(batteryComponent.BatteryHandler, inventory);
	}

	private void Start()
	{
		weaponHandler.Equip(weaponData.Weapon);
	}

	private void Update()
	{
		weaponHandler.Position = weaponTransform.position;
		weaponHandler.Direction = weaponTransform.forward;
		weaponHandler.Target = AimPosition;
		weaponHandler.Update(Time.deltaTime);
	}

	private Ray PlayerMouseRay => Camera.main.ScreenPointToRay(Input.mousePosition);
	private Plane WeaponPlane => new Plane(Vector3.up, -WeaponHeight);
	public Vector3 AimPosition =>
		WeaponPlane.Raycast(PlayerMouseRay, out float distance)
		? PlayerMouseRay.GetPoint(distance)
		: transform.position;

	public void Activate() => weaponHandler.Activate();
	public void Deactivate() => weaponHandler.Deactivate();

	public float WeaponHeight => weaponHandler.Position.y;
}
