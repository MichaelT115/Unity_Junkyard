using TMPro;
using UnityEngine;

public sealed class ShotgunAmmoDisplay : MonoBehaviour
{
	[SerializeField]
	private WeaponHandlerComponent weaponHandlerComponent;
	[SerializeField]
	private TMP_Text text;

	private void Update() => DisplayShotgunAmmo(weaponHandlerComponent.Inventory.ShotgunAmmoCount);

	private void DisplayShotgunAmmo(int amount) => text.text = amount.ToString();
}