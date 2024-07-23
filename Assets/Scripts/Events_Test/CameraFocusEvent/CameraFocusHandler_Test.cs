using DG.Tweening;
using Febucci.UI.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// �ӽ÷� ���� ī�޶� ��Ŀ�� �̺�Ʈ
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