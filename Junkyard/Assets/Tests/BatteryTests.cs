using NUnit.Framework;

namespace Tests
{
	public class BatteryTests
    {
        [Test]
        public void BatterySimplePasses()
        {
            var battery = new Battery(100, 100);

            Assert.AreEqual(100, battery.Power);
        }

        [Test]
        public void BatteryDecreasePower()
		{
            var batteryHandler = new BatteryHandler();
            var intialPower = batteryHandler.Battery.Power;
            //batteryHandler.ChangePower(-10, out Battery battery);

			//Assert.AreEqual(intialPower - 10, battery.Power);
		}

        [Test]
        public void BatteryIncreasePower()
        {
            var batteryHandler = new BatteryHandler();
            var intialPower = batteryHandler.Battery.Power;
            //batteryHandler.ChangePower(10, out Battery battery);

           // Assert.AreEqual(intialPower + 10, battery.Power);
        }
    }
}
