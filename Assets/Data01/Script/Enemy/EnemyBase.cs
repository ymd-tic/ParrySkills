using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyBase : MonoBehaviour
{
    [Serializable]
    protected class CoolTime // �N�[���^�C��
    {
        public float def = 0; // �f�t�H���g����
        [NonSerialized] public float cur = 0;   // ����
        [NonSerialized] public float goal = 0;  // �ڕW
    }

    [Serializable]
    protected class Range // �s���p�^�[���͈�
    {
        public float far = 0;   // ����
        public float near = 0;  // �߂�
        public float atack = 0; // �U������
    }

    [Serializable]
    protected class Speed // �ړ����x
    {
        public float fast = 0;  // ����
        public float slow = 0;  // �x��
        [NonSerialized]
        public readonly float zero = 0;  // ��~
    }

    //-----SerializeField------------------------------------------------------------
    [Header("�X�e�[�^�X")]          
    [SerializeField] private float maxHp = 100; // �ő�HP
    [SerializeField] protected Speed speed;     // �ړ����x
    [SerializeField] protected Range range;     // �s���p�^�[���͈�

    [Header("�G�t�F�N�g")]
    [SerializeField] private ParticleSystem parryEfect; // �p���B�G�t�F�N�g
    [SerializeField] private GameObject damageTextObj;  // �_���[�WUI


    //-----privateField--------------------------------------------------------------
    private Generic.ParamateValue hpValue;  // HP
    private CapsuleCollider capsuleCollider;// �J�v�Z���R���C�_�[
    private TMP_Text damageText;            // �_���[�WUI�e�L�X�g


    //-----publicField---------------------------------------------------------------
    [NonSerialized]public float atackPower = 10;


    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------
    protected Transform playerPos;  // �v���C���[���W
    protected Transform enemyPos;   // �G���W
    protected bool canDamageAnim = true;   // �_���[�W�A�j���[�V�����̍Đ��t���O
    protected bool isDie = false;          // ���S�t���O
    protected NavMeshAgent agent;
    protected Animator animator;
    protected new Rigidbody rigidbody;


    #region �V�X�e��

    protected virtual void Start()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
        enemyPos = this.gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        damageText = damageTextObj.transform.GetChild(0).GetComponent<TMP_Text>();

        hpValue = new Generic.ParamateValue(maxHp, maxHp, 0);
        //atackCoolTime = Generic.RandomPointRange(dafaultAtackCoolTime, 0.5f);
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
        Instantiate(damageTextObj, popTextPos, Quaternion.identity);

        if (hpValue.cur <= hpValue.min)
        {
            Die();
        }
    }

    /// <summary>
    /// �p���B���ꂽ����
    /// </summary>
    public virtual void TakeParry()
    {
        if(isDie) { return; }
    }

    /// <summary>
    /// HP��0�ɂȂ�����Ă΂��
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
    protected bool AnimationEnd([HideInInspector]string _animName)
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
