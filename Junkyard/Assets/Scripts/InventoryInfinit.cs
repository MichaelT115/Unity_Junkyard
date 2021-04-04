public sealed class InventoryInfinit : IInventory
{
	void IInventory.AddMinigunAmmo(int amount) { }

	void IInventory.AddShotgunAmmo(int amount) { }

	bool IInventory.HasMinigunAmmo(int amount) => true;

	bool IInventory.HasShotgunAmmo(int amount) => true;

	void IInventory.UseMinigunAmmo(int amount) { }

	void IInventory.UseShotgunAmmo(int amount) { }
}
