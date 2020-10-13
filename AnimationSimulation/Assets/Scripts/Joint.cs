using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint
{
    public int angleMax = 45;
    public int angleMin = -45;
    public Vector3 position;

    public Joint(Vector3 position)
    {
        this.position = position;
    }
}
