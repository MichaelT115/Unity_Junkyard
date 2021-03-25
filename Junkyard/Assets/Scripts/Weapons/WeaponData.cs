using UnityEngine;

namespace Weapons
{
	public abstract class WeaponData : ScriptableObject
	{
		protected const string WEAPON_PREFIX = "Weapon_";
		protected const string WEAPON_MENU_FOLDER = "Weapon";

		public abstract IWeapon Weapon { get; }
	}
}