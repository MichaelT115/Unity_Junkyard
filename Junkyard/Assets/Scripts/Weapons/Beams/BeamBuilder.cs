using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
	public sealed class BeamBuilder
	{
		private static readonly List<Rigidbody> hitRigidbodies = new List<Rigidbody>();

		public Beam BuildBeam(in Vector3 origin, in Vector3 target, in int maxHits)
		{
			hitRigidbodies.Clear();

			float distance = Vector3.Distance(origin, target);
			Vector3 direction = (target - origin) / distance;

			var entryHits = new RaycastHit[maxHits];
			var entryHitsCount = Physics.RaycastNonAlloc(origin, direction, entryHits, distance);

			if (entryHitsCount == 0)
			{
				return new Beam(origin, target, target);
			}

			var finalDistance = entryHitsCount == maxHits ? entryHits[entryHitsCount - 1].distance : distance;
			var endPoint = entryHitsCount == maxHits ? entryHits[entryHitsCount - 1].point : target;
			var exitHits = new RaycastHit[(entryHitsCount == maxHits ? entryHitsCount - 1 : entryHitsCount)];

			var exitHitsCount = Physics.RaycastNonAlloc(endPoint, -direction, exitHits, finalDistance);

			var hitsTotal = entryHitsCount + exitHitsCount;
			var hitPoints = new Vector3[hitsTotal];
			var hitNormals = new Vector3[hitsTotal];

			for (int i = 0; i < entryHitsCount; ++i)
			{
				if (entryHits[i].rigidbody)
				{
					hitRigidbodies.Add(entryHits[i].rigidbody);
				}

				hitPoints[i] = entryHits[i].point;
				hitNormals[i] = entryHits[i].normal;
			}

			for (int i = 0; i < exitHitsCount; ++i)
			{
				hitPoints[entryHitsCount + i] = exitHits[i].point;
				hitNormals[entryHitsCount + i] = exitHits[i].normal;
			}

			return new Beam(origin, target, endPoint, hitPoints, hitNormals, hitRigidbodies.ToArray());
		}
	}
}
