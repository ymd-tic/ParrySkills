using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGolemCtrl : EnemyBase
{

    //-----SerializeField------------------------------------------------------------



    //-----privateField--------------------------------------------------------------

    private enum AIState // ��ԃp�^�[��
    {
        Idle,       // �ҋ@
        Chase,      // �ǐ�
        Atack,      // �U��
        KnockBack   // �m�b�N�o�b�N
    }
    AIState aiState = AIState.Idle;

    public enum AtackState // �U���p�^�[��
    {
        Melee1,     // �ߐ�1
        Melee2,     // �ߐ�2
        Jump        // �W�����v
    }
    AtackState atackState = AtackState.Melee1;



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
        if(DistanceFromPlayer() <= range.chase)
        {
            if(DistanceFromPlayer() > range.atack)
            {
                ChangeAIState(AIState.Chase);
                return;
            }
        }

        // �U���N�[���^�C�����Ȃ��Ȃ�����
        curIdleTime += Time.deltaTime;
        if (curIdleTime > atackCoolTime)
        {
            ChangeAIState(AIState.Atack);
            canDamageAnim = false;
            curIdleTime = 0;
            return;
        }
    }

    private void Chase()
    {
        navMesh.destination = playerPos.position;

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
    }

    private void KnockBack()
    {
        if (AnimationEnd("KnockBack"))
        {
            rigidbody.isKinematic = true;
            canDamageAnim = true;

            // �v���C���[�Ƃ̋������ǐՔ͈͓��Ȃ�
            if (DistanceFromPlayer() <= range.chase)
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

    #region �U���p�^�[��

    private void Melee1()
    {

    }

    private void Melee2()
    {

    }

    private void Jump()
    {

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
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
                break;

            case AIState.Chase:
                navMesh.speed = speed.chase;
                navMesh.destination = playerPos.position;
                break;

            case AIState.Atack:
                // �U���������_���őI��
                AtackState atack = EnumGeneric.GetRandom<AtackState>();
                SetAtackState(atack);

                // �U���N�[���^�C���������_���Őݒ�
                atackCoolTime = Generic.RandomPointRange(dafaultAtackCoolTime, 0.5f);

                transform.LookAt(playerPos.position);
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
                break;

            case AIState.KnockBack:
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
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
                atackPower = Generic.RandomPointRange(-10.0f, 2.0f);
                break;

            case AtackState.Melee2:
                atackPower = Generic.RandomPointRange(-15.0f, 2.0f);
                break;

            case AtackState.Jump:
                atackPower = Generic.RandomPointRange(-20.0f, 3.0f);
                break;
        }
    }

    #endregion
}
