using UnityEngine;

namespace Weapons
{
	public sealed class BeamGenerator : MonoBehaviour
	{
		[SerializeField]
		private Beam beam;
		[SerializeField]
		private Transform start;
		[SerializeField]
		private Transform end;

		[SerializeField]
		private LineRenderer lineRenderer;
		[SerializeField]
		private ParticleSystem impactEffectPrefab;
		[SerializeField]
		private ParticleSystem[] impactEffects;

		private void Awake()
		{
			impactEffects = new ParticleSystem[20];

			for (var i = 0; i < impactEffects.Length; ++i)
			{
				impactEffects[i] = Instantiate(impactEffectPrefab);
			}
		}

		private void Update()
		{
			beam.Fire(start.position, end.position);

			lineRenderer.SetPositions(new Vector3[] { beam.Origin, beam.EndPoint });

			int index = 0;
			while (index < beam.HitPoints.Count)
			{
				impactEffects[index].gameObject.SetActive(true);
				impactEffects[index].transform.position = beam.HitPoints[index];
				++index;
			} 

			while (index < impactEffects.Length)
			{
				impactEffects[index].gameObject.SetActive(false);
				++index;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(beam.Origin, beam.Target);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(beam.Origin, 0.05f);
			Gizmos.DrawSphere(beam.Target, 0.05f);

			Gizmos.color = Color.blue;
			foreach (var hitPoint in beam.HitPoints)
			{
				Gizmos.DrawSphere(hitPoint, 0.05f);
			}
		}
	}
}
