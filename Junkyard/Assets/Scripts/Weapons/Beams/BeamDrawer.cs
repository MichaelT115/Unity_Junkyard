using System;
using UnityEngine;

namespace Weapons
{
	[Serializable]
	public sealed class BeamDrawer
	{
		private const int SEGMENT_COUNT = 20;
		[SerializeField]
		private LineRenderer lineRendererPrefab;
		[SerializeField]
		private LineRenderer lineRenderer;
		[SerializeField]
		private ParticleSystem impactEffectPrefab;
		[SerializeField]
		private ParticleSystem[] impactEffects;

		private Vector3[] offsets = new Vector3[SEGMENT_COUNT];

		public void Init()
		{
			lineRenderer = UnityEngine.Object.Instantiate(lineRendererPrefab);

			impactEffects = new ParticleSystem[20];
			for (var i = 0; i < impactEffects.Length; ++i)
			{
				impactEffects[i] = UnityEngine.Object.Instantiate(impactEffectPrefab);
			}

			lineRenderer.positionCount = SEGMENT_COUNT;
			lineRenderer.enabled = false;

			for (var i = 0; i < offsets.Length; ++i)
			{
				offsets[i] = Vector3.zero;
			}
		}

		public void Draw(in Beam beam)
		{
			UpdateLine(beam);
			UpdateHitEffects(beam);
		}

		private void UpdateLine(in Beam beam)
		{
			lineRenderer.enabled = true;

			lineRenderer.SetPosition(0, beam.Origin);
			var segmentSize = beam.Distance / SEGMENT_COUNT;
			for (var i = 1; i < SEGMENT_COUNT - 1; ++i)
			{
				Vector3 basePosition = beam.Origin + beam.Direction * segmentSize * i;
				offsets[i] = Vector3.Lerp(offsets[i], UnityEngine.Random.insideUnitSphere, 20 * Time.deltaTime);
				Vector3 offset = offsets[i] * segmentSize * 1f;
				Vector3 position = basePosition + offset;

				lineRenderer.SetPosition(i, position);
			}
			lineRenderer.SetPosition(SEGMENT_COUNT - 1, beam.EndPoint);
		}

		private void UpdateHitEffects(Beam beam)
		{
			int index = 0;
			while (index < beam.HitPoints.Length)
			{
				impactEffects[index].transform.position = beam.HitPoints[index];

				impactEffects[index].Play();
				++index;
			}

			while (index < impactEffects.Length)
			{
				impactEffects[index].Stop(false, ParticleSystemStopBehavior.StopEmitting);
				++index;
			}
		}

		public void Stop()
		{
			lineRenderer.enabled = false;

			var index = 0;
			while (index < impactEffects.Length)
			{
				impactEffects[index].Stop(false, ParticleSystemStopBehavior.StopEmitting);
				++index;
			}
		}
	}
}
