
public interface IInventory
{
	void AddMinigunAmmo(int amount);
	bool HasMinigunAmmo(int amount);
	void UseMinigunAmmo(int amount);

	void AddShotgunAmmo(int amount);
	bool HasShotgunAmmo(int amount);
	void UseShotgunAmmo(int amount);
}