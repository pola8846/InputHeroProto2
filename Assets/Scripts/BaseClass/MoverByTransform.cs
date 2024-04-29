using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoverByTransform : MonoBehaviour
{
    [SerializeField]
    private bool isSetByLocal = true;
    private Vector2 Position
    {
        get
        {
            if (isSetByLocal)
            {
                return transform.localPosition;
            }
            else
            {
                return transform.position;
            }
        }
        set
        {
            if (isSetByLocal)
            {
                transform.localPosition = value;
            }
            else
            {
                transform.position = value;
            }
        }
    }


}
