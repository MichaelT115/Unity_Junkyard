using UnityEngine;

namespace Weapons.Data
{
	public abstract class WeaponSO : ScriptableObject, IWeaponData
	{
		protected const string WEAPON_PREFIX = "Weapon_";
		protected const string WEAPON_MENU_FOLDER = "Weapon";

		public abstract IWeapon Weapon { get; }
	}
}