using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ��޵Ǵ� ���� ��ü.
/// ���� ���� ��Ʈ�ڽ��� ���ÿ� �浹 ����. ���� ���� ����� ����� ���ÿ� ���� �� ����.
/// �ߺ��� �����ϰ� �� ���ִ� �켱�� ���� ���� �ϳ��� ��ȯ
/// </summary>
public class Attack : MonoBehaviour
{

    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    protected Unit attackUnit;
    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    public Unit AttackUnit
    {
        get
        {
            return attackUnit;
        }
    }

    [SerializeField]
    protected string targetTag;//���ظ� ���� ����� �±�
    protected bool isInitialized = false;//�ʱ�ȭ �Ǿ��°�?

    private List<DamageArea> damageAreaList = new();//��ϵ� �����
    private List<Unit> damagedUnitList = new();//����� ���� ���� ����Ʈ

    public bool dealDamageOnEnter = true;//���� �� ������� �����°�?
    public bool isDestroySelfAuto = true;//������� ���� ������� �ı��Ǵ°�?

    protected virtual void Update()
    {
        //�ʱ�ȭ ������ �� ����
        if (!isInitialized)
        {
            return;
        }

        //isDestroySelfAuto Ȱ��ȭ �� ��ϵ� ��� ������� ������� �ı�
        if (isDestroySelfAuto && damageAreaList.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���� ����. �ʱ�ȭ�� ���ÿ� �̷����.
    /// ��ũ��Ʈ���� ��ġ�� ������ �� �� ����ϸ� �� ��
    /// �ַ� ����ü �߻�
    /// </summary>
    /// <param name="unit">������ ����</param>
    /// <param name="targetTag">�ǰ� ����� �±�</param>
    /// <param name="parent">�θ� �� Transform. null�̸� �θ� ����</param>
    /// <param name="DamageAreaObjects">�ʱ⿡ ����� ������� ���� ������Ʈ</param>
    /// <returns>������ Attack</returns>
    public static Attack MakeGameObject(Unit unit, string targetTag, Transform parent = null, params GameObject[] DamageAreaObjects)
    {
        GameObject go = new GameObject();
        if (parent != null)
        {
            go.transform.parent = parent;
        }
        Attack result = go.AddComponent<Attack>();
        result.Initialization(unit, targetTag, DamageAreaObjects);
        return result;
    }

    /// <summary>
    /// �ʱ�ȭ
    /// ��� ���� �ݵ�� �ʱ�ȭ�Ͽ��� ��
    /// </summary>
    /// <param name="unit">������ ����</param>
    /// <param name="targetTag">�ǰ� ����� �±�</param>
    /// <param name="DamageAreaObjects">�ʱ⿡ ����� ������� ���� ������Ʈ</param>
    public void Initialization(Unit unit, string targetTag, params GameObject[] DamageAreaObjects)
    {
        attackUnit = unit;
        this.targetTag = targetTag;
        damageAreaList.Clear();
        damagedUnitList.Clear();
        foreach (var obj in DamageAreaObjects)
        {
            EnrollDamage(obj);
        }
        isInitialized = true;
    }

    /// <summary>
    /// ����� ���
    /// </summary>
    /// <param name="damageArea">����� �����</param>
    public void EnrollDamage(DamageArea damageArea)//����� �ϳ� ���
    {
        if (damageAreaList.Contains(damageArea))
        {
            return;
        }
        damageAreaList.Add(damageArea);
        damageArea.Source = this;
    }

    /// <summary>
    /// ���� Ȥ�� �ڽ��� ��� ����� ���
    /// </summary>
    /// <param name="go">������ �� ������Ʈ</param>
    public void EnrollDamage(GameObject go)//�� ���� ���� ����� ���
    {
        foreach (DamageArea damageArea in go.GetComponentsInChildren<DamageArea>())
        {
            EnrollDamage(damageArea);
        }
    }

    /// <summary>
    /// ����� ����
    /// </summary>
    /// <param name="damageArea">������ �����</param>
    public void WithdrawDamage(DamageArea damageArea)//����� ����
    {
        if (damageAreaList.Contains(damageArea))
        {
            damageAreaList.Remove(damageArea);
        }
    }

    /// <summary>
    /// ���� Ȥ�� �ڽ��� ��� ����� ����
    /// </summary>
    /// <param name="go">������ �� ������Ʈ</param>
    public void WithdrawDamage(GameObject go)
    {
        foreach (DamageArea damageArea in go.GetComponentsInChildren<DamageArea>())
        {
            WithdrawDamage(damageArea);
        }
    }

    public void DamageEnter()
    {
        //dealDamageOnEnter Ȱ��ȭ �� ������ �� �����
        if (dealDamageOnEnter)
        {
            foreach (var contactedUnit in GetConnectedHitbox())
            {
                if (!damagedUnitList.Contains(contactedUnit.Key))
                {
                    damagedUnitList.Add(contactedUnit.Key);
                    contactedUnit.Value.Value.DealDamage(contactedUnit.Value.Key);
                }
            }
        }
    }

    public void DamageExit()
    {

    }

    /// <summary>
    /// ������ ���� Ž��
    /// ��ϵ� Damage�� ���� ���� ��� ������ �о, �켱���� ����Ͽ� ��� ������� ����� �� �˷���
    /// </summary>
    private Dictionary<Unit, KeyValuePair<HitBox, DamageArea>> GetConnectedHitbox()
    {
        Dictionary<HitBox, DamageArea> dict = new();//�켱���� ����Ͽ�, ��� ��Ʈ�ڽ����� � ���� ������ �浹�� ������ ���� ���
        Dictionary<Unit, KeyValuePair<HitBox, DamageArea>> temp = new();
        //temp ä���
        foreach (DamageArea damageArea in damageAreaList)//��� ���� ������ ����
        {
            foreach (HitBox hitBox in damageArea.HitBoxList)//�浹 ���� ��� ��Ʈ�ڽ��� ����
            {
                if (hitBox.CompareTag(targetTag))//��ǥ �����̸�
                {
                    if (!temp.ContainsKey(hitBox.Unit))//ó�� �浹�� �����̸�
                    {
                        temp.Add(hitBox.Unit, new KeyValuePair<HitBox, DamageArea>(hitBox, damageArea));//����Ѵ�
                    }
                    else//������ �浹 �˻�� �����̶��
                    {
                        //�켱�� ���� �� ������
                        if (temp[hitBox.Unit].Key.Priority + temp[hitBox.Unit].Value.Priority < hitBox.Priority + damageArea.Priority)
                        {
                            temp[hitBox.Unit] = new KeyValuePair<HitBox, DamageArea>(hitBox, damageArea);//����Ѵ�
                        }
                    }
                }
            }
        }

        ///
        /// ������ Attack���� �� ���ֿ� �� ���� ���� üũ
        /// ��Ʈ�ڽ� ���� ���� ���� �׳� Unit���� ����Ͽ� �浹 ���� ������ ����
        /// ���� ������ �켱���� ����Ͽ� ����Ͽ���
        /// 
        /// <Unit, <HitBox, DamageArea>>�� �Ѵٸ�
        /// ù ������ ã���� ������ key��, valued�� ��ųʸ��� ����, �浹�� hitbox�� key��, �浹�� DamageArea�� value�� �ִ´�
        /// ���� �����̶�� hitbox�� damagearea�� �켱���� �ջ�, ������ ��ü��
        ///
        return temp;
    }

}