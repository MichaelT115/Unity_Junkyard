public sealed class InventoryInfinit : IInventory
{
	public void AddAmmo(int amount) { }
	public bool HasAmmo(int amount) => true;
	public void UseAmmo(int amount) { }
}
