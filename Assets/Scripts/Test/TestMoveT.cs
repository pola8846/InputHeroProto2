using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoverByTransform mover = GetComponent<MoverByTransform>();
        //mover.StartMove(MoverByTransform.moveType.ByFunction, 1000, move);
    }


    Vector2 move(float t)
    {
        return new Vector2(t, 0);
    }
}
