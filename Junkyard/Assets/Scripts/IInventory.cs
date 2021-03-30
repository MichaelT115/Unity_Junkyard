public interface IInventory
{
	void AddAmmo(int amount);
	bool HasAmmo(int amount);
	void UseAmmo(int amount);
}