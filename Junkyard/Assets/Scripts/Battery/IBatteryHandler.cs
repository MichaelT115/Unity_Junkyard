public interface IBatteryHandler
{
	ref readonly Battery Battery { get; }

	void Charge(float power);
	void ClampAtMax();
	void Drain(float power);
	void SetBattery(in Battery battery);
}