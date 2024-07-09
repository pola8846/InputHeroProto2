using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class ComboManager : MonoBehaviour
{
    //�̱���
    private static ComboManager instance;
    public static ComboManager Instance => instance;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField]
    private List<InputType> comboInputs = new();
    private static List<InputType> ComboInputs => instance.comboInputs;
    [SerializeField]
    private SerializedDictionary<InputType, float> comboInputsTime = new();
    private static SerializedDictionary<InputType, float> ComboInputsTime => instance.comboInputsTime;
    private List<InputType> log = new();
    private static List<InputType> Log => instance.log;
    private Queue<PlayerSkill> findedSkills = new();
    private static Queue<PlayerSkill> FindedSkills => instance.findedSkills;

    [SerializeField]
    private int maxCombo;

    /// <summary>
    /// ��ȿ�� Ű �Է�
    /// </summary>
    /// <param name="input">�Է��� Ű</param>
    /// <returns>�ִ�ġ���� á�°�?</returns>
    public static bool InputLog(InputType input)
    {
        if (Log.Count >= instance.maxCombo)
        {
            return true;
        }

        if (ComboInputs.Contains(input))
        {
            //Debug.Log($"�Է�: {input}");
            Log.Add(input);
            //UIManager.Instance.testActionBar.GetComponent<TestActionBar>().add(input);
        }
        if (ComboInputsTime.ContainsKey(input))
        {
            TimeManager.DecreaseComboTime(ComboInputsTime[input]);
        }

        return Log.Count >= instance.maxCombo;
    }

    public static void FindCombos(List<PlayerSkill> skillList)
    {
        //�α� ��ü�� ���� �޺��� ã�´�
        for (int i = 0; i < Log.Count; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                //index�� i�� item(i+1��° item)�� ���������� �Ͽ� j����ŭ ��

                //�迭 ����
                InputType[] temp = new InputType[j + 1];
                Log.CopyTo(i - j, temp, 0, j + 1);

                //��ġ�ϴ� ��ų�� �ִ��� Ž��
                foreach (PlayerSkill skill in skillList)
                {
                    if (GameTools.CompareEnumList(skill.NeededCombo, temp))
                    {
                        FindedSkills.Enqueue(skill);
                    }
                }
            }

        }
        Log.Clear();
    }

    public static PlayerSkill GetFindedSkill()
    {
        if (FindedSkills.Count==0)
        {
            return null;
        }
        else
        {
            return FindedSkills.Dequeue();
        }
    }

    public static void Reset()
    {
        Log.Clear();
        FindedSkills.Clear();
        //UIManager.Instance.testActionBar.GetComponent<TestActionBar>().removeAll();
    }
}
