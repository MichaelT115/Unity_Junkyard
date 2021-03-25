namespace Weapons
{
	public interface IWeapon
	{
		WeaponHandler Owner { get; }

		void Activate();
		void Deactivate();
		void Equip(WeaponHandler owner);
		void Update(float deltaTime);
	}
}