using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	public class EnemyTests
	{
		[Test]
		public void EnemyTakesDamage()
		{
			var enemy = new GameObject().AddComponent<Enemy>();
			enemy.HealthComponent.Health = 10;

			Assert.AreEqual(5, enemy.HealthComponent);
		}
	}
}

