using UnityEngine;

namespace Weapons
{
	public class WeaponDataTemplate<WeaponType> : WeaponData where WeaponType : IWeapon
	{
		[SerializeField]
		private WeaponType weapon;

		public override IWeapon Weapon => weapon;
	}
}