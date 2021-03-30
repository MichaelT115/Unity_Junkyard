using System;
using UnityEngine;

namespace Weapons
{

	[Serializable]
	public struct Beam
	{
		[SerializeField] private Vector3 origin;
		[SerializeField] private Vector3 target;
		[SerializeField] private Vector3 endPoint;
		[SerializeField] private Rigidbody[] hitRigidbodies;
		[SerializeField] private Vector3[] hitPoints;
		[SerializeField] private Vector3[] hitNormals;

		[SerializeField] private float distance;
		[SerializeField] private float distanceToTarget;
		[SerializeField] private Vector3 direction;

		public Beam(Vector3 origin, Vector3 target, Vector3 endPoint) 
			: this(origin, target, endPoint, new Vector3[] { }, new Vector3[] { }, new Rigidbody[] { })
		{

		}

		public Beam(Vector3 origin, Vector3 target, Vector3 endPoint, Vector3[] hitPoints, Vector3[] hitNormals, Rigidbody[] hitRigidbodies)
		{
			this.origin = origin;
			this.target = target;
			this.endPoint = endPoint;
			this.hitRigidbodies = hitRigidbodies;
			this.hitPoints = hitPoints;
			this.hitNormals = hitNormals;

			distance = Vector3.Distance(origin, endPoint);
			distanceToTarget = Vector3.Distance(origin, target);
			direction = (endPoint - origin) / distance;
		}

		public static readonly Beam EMPTY = new Beam()
		{
			hitRigidbodies = new Rigidbody[] { },
			hitPoints = new Vector3[] { },
			hitNormals = new Vector3[] { }
		};

		public Vector3 Origin => origin;
		public Vector3 Direction => direction;
		public Vector3 Target => target;
		public Vector3 EndPoint => endPoint;
		public float Distance => distance;
		public Rigidbody[] HitRigidbodies => hitRigidbodies;
		public Vector3[] HitPoints => hitPoints;
		public Vector3[] HitNormals => hitNormals;
		public float DistanceToTarget => distanceToTarget;
	}
}
