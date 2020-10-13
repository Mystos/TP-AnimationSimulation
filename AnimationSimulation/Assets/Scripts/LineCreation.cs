using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreation : MonoBehaviour
{
    public LineRenderer line;
    public Transform targetPoints;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePoint(line, targetPoints);
    }

    public static void MovePoint(LineRenderer line, Transform target) 
    {
        // Move Last Index
        line.SetPosition(line.positionCount -1, target.position);
    }
}
