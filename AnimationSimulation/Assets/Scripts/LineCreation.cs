using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineCreation : MonoBehaviour
{
    public LineRenderer line;
    public Transform targetPoints;
    public float[] distBones;
    private Vector3[] joints;
    private Vector3 startPoint;
    private float angleMax = 45f;


    // Start is called before the first frame update
    void Start()
    {
        joints = new Vector3[] { Vector3.zero, new Vector3(1,0,0), new Vector3(2, 0, 0), new Vector3(3, 0, 0) };
        startPoint = joints[0];
        line.positionCount = joints.Length;
        line.SetPositions(joints);

        distBones = new float[joints.Length - 1];
        for (int i = 0; i < joints.Length -1; i++)
        {
            distBones[i] = (joints[i] - joints[i + 1]).magnitude;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FABRIK(line,distBones,startPoint, targetPoints);
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
}
