using System;
using System.Collections;
using System.Collections.Generic;
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

    private enum AtackState // �U���p�^�[��
    {
        Melee1,     // �ߐ�1
        Melee2,     // �ߐ�2
        Jump        // �W�����v
    }
    AtackState atackStae = AtackState.Melee1;

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
        switch (atackStae)
        {
            case AtackState.Melee1:
                break;

            case AtackState.Melee2:
                break;

            case AtackState.Jump:
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

                transform.LookAt(playerPos.position);
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
                break;

            case AIState.KnockBack:
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
                break;
        }

        Debug.Log($"{_nextState}�X�e�[�g�ɍX�V");
    }

    /// <summary>
    /// �U���X�e�[�g�����߂�
    /// </summary>
    /// <param name="_atackState"></param>
    private void SetAtackState(AtackState _atackState)
    {
        atackStae = _atackState;

        animator.SetInteger("AtackValue",(int)_atackState + 1);
        // +1�Ƃ��Ă���̂�Animator�̊e�J�ڏ�����1����n�܂邽��

        // �U���X�e�[�g�Ɉڂ鎞��1�񂾂��Ă΂�鏈��
        switch (_atackState)
        {
            case AtackState.Melee1:
                break;

            case AtackState.Melee2:
                break;

            case AtackState.Jump:
                break;
        }
    }
}
