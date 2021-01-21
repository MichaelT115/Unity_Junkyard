using UnityEngine;

public class PickUpPowerUp : MonoBehaviour
{
	[SerializeField]
	private float powerToAdd = 25;
	public float PowerToAdd => powerToAdd;

	private void OnTriggerEnter(Collider other)
	{
		BatteryComponent batteryComponent = other.GetComponent<BatteryComponent>();

		if (batteryComponent)
		{
			batteryComponent.BatteryHandler.Charge(powerToAdd);
			Destroy(gameObject);
		}
	}
}
