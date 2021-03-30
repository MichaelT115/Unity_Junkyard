using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Weapons
{
	[Serializable]
	public sealed class WeaponMinigun : IWeapon
	{
		private const float TIME_BETWEEN_SHOTS = 0.02f;

		[SerializeField]
		private float maxAngleOffset = 10;
		private WeaponHandler owner;

		[SerializeField]
		private LineRenderer lineRendererPrefab;
		[SerializeField]
		private ParticleSystem impactEffectPrefab;
		[SerializeField]
		private ParticleSystem fireEffectPrefab;

		private readonly List<LineRenderer> lineRenderers = new List<LineRenderer>();
		private ParticleSystem impactEffect;
		private ParticleSystem fireEffect;

		[SerializeField]
		private bool isActive = false;

		private float timeOfNextShot = 0;
		private float timeActive = 0;

		public void Equip(WeaponHandler owner)
		{
			this.owner = owner;

			impactEffect = Object.Instantiate(impactEffectPrefab);
			fireEffect = Object.Instantiate(fireEffectPrefab);

			impactEffect.Stop();
			fireEffect.Stop();
		}

		public void Activate()
		{
			isActive = true;
			timeActive = 0;
			timeOfNextShot = 0;
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

				while (timeActive > timeOfNextShot && owner.HasAmmo(1))
				{
					Fire();
					timeOfNextShot += TIME_BETWEEN_SHOTS;
				}
			}

			Vector3[] positions = new Vector3[2];
			for (int i = lineRenderers.Count - 1; 0 <= i; --i)
			{
				var startColor = lineRenderers[i].startColor;
				startColor.a -= 2f * deltaTime;
				lineRenderers[i].startColor = startColor;
				_ = lineRenderers[i].GetPositions(positions);

				for (int j = 0; j < 2; ++j)
				{
					positions[j] += Vector3.up * 0.25f * deltaTime;
				}

				lineRenderers[i].SetPositions(positions);

				if (startColor.a < 0)
				{
					Object.Destroy(lineRenderers[i]);
					lineRenderers.RemoveAt(i);
				}
			}
		}

		private void Fire()
		{
			PlayFireEffect(owner.Position, owner.Direction);

			owner.UseAmmo(1);

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
			var lineRenderer = Object.Instantiate(lineRendererPrefab);
			lineRenderer.enabled = true;
			lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });

			var startColor = lineRenderer.startColor;
			startColor.a = 1;
			lineRenderer.startColor = startColor;

			lineRenderers.Add(lineRenderer);
		}

		public WeaponHandler Owner => owner;
	}
}