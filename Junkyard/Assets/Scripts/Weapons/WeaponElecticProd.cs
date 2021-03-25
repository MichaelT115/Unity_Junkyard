using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Weapons
{
	[Serializable]
	public class WeaponElecticProd : IWeapon
	{
		private WeaponHandler owner;

		[SerializeField]
		private float range = 2;

		[SerializeField]
		private ParticleSystem weaponEffectPrefab;
		private ParticleSystem weaponEffect;

		public void Equip(WeaponHandler owner)
		{
			this.owner = owner;
			weaponEffect = Object.Instantiate(weaponEffectPrefab);
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
		}

		private void Fire()
		{
			PlayFireEffect(owner.Position, owner.Direction);

			owner.DrainBattery(1);

			float halfRange = range / 2;
			Quaternion orientation = Quaternion.LookRotation(owner.Direction);
			var results = Physics.OverlapBox(owner.Position + (owner.Direction * halfRange), new Vector3(halfRange, 1, halfRange), orientation);
			var map = new Dictionary<int, bool>(results.Length);


			for (int i = 0; i < results.Length; ++i)
			{
				Rigidbody attachedRigidbody = results[i].attachedRigidbody;
				if (attachedRigidbody && !map.ContainsKey(attachedRigidbody.GetInstanceID()) && attachedRigidbody.TryGetComponent(out Enemy enemy))
				{
					map[attachedRigidbody.GetInstanceID()] = true;

					enemy.BatteryComponent.Charge(10);
				}
			}
		}

		private void PlayFireEffect(Vector3 position, Vector3 direction)
		{
			weaponEffect.transform.position = position;
			weaponEffect.transform.forward = direction;
			weaponEffect.Play(true);
		}

		public WeaponHandler Owner => owner;
	}
}