using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
	[Serializable]
	public sealed class Beam
	{
		private Vector3 origin;
		private Vector3 target;
		private Vector3 direction;
		private float distance;
		private int maxHits = 8;

		private Vector3 endPoint;

		[SerializeField]
		private List<Rigidbody> hitRigidBodies = new List<Rigidbody>();
		[SerializeField]
		private List<Vector3> hitPoints = new List<Vector3>();
		[SerializeField]
		private List<Vector3> hitNormals = new List<Vector3>();

		public void Fire(Vector3 origin, Vector3 target)
		{
			Clear();

			this.origin = origin;
			this.target = target;
			endPoint = target;
			this.distance = Vector3.Distance(origin, target);
			this.direction = (target - origin) / distance;

			var entryHits = new RaycastHit[maxHits];
			var hitCount = Physics.RaycastNonAlloc(origin, direction, entryHits, distance);

			if (hitCount > 0)
			{
				for (int i = 0; i < hitCount; ++i)
				{
					var hit = entryHits[i];

					if (hit.rigidbody)
					{
						hitRigidBodies.Add(hit.rigidbody);
					}

					hitPoints.Add(hit.point);
					hitNormals.Add(hit.normal);
				}

				var finalDistance = hitCount == maxHits ? entryHits[hitCount - 1].distance : distance;
				endPoint = hitCount == maxHits ? entryHits[hitCount - 1].point : target;

				var exitHits = Physics.RaycastAll(endPoint, -direction, finalDistance);

				foreach (var hit in exitHits)
				{
					hitPoints.Add(hit.point);
					hitNormals.Add(hit.normal);
				}
			}
		}

		private void Clear()
		{
			origin = Vector3.zero;
			direction = Vector3.zero;
			target = Vector3.zero;

			hitRigidBodies.Clear();
			hitPoints.Clear();
			hitNormals.Clear();
		}

		public Vector3 Origin => origin;
		public Vector3 Direction => direction;
		public Vector3 Target => target;
		public Vector3 EndPoint => endPoint;

		public List<Rigidbody> HitRigidBodies => hitRigidBodies;
		public List<Vector3> HitPoints => hitPoints;
		public List<Vector3> HitNormals => hitNormals;

		public int MaxHits { get => maxHits; set => maxHits = value; }
		public float Distance => distance;
	}

	[Serializable]
	public sealed class BeamDrawer
	{
		[SerializeField]
		private LineRenderer lineRendererPrefab;
		[SerializeField]
		private LineRenderer lineRenderer;
		[SerializeField]
		private ParticleSystem impactEffectPrefab;
		[SerializeField]
		private ParticleSystem[] impactEffects;

		public void Init()
		{
			lineRenderer = UnityEngine.Object.Instantiate(lineRendererPrefab);

			impactEffects = new ParticleSystem[20];
			for (var i = 0; i < impactEffects.Length; ++i)
			{
				impactEffects[i] = UnityEngine.Object.Instantiate(impactEffectPrefab);
			}

			lineRenderer.positionCount = 20;
		}

		public void Draw(Beam beam)
		{
			lineRenderer.enabled = true;

			lineRenderer.SetPosition(0, beam.Origin);
			var segmentSize = Vector3.Distance(beam.Origin, beam.EndPoint) / 20;
			for (var i = 1; i < 19; ++i)
			{
				Vector3 basePosition = beam.Origin + beam.Direction * segmentSize * i;
				Vector3 offset = UnityEngine.Random.insideUnitSphere * segmentSize * 0.2f;
				Vector3 position =  basePosition + offset;

				lineRenderer.SetPosition(i, position);
			}
			lineRenderer.SetPosition(19, beam.EndPoint);

			int index = 0;
			while (index < beam.HitPoints.Count)
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
