using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Weapons
{
	[Serializable]
	public class WeaponContinuousBeam : IWeapon
	{
		private WeaponHandler owner;

		private bool isBeamActive;
		private readonly BeamBuilder beamGenerator = new BeamBuilder();
		[SerializeField]
		private Beam beam;
		[SerializeField]
		private BeamDrawer beamDrawer;

		public void Equip(WeaponHandler owner)
		{
			this.owner = owner;
			beamDrawer.Init();
		}

		public void Activate()
		{
			isBeamActive = true;
		}

		public void Deactivate()
		{
			isBeamActive = false;

			beamDrawer.Stop();
		}

		public void Update(float deltaTime)
		{
			if (isBeamActive)
			{
				beam =beamGenerator.BuildBeam(owner.Position, Target, 3);

				for (int i = 0; i < beam.HitRigidbodies.Length; ++i)
				{
					if (beam.HitRigidbodies[i].CompareTag("Enemy"))
					{
						var health = beam.HitRigidbodies[i].GetComponent<HealthComponent>();
						health.Damage(5);
					}
				}

				beamDrawer.Draw(beam);
			}
		}

		private Vector3 Target
		{
			get
			{
				var distance = Math.Min(Vector3.Distance(owner.Position, owner.Target), 8);
				var direction = (owner.Target - owner.Position).normalized;
				return owner.Position + direction * distance;
			}
		}

		public WeaponHandler Owner => owner;
	}
}
