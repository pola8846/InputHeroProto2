using System;
using UnityEngine;

// Ŀ���� ��Ʈ����Ʈ
// SerializeField ����Ʈ�� �Բ� ����Ͽ� �ν����Ϳ��� �˾�â���� �ڽ� Ŭ���� ���¸� ������ �� �ְ� �Ѵ�
// [SerializeField, SubclassSelector] public List<BaseClass> ... <-�̷���

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SubclassSelectorAttribute : PropertyAttribute { }