using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone 
{
    public readonly Joint joint1; 
    public readonly Joint joint2;
    public readonly float distance;
    public Vector3 directionForAngle;

    public Bone(Joint joint1, Joint joint2)
    {
        this.joint1 = joint1;
        this.joint2 = joint2;
        distance = CalculateDistance();
    }

    public float CalculateDistance()
    {
        return (joint2.position - joint1.position).magnitude;
    }


}
