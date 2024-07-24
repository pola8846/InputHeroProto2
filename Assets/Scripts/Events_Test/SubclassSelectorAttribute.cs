using System;
using UnityEngine;

// 커스텀 어트리뷰트
// SerializeField 리스트와 함께 사용하여 인스펙터에서 팝업창으로 자식 클래스 형태를 선택할 수 있게 한다
// [SerializeField, SubclassSelector] public List<BaseClass> ... <-이런식

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SubclassSelectorAttribute : PropertyAttribute { }