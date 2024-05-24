using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttttttt : MonoBehaviour
{
    Dictionary<float, float> dic = new();
    public float value;

    private void Start()
    {
        dic.Clear();

        dic.Add(0, 0);
        dic.Add(10, 10);
        dic.Add(20, 50);
        dic.Add(30, 150);

        
    }

    private void Update()
    {
        float temp = GameTools.GetNonlinearGraph(dic, value);
        Debug.Log(temp);
    }
}
