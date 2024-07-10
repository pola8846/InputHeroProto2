using UnityEngine;

public class HitBox : MonoBehaviour, IBulletHitChecker
{
    /// <summary>
    /// ��ü ����. �ڵ����� ��ϵ�
    /// </summary>
    private Unit unit;
    /// <summary>
    /// ��ü ����
    /// </summary>
    public Unit Unit
    {
        get { return unit; }
        set { unit = value; }
    }


    /// <summary>
    /// �켱��. ���� ���� �켱��.
    /// </summary>
    [SerializeField]
    private int priority = 0;
    public int Priority { get => priority; }

    protected virtual void Start()
    {
        //�ڵ� ���
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
            Debug.LogWarning($"HitBox: {gameObject}���� ��ü Unit�� ã�� ����");
        }
    }

    /// <summary>
    /// ��Ʈ�ڽ��� ���� ������� �����ϴ� ����
    /// HitBox�� DamageArea�� �켱���� ���� ���� ���� ���� �ϳ��� �ǰ� ó��
    /// ����Ͽ� override�Ͽ� �ǰ� �� ���� �ڵ� ���� ����
    /// </summary>
    /// <param name="damage">���� �����</param>
    public virtual void Damage(float damage)
    {
        unit.Damage(damage);
    }
}
