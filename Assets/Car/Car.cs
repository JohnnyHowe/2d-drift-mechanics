using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour, CarInterface
{
    public abstract float GetDriftAngle();
    public abstract float GetDriftSpeed();
}
