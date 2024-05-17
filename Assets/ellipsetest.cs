using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ellipsetest : MonoBehaviour
{

    [SerializeField, Range(0, 100)] float maxX;
    [SerializeField, Range(0, 100)] float maxY;

    [SerializeField] float x;
    [SerializeField] float y;

    public Transform target;
    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        float t = Mathf.Clamp(angle, 0, Mathf.PI * 2);

        float a = maxX * Mathf.Cos(t);
        float b = maxY * Mathf.Sin(t);

        

    }

    Vector3 getEllipseEdgesPos(Vector3 Pos, Vector3 player, float maxX, float maxY, float z)
    {
        Vector3 returnPos = Vector3.zero;

        Vector3 v = Pos - player;
        float angle = Mathf.Atan2(v.y * maxX / maxY, v.x) * Mathf.Rad2Deg;

        returnPos.x = player.x + Mathf.Cos(Mathf.Deg2Rad*angle) * maxX; 
        returnPos.y = player.y + Mathf.Sin(Mathf.Deg2Rad*angle) * maxY;
        returnPos.z = z;

        return returnPos;

    }
}
