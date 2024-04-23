using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField]
    private Unit unit;
    public Unit Unit
    {
        get { return unit; }
        set { unit = value; }
    }
    protected virtual void Start()
    {
        var temp = GetComponentInParent<Unit>();
        if (temp != null)
        {
            Unit = temp;
            gameObject.layer = LayerMask.NameToLayer("HitBox");
            gameObject.tag = Unit.gameObject.tag;
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            Debug.LogWarning($"HitBox: {gameObject}에서 모체 Unit을 찾지 못함");
        }
    }
}
