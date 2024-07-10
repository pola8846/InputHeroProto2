using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkText : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       
        timerMode = true;
    }


    [SerializeField] float maxTime;
    [SerializeField] float blinkTime;
    float time;
    bool timerMode;
    [SerializeField] float randomMaxTimeRange;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (timerMode)
        {
            if (time > maxTime)
            {
                spriteRenderer.material.SetColor("_EmissionColor", Color.grey);
                timerMode = false;
                time = 0;

                maxTime = Random.Range(0, randomMaxTimeRange);
            }
        }
        else
        {
            if (time > blinkTime)
            {
                spriteRenderer.material.SetColor("_EmissionColor", Color.white);
                timerMode = true;
                time = 0;
            }
        }

    }
}
