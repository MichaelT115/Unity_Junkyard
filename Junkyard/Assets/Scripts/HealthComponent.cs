using UnityEngine;

public sealed class HealthComponent : MonoBehaviour
{
	[SerializeField]
	private float health = 10;

	public delegate void HealthUpdate(float health);
	public event HealthUpdate OnHealthUpdate = (float health) => { };

	public float Health { get => health; set => SetHealth(health); }

	public void Damage(float damage)
	{
		SetHealth(health - damage);
	}

	private void SetHealth(float value)
	{
		health = value;

		OnHealthUpdate(health);
	}

	public bool IsZeroOrBelow => Health <= 0;
}
