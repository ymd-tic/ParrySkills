using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static EnemyGolemCtrl;

public class EnemySkeletonCtrl : EnemyBase
{

    //-----SerializeField------------------------------------------------------------



    //-----privateField--------------------------------------------------------------
    private EnemyArea enemyArea;    // �X�|�[�������G���A
    private bool isGoalPoint;       // �ڕW�n�_�ɒ��������̃t���O
    private Coroutine coroutine;    // �R���[�`��

    private enum AIState // ��ԃp�^�[��
    {
        Idle,       // �ҋ@
        Wait,       // ���n��
        Patrol,     // ����
        Chase,      // �ǐ�
        Atack,      // �U��
        Damage,     // �_���[�W
        KnockBack,  // �m�b�N�o�b�N
        Retreat     // ���
    }
    AIState aiState = AIState.Patrol;

    private enum AtackState // �U���p�^�[��
    {
        Melee1,     // �ߐ�1
    }
    private AtackState atackState = AtackState.Melee1;

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------




    #region �V�X�e��

    protected override void Start()
    {
        base.Start();

        enemyArea = this.transform.parent.GetComponent<EnemyArea>();
        // �ړI�n���G���A���ɐݒ�
        navMesh.destination = enemyArea.GetRandomPosInSphere();
    }

    protected override void Update()
    {
        base.Update();

        switch (aiState)
        {
            case AIState.Idle:
                Idle();
                break;

            case AIState.Wait:
                Wait();
                break;

            case AIState.Patrol:
                Patrol();
                break;

            case AIState.Chase:
                Chase();
                break;

            case AIState.Atack:
                Atack();
                break;

            case AIState.Damage:
                Damage();
                break;

            case AIState.KnockBack:
                KnockBack();
                break;

            case AIState.Retreat:
                Retreat();
                break;
        }
    }

    #endregion


    #region ��ԃX�e�[�g

    private void Idle()
    {
        // �v���C���[�Ƃ̋������ǐՔ͈͊O�Ȃ�
        if (DistanceFromPlayer() > range.chase)
        {
            ChangeAIState(AIState.Patrol);
            return;
        }
        // �v���C���[�Ƃ̋������U���͈͊O�Ȃ�
        else if (DistanceFromPlayer() > range.atack)
        {
            ChangeAIState(AIState.Chase);
            return;
        }
        else if(DistanceFromPlayer() <= range.leave)
        {
            ChangeAIState(AIState.Retreat);
            return;
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

    private void Wait()
    {
        if (!isGoalPoint)
        {
            // ��莞�ԑ҂����̒n�_�Ɉړ�
            coroutine = StartCoroutine(SetNextPatrolPoint());
        }

        // �v���C���[�Ƃ̋������ǐՔ͈͖����Ȃ�
        if (DistanceFromPlayer() <= range.chase)
        {
            ChangeAIState(AIState.Chase);
            isGoalPoint = false;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            return;
        }
    }

    private void Patrol()
    {
        // �ړI�n�ɋ߂Â�����
        if (navMesh.remainingDistance < 2f)
        {
            ChangeAIState(AIState.Wait);
        }

        // �v���C���[�Ƃ̋������ǐՔ͈͓��Ȃ�
        if (DistanceFromPlayer() <= range.chase)
        {
            ChangeAIState(AIState.Chase);
            return;
        }
    }

    private void Chase()
    {
        navMesh.destination = playerPos.position;

        // �v���C���[�Ƃ̋������ǐՔ͈͊O�Ȃ�
        if (DistanceFromPlayer() > range.chase)
        {
            ChangeAIState(AIState.Patrol);
            return;
        }

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
        switch(atackState)
        {
            case AtackState.Melee1:
                Melee1();
                break;
        }

        // �U�����[�V�����̏I��
        if (AnimationEnd("Atack"))
        {
            // �v���C���[�Ƃ̋�����
            // �ǐՔ͈͊O
            if (DistanceFromPlayer() > range.chase)
            {
                ChangeAIState(AIState.Patrol);
            }
            // �U���͈͊O
            else if (DistanceFromPlayer() > range.atack)
            {
                ChangeAIState(AIState.Chase);
            }
            else if(DistanceFromPlayer() > range.leave)
            {
                ChangeAIState(AIState.Idle);
            }
            else if (DistanceFromPlayer() <= range.leave)
            {
                ChangeAIState(AIState.Retreat);
            }
            canDamageAnim = true;
        }
    }

    private void Damage()
    {
        curIdleTime += Time.deltaTime;
        if (curIdleTime > atackCoolTime)
        {
            ChangeAIState(AIState.Atack);
            canDamageAnim = false;
            curIdleTime = 0;
            rigidbody.isKinematic = true;
            return;
        }

        if (AnimationEnd("Damage"))
        {
            rigidbody.isKinematic = true;

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

    private void KnockBack()
    {
        if(AnimationEnd("KnockBack"))
        {
            rigidbody.isKinematic = true;
            canDamageAnim = true;

            // �v���C���[�Ƃ̋������ǐՔ͈͓��Ȃ�
            if (DistanceFromPlayer() <= range.chase)
            {   // ���U���͈͓��Ȃ�
                if (DistanceFromPlayer() <= range.atack)
                {
                    ChangeAIState(AIState.Idle);

                    if(DistanceFromPlayer() <= range.leave)
                    {
                        ChangeAIState(AIState.Retreat);
                    }

                    return;
                }

                ChangeAIState(AIState.Chase);
            }
            else
            {
                ChangeAIState(AIState.Patrol);
            }
        }
    }


    private void Retreat()
    {
        transform.LookAt(playerPos);
        // �v���C���[���痣�������Ɉړ�
        Vector3 directionAwayFromPlayer = (transform.position - playerPos.position).normalized;
        Vector3 retreatPosition = transform.position + directionAwayFromPlayer * range.leave;

        navMesh.destination = retreatPosition;

        // �U���N�[���^�C�����Ȃ��Ȃ�����

        curIdleTime += Time.deltaTime;
        if (curIdleTime > atackCoolTime)
        {
            ChangeAIState(AIState.Atack);
            canDamageAnim = false;
            curIdleTime = 0;
            return;
        }

        // ���̋������������Idle��ԂɑJ��
        if (DistanceFromPlayer() >= range.leave)
        {
            ChangeAIState(AIState.Idle);
        }
    }
    #endregion

    #region �U���p�^�[��

    private void Melee1()
    {

    }

    #endregion

    #region �G�l�~�[�̐���

    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    /// <param name="_damage">�_���[�W</param>
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        if (canDamageAnim)
        {
            rigidbody.isKinematic = false;
            ChangeAIState(AIState.Damage);
            transform.LookAt(playerPos);
        }
    }

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
        if(isDie) {return;}

        aiState = _nextState;   // �X�e�[�g�X�V

        animator.SetTrigger($"{_nextState}");   // �A�j���[�V�����X�V

        foreach (var animState in animator.parameters)
        {
            // �g���K�[�ȊO�̓X�L�b�v
            if (animState.type != AnimatorControllerParameterType.Trigger) { continue; }

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

            case AIState.Wait:
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
                break;

            case AIState.Patrol:
                navMesh.speed = speed.patrol;
                navMesh.destination = enemyArea.GetRandomPosInSphere();
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

            case AIState.Damage:
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
                break;

            case AIState.KnockBack:
                navMesh.speed = speed.zero;
                navMesh.destination = enemyPos.position;
                break;

            case AIState.Retreat:
                navMesh.speed = speed.patrol;
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

        animator.SetInteger("AtackValue", (int)_atack + 1);
        // +1�Ƃ��Ă���̂�Animator�̊e�J�ڏ�����1����n�܂邽��

        // �U���X�e�[�g�Ɉڂ鎞��1�񂾂��Ă΂�鏈��
        switch (_atack)
        {
            case AtackState.Melee1:
                atackPower = Generic.RandomPointRange(-10.0f, 2.0f);
                break;
        }
    }

    #endregion


    #region �R���[�`��

    /// <summary>
    /// ����n�_�ɒ��������莞�ԑҋ@���Ă��玟�̒n�_�����߂�
    /// </summary>
    /// <returns></returns>
    IEnumerator SetNextPatrolPoint()
    {
        float waitTime = 5.5f; // �ҋ@����
        isGoalPoint = true;
        yield return new WaitForSeconds(waitTime);
        ChangeAIState(AIState.Patrol);
        isGoalPoint = false;
    }

    #endregion
}