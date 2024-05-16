using System.Collections.Generic;
using UnityEngine;

public class ComboManager
{
    //싱글톤
    private static ComboManager instance;
    public static ComboManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ComboManager();
            }
            return instance;
        }
    }

    [SerializeField]
    private List<InputType> comboInputs = new();
    private List<InputType> log = new();
    private Queue<PlayerSkill> findedSkills = new();

    [SerializeField, Range(1, 12)]
    private int maxCombo;

    /// <summary>
    /// 유효한 키 입력
    /// </summary>
    /// <param name="input">입력한 키</param>
    /// <returns>최대치까지 찼는가?</returns>
    public bool InputLog(InputType input)
    {
        if (log.Count >= maxCombo)
        {
            return true;
        }

        if (comboInputs.Contains(input))
        {
            log.Add(input);
        }

        return log.Count >= maxCombo;
    }

    public void FindCombos(List<PlayerSkill> skillList)
    {
        //로그 전체를 돌며 콤보를 찾는다
        for (int i = 0; i < log.Count; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                //index가 i인 item(i+1번째 item)을 마지막으로 하여 j개만큼 비교

                //배열 복사
                InputType[] temp = new InputType[j + 1];
                log.CopyTo(i - j, temp, 0, j + 1);

                //일치하는 스킬이 있는지 탐색
                foreach (PlayerSkill skill in findedSkills)
                {
                    if (GameTools.CompareEnumList(skill.NeededCombo, temp))
                    {
                        findedSkills.Enqueue(skill);
                    }
                }
            }

        }
        log.Clear();
    }

    public void Reset()
    {
        comboInputs.Clear();
        log.Clear();
        findedSkills.Clear();
    }
}
