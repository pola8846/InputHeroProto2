using DG.Tweening;
using Febucci.UI.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// 임시로 만든 카메라 포커스 이벤트
[CreateAssetMenu(menuName = "CameraFocusHandler_Test_JW")]
public class CameraFocusHandler_Test : CustomHandler_Test
{
    public GameObject ObjectToFocus;
    public GameObject Camera;

    public override void Enter()
    {
        base.Enter();

        if (ObjectToFocus != null && Camera != null)
        {
            Camera.GetComponent<CameraTracking>().SetFocusPoint(ObjectToFocus);
        }

        lifeCycleBools.isDone_This = true;
    }
}