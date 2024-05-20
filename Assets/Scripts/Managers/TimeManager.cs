using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //싱글톤
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    //물리 프레임
    private const int framePerSec = 50;

    //시간 변경 시 이벤트
    private static event EventHandler<float> onTimeScaleChanged;
    public static event EventHandler<float> OnTimeScaleChanged
    {
        add { onTimeScaleChanged += value; }
        remove { onTimeScaleChanged -= value; }
    }

    //슬로우
    [SerializeField]
    private float slowRate = .5f;
    public static float SlowRate => instance.slowRate;

    [SerializeField]
    private float slowTime = 4f;

    private bool isSlowed = false;
    public static bool IsSlowed => instance.isSlowed;
    private bool isUsingSkills = false;
    public static bool IsUsingSkills
    {
        get
        {
            return instance.isUsingSkills;
        }
        set
        {
            instance.isUsingSkills = value;
        }
    }


    private TickTimer slowTimer;
    [SerializeField]
    private Slider slowSlider;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //싱글톤
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void Start()
    {
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        slowTimer = new(unscaledTime: true);
    }

    private void Update()
    {
        //슬로우 도중에
        if (isSlowed && !isUsingSkills)
        {
            float remainTime = slowTimer.GetRemain(slowTime);
            slowSlider.value = remainTime / instance.slowTime;//게이지 조절
            //슬로우 시간을 다 썼다면
            if (remainTime <= 0)
            {
                StartCoroutine(StartSkillQueue());//스킬 실행 시작
            }
        }
    }

    #region 불릿타임

    /// <summary>
    /// 불릿타임 시작
    /// </summary>
    public static void StartSlow()
    {
        instance.isSlowed = true;
        SetTimeScale(instance.slowRate);
        instance.slowTimer.Reset();
    }

    //스킬 사용
    private IEnumerator StartSkillQueue()
    {
        isUsingSkills = true;
        ComboManager.FindCombos(GameManager.Player.SkillList);

        //스킬 실행부
        while (true)
        {
            PlayerSkill skill = ComboManager.GetFindedSkill();//큐에서 스킬 찾아오기
            if (skill == null)//남은 스킬이 없다면 끝
            {
                break;
            }
            else
            {
                skill.Invoke();//스킬 실행
                yield return new WaitUntil(() => !PlayerSkill.IsUsing);//실행한 스킬이 끝날 때까지 대기
            }
        }


        EndSlow();
        isUsingSkills = false;
    }

    /// <summary>
    /// 불릿타임 종료(스킬 사용까지)
    /// </summary>
    public static void EndSlow()
    {
        SetTimeScale(1);
        ComboManager.Reset();
        instance.isSlowed = false;
    }

    #endregion

    //시간 배율 조정
    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        onTimeScaleChanged?.Invoke(instance, Time.timeScale);
    }
}
