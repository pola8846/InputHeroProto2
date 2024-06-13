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
    [SerializeField] private float slowRate = .5f;//슬로우 배율
    public static float SlowRate => instance.slowRate;//슬로우 배율
    [SerializeField] private float slowStartTime = .6f;
    [SerializeField] private float slowEndTime = .3f;

    [SerializeField]
    private float slowTime = 4f;//슬로우 지속 시간

    [SerializeField]
    private bool isSlowed = false;//슬로우 중인가?
    [SerializeField]
    private bool isSlowing = false;//슬로우 걸거나 종료하는 중인가?
    public static bool IsSlowed => instance.isSlowed;
    public static bool IsSlowing => instance.isSlowing;
    private bool isUsingSkills = false;//스킬 사용 중인가?
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


    private TickTimer slowTimer;//슬로우 남은 시간 타이머
    private TickTimer slowEnterTimer;//슬로우 진입/해제 시간 타이머
    [SerializeField] private Slider slowSlider;//슬로우 남은 시간 UI

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
        slowEnterTimer = new(unscaledTime: true);
    }

    private void Update()
    {
        //슬로우 도중에
        if (isSlowed && !isUsingSkills && !isSlowing)
        {
            float remainTime = slowTimer.GetRemain(slowTime);
            slowSlider.value = remainTime / instance.slowTime;//게이지 조절
            //슬로우 시간을 다 썼다면
            if (remainTime <= 0)
            {
                EndSlow();
                //StartCoroutine(EndingSlow());
                //StartCoroutine(StartSkillQueue());//스킬 실행 시작
            }
        }
    }

    #region 불릿타임

    /// <summary>
    /// 불릿타임 시작
    /// </summary>
    public static void StartSlow()
    {
        instance.isSlowing = true;
        instance.isSlowed = true;
        instance.StartCoroutine(instance.StartingSlow());

        //SetTimeScale(instance.slowRate);
    }

    /// <summary>
    /// 불릿타임 종료(스킬 사용까지)
    /// </summary>
    public static void EndSlow()
    {
        instance.isSlowing = true;
        instance.StartCoroutine(instance.EndingSlow());

        //SetTimeScale(1);
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

    //슬로우 걸기 시작
    private IEnumerator StartingSlow()
    {
        slowEnterTimer.Reset();
        while (true)
        {
            float remainTime = slowEnterTimer.GetRemain(slowStartTime);//남은 시간
            float remainTimeRate = remainTime / slowStartTime;//남은 시간 비율, 1->0으로 점차 이전

            SetTimeScale(slowRate + (Mathf.Lerp(0, 1- slowRate, remainTimeRate)));

            if (remainTime <= 0)//완전히 슬로우 걸리면 종료
            {
                instance.slowTimer.Reset();
                isSlowing = false;
                break;
            }

            yield return null;
        }
    }
    private IEnumerator EndingSlow()
    {
        slowEnterTimer.Reset();
        while (true)
        {
            float remainTime = slowEnterTimer.GetRemain(slowEndTime);//남은 시간
            float remainTimeRate = remainTime / slowEndTime;//남은 시간 비율, 1->0으로 점차 이전

            SetTimeScale(slowRate + (Mathf.Lerp(1 - slowRate, 0, remainTimeRate)));

            if (remainTime <= 0)//완전히 슬로우 걸리면 종료
            {
                instance.slowTimer.Reset();
                isSlowing = false;
                isSlowed = false;
                ComboManager.Reset();
                break;
            }

            yield return null;
        }
    }
    #endregion

    //시간 배율 조정
    public static void SetTimeScale(float scale)
    {
        Debug.Log(scale);
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        onTimeScaleChanged?.Invoke(instance, Time.timeScale);
    }

    //시간 소모
    public static void DecreaseComboTime(float time)
    {
        instance.slowTimer.AddOffset(time);
    }

}
