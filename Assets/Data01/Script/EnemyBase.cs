using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("�ő�Hp")]          [SerializeField] private   float maxHp = 100;
    [Header("�U���N�[���^�C��")][SerializeField] protected float atackCoolTime = 3;
    [Header("�_���\�W�\�L")]    [SerializeField] private GameObject damageTextPrefab;
    [Header("�p���B�\�G�t�F�N�g")][SerializeField] private ParticleSystem parryEfect;

    [Header("�s���p�^�[���͈�")][SerializeField] protected Range range;
    [System.Serializable] protected struct Range { public float chase, atack; }

    [Header("���񑬓x")][SerializeField] protected Speed speed;
    [System.Serializable] protected struct Speed { public float patrol, chase, zero; }

    //-----privateField--------------------------------------------------------------
    private Generic.ParamateValue hpValue;
    private CapsuleCollider capsuleCollider;
    private TMP_Text damageText;

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------
    protected Transform playerPos;  // �v���C���[���W
    protected Transform enemyPos;   // �G���W
    protected bool canDamageAnim = true;   // �_���[�W�A�j���[�V�������Đ����邩
    protected NavMeshAgent navMesh;
    protected Animator animator;
    protected new Rigidbody rigidbody;
    protected float curIdleTime = 0;  // �ҋ@����
    protected bool isDie = false;


    #region �V�X�e��

    protected virtual void Start()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
        enemyPos = this.gameObject.transform;
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        damageText = damageTextPrefab.transform.GetChild(0).GetComponent<TMP_Text>();

        hpValue = new Generic.ParamateValue(maxHp, maxHp, 0);
    }


    protected virtual void Update()
    {
        if (AnimationEnd("Die")) // ���ʃA�j���[�V�������I����������
        {
            Debug.Log($"{gameObject.name}��|����");
            Destroy(this.gameObject);
        }

        if (isDie) { return; }  // ���񂾂牽�����Ȃ�
    }

    /// <summary>
    /// �v���C���[�Ƃ̋�����Ԃ�
    /// </summary>
    /// <returns>����</returns>
    protected float DistanceFromPlayer()
    {
        return Vector3.Distance(playerPos.position, enemyPos.position);
    }
    #endregion


    #region �G�l�~�[�̐���

    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    /// <param name="_damage">�_���[�W</param>
    public virtual void TakeDamage(int _damage)
    {
        if (isDie) { return; }

        hpValue.cur += _damage;

        // �󂯂��_���[�W�𔽉f
        damageText.text = $"{Mathf.Abs(_damage)}";
        // UI�̃|�b�v�A�b�v�ʒu
        Vector3 popTextPos = new Vector3(enemyPos.position.x, 2.5f, enemyPos.position.z);
        // �_���[�WUI����
        Instantiate(damageTextPrefab, popTextPos, Quaternion.identity);

        if (hpValue.cur <= hpValue.min)
        {
            Die();
        }
    }

    /// <summary>
    /// �p���B�����
    /// </summary>
    public virtual void TakeParry()
    {
        if(isDie) { return; }

    }

    /// <summary>
    /// HP��0�ɂȂ�����
    /// </summary>
    private void Die()
    {
        isDie = true;
        animator.SetTrigger("Die");
        AreaManager.enemyList.Remove(this.gameObject);
        capsuleCollider.enabled = false;
    }

    #endregion


    #region �A�j���[�V����Event

    /// <summary>
    /// �p���B�G�t�F�N�g���o��
    /// </summary>
    private void PlayParryEfect()
    {
        parryEfect.Play();
    }

    /// <summary>
    /// �A�j���[�V�����̏I��
    /// </summary>
    /// <param name="_animName">�X�e�[�g��</param>
    /// <returns></returns>
    protected bool AnimationEnd(string _animName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(_animName))
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag(_animName))
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }
        return false;
    }
    #endregion
}
