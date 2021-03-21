using UnityEngine;

public class Enemy : MonoBehaviour
{
	private new Rigidbody rigidbody;
	[SerializeField]
	private bool isDead;

	private void Awake()
	{
		HealthComponent = GetComponent<HealthComponent>();
		rigidbody = GetComponent<Rigidbody>();
		isDead = false;
	}

	private void Update()
	{
		if (HealthComponent.IsZeroOrBelow)
		{
			Kill();
		}
	}

	private void Kill()
	{
		rigidbody.isKinematic = false;
		rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.AddForce(Random.onUnitSphere);
		enabled = false;
		isDead = true;
	}

	public HealthComponent HealthComponent { get; private set; }
	public bool IsDead => isDead;
}
