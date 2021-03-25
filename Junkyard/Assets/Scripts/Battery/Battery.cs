using System;
using UnityEngine;

public enum Polarity : sbyte
{
	NUETRAL = 0,
	POSITIVE = 1,
	NEGATIVE = -1
}

[Serializable]
public struct Battery
{
	[SerializeField]
	private float power;
	[SerializeField]
	private float maxPower;
	[SerializeField]
	private Polarity polarity;

	public Battery(float power = 100, float maxPower = 100, Polarity polarity = Polarity.NUETRAL)
	{
		this.power = power;
		this.maxPower = maxPower;
		this.polarity = polarity;
	}

	public float Power { get => power; }
	public float MaxPower { get => maxPower; }
	public Polarity Polarity { get => polarity; }

	public Battery WithPower(float power) => new Battery(power, maxPower, polarity);
}
