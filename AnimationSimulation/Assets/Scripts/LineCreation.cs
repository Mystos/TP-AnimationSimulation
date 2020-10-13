using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineCreation : MonoBehaviour
{
    public LineRenderer line;
    public Transform targetPoints;
    private List<Joint> joints;
    private List<Bone> bones;
    private Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        joints = new List<Joint>() {
        new Joint(Vector3.zero),
        new Joint(new Vector3(1,0,0)),
        new Joint(new Vector3(2,0,0)),
        new Joint(new Vector3(3,0,0))
        };
        startPoint = joints[0].position;

        bones = new List<Bone>();
        for (int i = 0; i < joints.Count -1; i++)
        {
            bones.Add(new Bone(joints[i], joints[i + 1]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //FABRIK(line,distBones,startPoint, targetPoints);
        joints = FABRIK(joints, bones, startPoint, targetPoints);
        line.positionCount = joints.Count;

        for (int i = 0; i < joints.Count; i++)
        {
            line.SetPosition(i, joints[i].position);
        }
    }

    public static void MovePoint(LineRenderer line, Transform target) 
    {
        // Move Last Index
        line.SetPosition(line.positionCount -1, target.position);
    }

    public static void FABRIK(LineRenderer line, float[] distBones,Vector3 startPoint, Transform target)
    {
        line.SetPosition(line.positionCount - 1, target.position);
        
        for (int i = line.positionCount - 1; i > 0 ; i--)
        {
            Vector3 positionCalculated = line.GetPosition(i) + distBones[i-1] * Vector3.Normalize(line.GetPosition(i - 1) - line.GetPosition(i));
            line.SetPosition(i - 1 , positionCalculated);
        }

        line.SetPosition(0, startPoint);

        for (int i = 1; i < line.positionCount; i++)
        {
            Vector3 positionCalculated = line.GetPosition(i-1) + distBones[i - 1] * Vector3.Normalize(line.GetPosition(i) - line.GetPosition(i - 1));
            line.SetPosition(i, positionCalculated);
        }
    }

    public static List<Joint> FABRIK(List<Joint> listJoint, List<Bone> listBone, Vector3 startPoint, Transform target)
    {
        listJoint.Last().position = target.position;

        listJoint = GetFabrikPosition(listJoint, listBone, FrabrikDirection.backward);

        listJoint[0].position = startPoint;

        return GetFabrikPosition(listJoint, listBone, FrabrikDirection.forward);
    }

    public static List<Joint> GetFabrikPosition(List<Joint> listJoint, List<Bone> listBone, FrabrikDirection direction)
    {
        if(direction == FrabrikDirection.backward)
        {
            listJoint.Reverse();
            listBone.Reverse();
        }

        for (int i = 1; i < listJoint.Count; i++)
        {
            listJoint[i].position = listJoint[i - 1].position + listBone[i - 1].distance * Vector3.Normalize(listJoint[i].position - listJoint[i-1].position);

            listBone[i - 1].directionForAngle = listJoint[i].position - listJoint[i-1].position;
            
            Debug.DrawLine(listJoint[i].position, listJoint[i].position + listBone[i - 1].directionForAngle.normalized * 2);
            //listJoint[i].position = GetAngledPosition(listBone[i - 1]);
        }

        if (direction == FrabrikDirection.backward)
        {
            listJoint.Reverse();
            listBone.Reverse();
        }

        return listJoint;
    }

    public static Vector3 GetAngledPosition(Bone bone)
    {
        Vector3 pointInLocal = bone.joint2.position - bone.joint1.position;
        Vector3 polarCoordinateAlpha = CartesianToPolar(pointInLocal.x, pointInLocal.y);
        float angleAlpha = polarCoordinateAlpha.x;
        float radius = polarCoordinateAlpha.y;

        Debug.DrawLine(bone.joint1.position, PolarToCartesian(bone.joint1.angleMax, radius) + bone.directionForAngle.normalized * 2);
        Debug.DrawLine(bone.joint1.position, PolarToCartesian(bone.joint1.angleMin, radius) + bone.directionForAngle.normalized * 2);


        if (angleAlpha > bone.joint1.angleMax || angleAlpha < bone.joint1.angleMin)
        {
            Vector3 pointMax = PolarToCartesian(bone.joint1.angleMax, radius);
            Vector3 pointMin = PolarToCartesian(bone.joint1.angleMin, radius);

            float distanceAlphaMax = (pointMax - pointInLocal).magnitude;
            float distanceAlphaMin = (pointMin - pointInLocal).magnitude; ;

            if (distanceAlphaMin < distanceAlphaMax)
            {
                pointInLocal = pointMin;
            }
            else
            {
                pointInLocal = pointMax;
            }
        }

        return pointInLocal + bone.joint1.position;
    }

    public enum FrabrikDirection{
        forward,
        backward
    }

    public static Vector3 CartesianToPolar(float x, float y)
    {
        double radius = Math.Sqrt((x * x) + (y * y));
        double angle = Math.Atan2(y, x);

        return new Vector3((float)angle * 180 / (float)Math.PI , (float)radius);
    }

    public static Vector3 PolarToCartesian(double angle, double radius)
    {
        double angleRad = (Math.PI / 180.0) * (angle);
        double x = radius * Math.Cos(angleRad);
        double y = radius * Math.Sin(angleRad);
        return new Vector3((float)x, (float)y);
    }

}
