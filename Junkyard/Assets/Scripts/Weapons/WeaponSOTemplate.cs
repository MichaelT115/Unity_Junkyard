using UnityEngine;

namespace Weapons.Data
{
	public class WeaponSOTemplate<WeaponType> : WeaponSO where WeaponType : IWeapon
	{
		[SerializeField]
		private WeaponType weapon;

		public override IWeapon Weapon => Instantiate(this).weapon;
	}
}