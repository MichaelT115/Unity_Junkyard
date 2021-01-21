using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BatteryPoweredLight : MonoBehaviour
{
    [SerializeField]
    private new Light light;

    [SerializeField]
    private BatteryComponent batteryComponent;

    private void Update()
    {
        Battery battery = batteryComponent.Battery;

        if (batteryComponent.IsZero)
        {
            light.intensity = 0.25f;
            light.color = Color.red;
        }
        else
        {
            light.intensity = battery.Power / battery.MaxPower;
        }
    }
}
