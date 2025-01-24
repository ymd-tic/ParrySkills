using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGolemCtrl : EnemyBase
{
    private enum AIState // ��ԃp�^�[��
    {
        Idle,       // �ҋ@
        Chase,      // �ǐ�
        Atack,      // �U��
        KnockBack   // �m�b�N�o�b�N
    }

    public enum AtackState // �U���p�^�[��
    {
        Melee1,     // �ߐ�1
        Melee2,     // �ߐ�2
        Jump        // �W�����v
    }

    //-----SerializeField------------------------------------------------------------
    [Header("�N�[���^�C��")]
    [SerializeField] private CoolTime atackTime;   // �U���N�[���^�C��


    //-----privateField--------------------------------------------------------------
    private AIState aiState = AIState.Idle;
    private AtackState atackState = AtackState.Melee1;



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �V�X�e��

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        switch (aiState)
        {
            case AIState.Idle:
                Idle();
                break;

            case AIState.Chase:
                Chase();
                break;

            case AIState.Atack:
                Atack();
                break;

            case AIState.KnockBack:
                KnockBack();
                break;
        }
    }

    #endregion

    #region ��ԃX�e�[�g

    private void Idle()
    {
        // �v���C���[�Ƃ̋������ǐՔ͈͓��Ȃ�
        if(DistanceFromPlayer() <= range.far)
        {
            if(DistanceFromPlayer() > range.atack)
            {
                ChangeAIState(AIState.Chase);
                return;
            }
        }

        // �U���N�[���^�C�����I�������U���X�e�[�g�Ɉڂ�
        atackTime.cur += Time.deltaTime;
        if (atackTime.cur > atackTime.goal)
        {
            ChangeAIState(AIState.Atack);
            canDamageAnim = false;
            atackTime.cur = 0;
            return;
        }
    }

    private void Chase()
    {
        agent.destination = playerPos.position;

        // �v���C���[�Ƃ̋������U���͈͓��Ȃ�
        if (DistanceFromPlayer() <= range.atack)
        {
            ChangeAIState(AIState.Idle);
            return;
        }
    }

    private void Atack()
    {
        // �U������
        switch (atackState)
        {
            case AtackState.Melee1:
                Melee1();
                break;

            case AtackState.Melee2:
                Melee2();
                break;

            case AtackState.Jump:
                Jump();
                break;
        }

        // �U�����[�V�����̏I��
        if (AnimationEnd("Atack"))
        {
            // �v���C���[�Ƃ̋������U���͈͊O
            if (DistanceFromPlayer() > range.atack)
            {
                ChangeAIState(AIState.Chase);
            }
            else
            {
                ChangeAIState(AIState.Idle);
            }

            animator.SetInteger("AtackValue", 0);
        }

        #region �U���X�e�[�g

        void Melee1()
        {

        }

        void Melee2()
        {

        }

        void Jump()
        {

        }
        #endregion

    }

    private void KnockBack()
    {
        if (AnimationEnd("KnockBack"))
        {
            rigidbody.isKinematic = true;
            canDamageAnim = true;

            // �v���C���[�Ƃ̋������ǐՔ͈͓��Ȃ�
            if (DistanceFromPlayer() <= range.far)
            {   // ���U���͈͓��Ȃ�
                if (DistanceFromPlayer() <= range.atack)
                {
                    ChangeAIState(AIState.Idle);
                    return;
                }

                ChangeAIState(AIState.Chase);
            }
        }
    }
    #endregion

    #region �G�l�~�[�̐���

    public override void TakeParry()
    {
        base.TakeParry();

        rigidbody.isKinematic = false;
        ChangeAIState(AIState.KnockBack);
        transform.LookAt(playerPos);
    }

    /// <summary>
    /// ��ԃX�e�[�g��ς���
    /// �X�e�[�g���̃A�j���[�V�������Đ�
    /// </summary>
    /// <param name="_nextState">���̃X�e�[�g</param>
    private void ChangeAIState(AIState _nextState)
    {
        if (isDie) { return; }

        // �X�e�[�g�X�V
        aiState = _nextState;

        // �A�j���[�V�����X�V
        animator.SetTrigger($"{_nextState}");   

        foreach (var animState in animator.parameters)
        {
            if(animState.type != AnimatorControllerParameterType.Trigger) {  continue; }

            if (animState.name != $"{_nextState}")
            {
                animator.ResetTrigger($"{animState.name}");
            }
        }

        // ���̃X�e�[�g�Ɉڂ鎞��1�񂾂��Ă΂�鏈��
        switch (_nextState)
        {
            case AIState.Idle:
                agent.speed = speed.zero;
                agent.destination = enemyPos.position;
                break;

            case AIState.Chase:
                agent.speed = speed.fast;
                agent.destination = playerPos.position;
                break;

            case AIState.Atack:
                // �U���������_���őI��
                AtackState atack = EnumGeneric.GetRandom<AtackState>();
                SetAtackState(atack);

                // �U���N�[���^�C���������_���Őݒ�
                atackTime.goal = Generic.RandomErrorRange(atackTime.def, 0.5f);

                transform.LookAt(playerPos.position);
                agent.speed = speed.zero;
                agent.destination = enemyPos.position;
                break;

            case AIState.KnockBack:
                agent.speed = speed.zero;
                agent.destination = enemyPos.position;
                break;
        }

        //Debug.Log($"{_nextState}�X�e�[�g�ɍX�V");
    }

    /// <summary>
    /// �U���X�e�[�g��ݒ�
    /// </summary>
    /// <param name="_atack">�U��</param>
    private void SetAtackState(AtackState _atack)
    {
        atackState = _atack;

        animator.SetInteger("AtackValue",(int)_atack + 1);
        // +1�Ƃ��Ă���̂�Animator�̊e�J�ڏ�����1����n�܂邽��

        // �U���X�e�[�g�Ɉڂ鎞��1�񂾂��Ă΂�鏈��
        switch (_atack)
        {
            case AtackState.Melee1:
                atackPower = Generic.RandomErrorRange(-10.0f, 2.0f);
                break;

            case AtackState.Melee2:
                atackPower = Generic.RandomErrorRange(-15.0f, 2.0f);
                break;

            case AtackState.Jump:
                atackPower = Generic.RandomErrorRange(-20.0f, 3.0f);
                break;
        }
    }

    #endregion
}
