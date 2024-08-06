using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class TimeManager : MonoBehaviour
{
    //�̱���
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    //���� ������
    private const int framePerSec = 50;

    //�ð� ���� �� �̺�Ʈ
    private static event EventHandler<float> onTimeScaleChanged;
    public static event EventHandler<float> OnTimeScaleChanged
    {
        add { onTimeScaleChanged += value; }
        remove { onTimeScaleChanged -= value; }
    }

    //���ο�
    [SerializeField] private float slowRate = .5f;//���ο� ����
    public static float SlowRate => instance.slowRate;//���ο� ����
    [SerializeField] private float slowStartTime = .6f;
    [SerializeField] private float slowEndTime = .3f;

    [SerializeField]
    private float slowTime = 4f;//���ο� ���� �ð�

    [SerializeField]
    private bool isSlowed = false;//���ο� ���ΰ�?
    [SerializeField]
    private bool isSlowing = false;//���ο� �ɰų� �����ϴ� ���ΰ�?
    public static bool IsSlowed => instance.isSlowed;
    public static bool IsSlowing => instance.isSlowing;
    private bool isUsingSkills = false;//��ų ��� ���ΰ�?
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


    private TickTimer slowTimer;//���ο� ���� �ð� Ÿ�̸�
    private TickTimer slowEnterTimer;//���ο� ����/���� �ð� Ÿ�̸�
    [SerializeField] private Slider slowSlider;//���ο� ���� �ð� UI

    private StudioEventEmitter eventEmitter;
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //�̱���
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
        eventEmitter = GetComponent<StudioEventEmitter>();
    }

    private void Update()
    {
        //���ο� ���߿�
        if (isSlowed && !isUsingSkills && !isSlowing)
        {
            float remainTime = slowTimer.GetRemain(slowTime);
            //slowSlider.value = remainTime / instance.slowTime;//������ ����
            //���ο� �ð��� �� ��ٸ�
            if (remainTime <= 0)
            {
                EndSlow();
                //StartCoroutine(EndingSlow());
            }
        }
    }

    #region �Ҹ�Ÿ��

    /// <summary>
    /// �Ҹ�Ÿ�� ����
    /// </summary>
    public static void StartSlow()
    {
        instance.isSlowing = true;
        instance.isSlowed = true;
        instance.StartCoroutine(instance.StartingSlow());
    }

    /// <summary>
    /// �Ҹ�Ÿ�� ����(��ų ������)
    /// </summary>
    public static void EndSlow()
    {
        instance.isSlowing = true;
        instance.StartCoroutine(instance.EndingSlow());
    }

    //���ο� �ɱ� ����
    private IEnumerator StartingSlow()
    {
        slowEnterTimer.Reset();
        eventEmitter.SetParameter("Magnet_Ingame", 1);
        while (true)
        {
            float remainTime = slowEnterTimer.GetRemain(slowStartTime);//���� �ð�
            float remainTimeRate = remainTime / slowStartTime;//���� �ð� ����, 1->0���� ���� ����

            SetTimeScale(slowRate + (Mathf.Lerp(0, 1- slowRate, remainTimeRate)));

            if (remainTime <= 0)//������ ���ο� �ɸ��� ����
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
        eventEmitter.SetParameter("Magnet_Ingame", 0);
        while (true)
        {
            float remainTime = slowEnterTimer.GetRemain(slowEndTime);//���� �ð�
            float remainTimeRate = remainTime / slowEndTime;//���� �ð� ����, 1->0���� ���� ����

            SetTimeScale(slowRate + (Mathf.Lerp(1 - slowRate, 0, remainTimeRate)));

            if (remainTime <= 0)//������ ���ο� �ɸ��� ����
            {
                instance.slowTimer.Reset();
                isSlowing = false;
                isSlowed = false;
                break;
            }

            yield return null;
        }
    }
    #endregion

    //�ð� ���� ����
    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        onTimeScaleChanged?.Invoke(instance, Time.timeScale);
    }

    //�ð� �Ҹ�
    public static void DecreaseSlowTime(float time)
    {
        instance.slowTimer.AddOffset(time);
    }

}
