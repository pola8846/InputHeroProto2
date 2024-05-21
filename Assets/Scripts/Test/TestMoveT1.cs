using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveT1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MoverByTransform mover = GetComponent<MoverByTransform>();
        mover.StartMove(MoverByTransform.moveType.ByFunction, 1000, move);
    }

    float cic = .5f;

    Vector2 move(float t)
    {
        t = t * 7;
        float y = Mathf.Sin(-t) / 4f;
        float x = (Mathf.Cos(t) + t)/6f;
        return new Vector2(x, y);
    }
}
