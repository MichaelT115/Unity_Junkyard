using UnityEngine;
using Weapons.Data;

public sealed class WeaponHandlerComponent : MonoBehaviour
{
	[SerializeField]
	private WeaponSO weaponData;
	[SerializeField]
	private WeaponHandler weaponHandler;

	private BatteryComponent batteryComponent;
	private HealthComponent healthComponent;

	[SerializeField]
	private Transform weaponTransform;

	private void Awake()
	{
		batteryComponent = GetComponent<BatteryComponent>();
		healthComponent = GetComponent<HealthComponent>();
	}

	private void Start()
	{
		weaponHandler.BatteryHandler = batteryComponent.BatteryHandler;
		weaponHandler.Equip(weaponData.Weapon);
	}

	private void Update()
	{
		weaponHandler.Position = weaponTransform.position;
		weaponHandler.Direction = weaponTransform.forward;
		weaponHandler.Update(Time.deltaTime);
	}

	public void Activate() => weaponHandler.Activate();
	public void Deactivate() => weaponHandler.Deactivate();

	public float WeaponHeight => weaponHandler.Position.y;
}
