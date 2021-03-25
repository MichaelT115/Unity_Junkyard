using UnityEngine;

namespace Weapons
{
	[CreateAssetMenu(fileName = WEAPON_PREFIX + "SingleShotBeam", menuName = WEAPON_MENU_FOLDER + "/Single Shot Beam")]
	public sealed class WeaponDataSingleShotBeam : WeaponDataTemplate<WeaponSingleShotBeam> { }
}