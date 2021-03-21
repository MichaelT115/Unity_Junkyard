using UnityEngine;

public class BatteryPack : MonoBehaviour
{
	[SerializeField]
	private float charge = 20;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(Tags.PLAYER))
		{
			other.GetComponent<Player>().BatteryComponent.BatteryHandler.Charge(charge);

			Destroy(gameObject);
		}
	}
}
