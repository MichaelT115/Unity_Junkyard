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
		[SerializeField]
		private Beam beam;
		[SerializeField]
		private BeamDrawer beamDrawer;

		public void Equip(WeaponHandler owner)
		{
			this.owner = owner;
			beam = new Beam() { MaxHits = 3 };
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
				beam.Fire(owner.Position, Target);

				foreach (var rigidbody in beam.HitRigidBodies)
				{
					if (rigidbody.CompareTag("Enemy"))
					{
						var health = rigidbody.GetComponent<HealthComponent>();
						health.Damage(5);
					}
				}

				beamDrawer.Draw(beam);
			}
		}

		private Vector3 Target => owner.Position + (owner.Direction * 10);

		public WeaponHandler Owner => owner;
	}
}
