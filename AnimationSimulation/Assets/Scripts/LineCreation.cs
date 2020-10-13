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
        }

        if (direction == FrabrikDirection.backward)
        {
            listJoint.Reverse();
            listBone.Reverse();
        }

        return listJoint;
    }

    public enum FrabrikDirection{
        forward,
        backward
    }



}
