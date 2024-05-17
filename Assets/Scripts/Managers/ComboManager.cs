using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    //싱글톤
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
    private List<InputType> log = new();
    private static List<InputType> Log => instance.log;
    private Queue<PlayerSkill> findedSkills = new();
    private static Queue<PlayerSkill> FindedSkills => instance.findedSkills;

    [SerializeField, Range(1, 12)]
    private int maxCombo;

    /// <summary>
    /// 유효한 키 입력
    /// </summary>
    /// <param name="input">입력한 키</param>
    /// <returns>최대치까지 찼는가?</returns>
    public static bool InputLog(InputType input)
    {
        if (Log.Count >= instance.maxCombo)
        {
            return true;
        }

        if (ComboInputs.Contains(input))
        {
            Debug.Log($"입력: {input}");
            Log.Add(input);
        }

        return Log.Count >= instance.maxCombo;
    }

    public static void FindCombos(List<PlayerSkill> skillList)
    {
        //로그 전체를 돌며 콤보를 찾는다
        for (int i = 0; i < Log.Count; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                //index가 i인 item(i+1번째 item)을 마지막으로 하여 j개만큼 비교

                //배열 복사
                InputType[] temp = new InputType[j + 1];
                Log.CopyTo(i - j, temp, 0, j + 1);

                //일치하는 스킬이 있는지 탐색
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
    }
}
