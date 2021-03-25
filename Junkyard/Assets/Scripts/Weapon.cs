using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Weapon
{
	private WeaponHandler owner;

	[SerializeField]
	private LineRenderer lineRendererPrefab;
	[SerializeField]
	private ParticleSystem impactEffectPrefab;
	[SerializeField]
	private ParticleSystem fireEffectPrefab;

	private LineRenderer lineRenderer;
	private ParticleSystem impactEffect;
	private ParticleSystem fireEffect;

	public void Equip(WeaponHandler owner)
	{
		this.owner = owner;

		lineRenderer = Object.Instantiate(lineRendererPrefab);
		impactEffect = Object.Instantiate(impactEffectPrefab);
		fireEffect = Object.Instantiate(fireEffectPrefab);
	}

	public void Activate()
	{
		Fire();
	}

	public void Deactivate()
	{

	}

	public void Update(float deltaTime)
	{
		var startColor = lineRenderer.startColor;
		startColor.a -= 2f * deltaTime;
		lineRenderer.startColor = startColor;

		if (startColor.a < 0)
		{
			lineRenderer.enabled = false;
		}
	}

	private void Fire()
	{
		PlayFireEffect(owner.Position, owner.Direction);

		owner.DrainBattery(1);

		Ray hitRay = new Ray(owner.Position, owner.Direction);
		if (Physics.Raycast(hitRay, out RaycastHit hitInfo, 1000))
		{
			if (hitInfo.rigidbody)
			{
				if (hitInfo.rigidbody.CompareTag("Enemy"))
				{
					var health = hitInfo.rigidbody.GetComponent<HealthComponent>();
					health.Damage(5);
				}
				else
				{
					hitInfo.rigidbody.AddForceAtPosition(owner.Direction * 100, hitInfo.point);
				}
			}

			EnableGraphic(owner.Position, hitInfo.point);
			PlayImpactEffect(hitInfo.point, hitInfo.normal);
		}
		else
		{
			EnableGraphic(owner.Position, hitRay.GetPoint(1000));
		}
	}

	private void PlayImpactEffect(Vector3 position, Vector3 normal)
	{
		impactEffect.transform.position = position;
		impactEffect.transform.forward = normal;
		impactEffect.Play(true);
	}

	private void PlayFireEffect(Vector3 position, Vector3 direction)
	{
		fireEffect.transform.position = position;
		fireEffect.transform.forward = direction;
		fireEffect.Play(true);
	}

	private void EnableGraphic(Vector3 startPoint, Vector3 endPoint)
	{
		lineRenderer.enabled = true;
		lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });

		var startColor = lineRenderer.startColor;
		startColor.a = 1;
		lineRenderer.startColor = startColor;
	}

	public WeaponHandler Owner => owner;
}
