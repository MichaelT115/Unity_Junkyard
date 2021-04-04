using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Weapons
{
	[Serializable]
	public sealed class WeaponShotgun : IWeapon
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
		[SerializeField]
		private bool hasFired = false;

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
			hasFired = false;
		}

		public void Update(float deltaTime)
		{
			if (CanFire)
			{
				Fire();
				timeOfNextShot = Time.time + TIME_BETWEEN_SHOTS;
			}

			UpdateGraphics(deltaTime);
		}

		private void UpdateGraphics(float deltaTime)
		{
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

		private bool CanFire => isActive && !hasFired && Time.time > timeOfNextShot && owner.HasShotgunAmmo(1);

		private void Fire()
		{

			PlayFireEffect(owner.Position, owner.Direction);
			UseAmmo();

			for (int i = 0; i < 8; ++i)
			{
				var hitRay = new Ray(owner.Position, RandomAngleOffset * owner.Direction);

				var hasHit = Physics.Raycast(hitRay, out RaycastHit hitInfo, 1000);

				if (hasHit)
				{
					HandleHit(hitRay, hitInfo);
				}

				PlayFireEffect(owner.Position, owner.Direction);
				if (hasHit)
				{
					PlayImpactEffect(hitInfo.point, hitInfo.normal);
				}

				EnableGraphic(owner.Position, hasHit ? hitInfo.point : hitRay.GetPoint(1000));
			}

			hasFired = true;
		}

		private void HandleHit(in Ray hitRay, in RaycastHit hitInfo)
		{
			if (!hitInfo.rigidbody)
			{
				return;
			}

			if (hitInfo.rigidbody.CompareTag("Enemy"))
			{
				var health = hitInfo.rigidbody.GetComponent<HealthComponent>();
				health.Damage(1);
			}
			else
			{
				hitInfo.rigidbody.AddForceAtPosition(hitRay.direction * 100, hitInfo.point);
			}
		}

		private void UseAmmo() => owner.UseShotgunAmmo(1);

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