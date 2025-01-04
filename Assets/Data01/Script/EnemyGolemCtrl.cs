using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGolemCtrl : EnemyBase
{

    //-----SerializeField------------------------------------------------------------



    //-----privateField--------------------------------------------------------------
    private enum AIState // 状態パターン
    {
        Idle,       // 待機
        Chase,      // 追跡
        Atack,      // 攻撃
        KnockBack   // ノックバック
    }
    AIState aiState = AIState.Idle;

    private enum AtackState // 攻撃パターン
    {
        Melee1,     // 近接1
        Melee2,     // 近接2
        Jump        // ジャンプ
    }
    AtackState atackStae = AtackState.Melee1;

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region システム

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

    #region 状態ステート

    private void Idle()
    {
        // プレイヤーとの距離が追跡範囲内なら
        if(DistanceFromPlayer() <= range.chase)
        {
            if(DistanceFromPlayer() > range.atack)
            {
                ChangeAIState(AIState.Chase);
                return;
            }
        }

        // 攻撃クールタイムがなくなったら
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

        // プレイヤーとの距離が攻撃範囲内なら
        if (DistanceFromPlayer() <= range.atack)
        {
            ChangeAIState(AIState.Idle);
            return;
        }
    }

    private void Atack()
    {
        // 攻撃分岐
        switch (atackStae)
        {
            case AtackState.Melee1:
                break;

            case AtackState.Melee2:
                break;

            case AtackState.Jump:
                break;
        }

        // 攻撃モーションの終了
        if (AnimationEnd("Atack"))
        {
            // プレイヤーとの距離が攻撃範囲外
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

    #region 攻撃パターン

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
    /// 状態ステートを変える
    /// ステート毎のアニメーションを再生
    /// </summary>
    /// <param name="_nextState">次のステート</param>
    private void ChangeAIState(AIState _nextState)
    {
        if (isDie) { return; }

        // ステート更新
        aiState = _nextState;

        // アニメーション更新
        animator.SetTrigger($"{_nextState}");   

        foreach (var animState in animator.parameters)
        {
            if(animState.type != AnimatorControllerParameterType.Trigger) {  continue; }

            if (animState.name != $"{_nextState}")
            {
                animator.ResetTrigger($"{animState.name}");
            }
        }

        // 次のステートに移る時に1回だけ呼ばれる処理
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
                // 攻撃をランダムで選択
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

        Debug.Log($"{_nextState}ステートに更新");
    }

    /// <summary>
    /// 攻撃ステートを決める
    /// </summary>
    /// <param name="_atackState"></param>
    private void SetAtackState(AtackState _atackState)
    {
        atackStae = _atackState;

        animator.SetInteger("AtackValue",(int)_atackState + 1);
        // +1としているのはAnimatorの各遷移条件が1から始まるため

        // 攻撃ステートに移る時に1回だけ呼ばれる処理
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
