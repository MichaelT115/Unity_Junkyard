using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Weapons
{
	[Serializable]
	public class WeaponRepeaterBeam : IWeapon
	{
		[SerializeField]
		private float maxAngleOffset = 5;
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

		[SerializeField]
		private bool isActive = false;

		private float timeSinceLastShot = 0;
		private float timeActive = 0;

		public void Equip(WeaponHandler owner)
		{
			this.owner = owner;

			lineRenderer = Object.Instantiate(lineRendererPrefab);
			impactEffect = Object.Instantiate(impactEffectPrefab);
			fireEffect = Object.Instantiate(fireEffectPrefab);

			lineRenderer.enabled = false;
			impactEffect.Stop();
			fireEffect.Stop();
		}

		public void Activate()
		{
			isActive = true;
			timeActive = 0;
			Fire();
		}

		public void Deactivate()
		{
			isActive = false;
		}

		public void Update(float deltaTime)
		{
			if (isActive)
			{
				timeActive += deltaTime;
				timeSinceLastShot += deltaTime;

				if (timeSinceLastShot > 0.1)
				{
					Fire();
				}
			}

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
			timeSinceLastShot = 0;

			PlayFireEffect(owner.Position, owner.Direction);

			owner.DrainBattery(1);

			Ray hitRay = new Ray(owner.Position, RandomAngleOffset * owner.Direction);
			if (Physics.Raycast(hitRay, out RaycastHit hitInfo, 1000))
			{
				if (hitInfo.rigidbody)
				{
					if (hitInfo.rigidbody.CompareTag("Enemy"))
					{
						var health = hitInfo.rigidbody.GetComponent<HealthComponent>();
						health.Damage(maxAngleOffset);
					}
					else
					{
						hitInfo.rigidbody.AddForceAtPosition(hitRay.direction * 100, hitInfo.point);
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

		private Quaternion RandomAngleOffset => Quaternion
			.Euler(
			Random.Range(-maxAngleOffset, maxAngleOffset),
			Random.Range(-maxAngleOffset, maxAngleOffset),
			Random.Range(-maxAngleOffset, maxAngleOffset)
			);

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
}