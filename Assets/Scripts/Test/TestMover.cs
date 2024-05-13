using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour
{
    [SerializeField]
    private PlayerUnit mMoveReceiver;
    // Update is called once per frame
    void Update()
    {
        if (mMoveReceiver != null)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    mMoveReceiver.KeyDown(key);
                }
                

                if (Input.GetKeyUp(key))
                {
                    mMoveReceiver.KeyUp(key);
                }
            }
        }
    }
}
