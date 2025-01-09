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

    private enum AIState // 状態パターン
    {
        Idle,       // 待機
        Chase,      // 追跡
        Atack,      // 攻撃
        KnockBack   // ノックバック
    }
    AIState aiState = AIState.Idle;

    public enum AtackState // 攻撃パターン
    {
        Melee1,     // 近接1
        Melee2,     // 近接2
        Jump        // ジャンプ
    }
    AtackState atackState = AtackState.Melee1;



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
        if (AnimationEnd("KnockBack"))
        {
            rigidbody.isKinematic = true;
            canDamageAnim = true;

            // プレイヤーとの距離が追跡範囲内なら
            if (DistanceFromPlayer() <= range.chase)
            {   // かつ攻撃範囲内なら
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

    #region エネミーの制御

    public override void TakeParry()
    {
        base.TakeParry();

        rigidbody.isKinematic = false;
        ChangeAIState(AIState.KnockBack);
        transform.LookAt(playerPos);
    }

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

                // 攻撃クールタイムをランダムで設定
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

        //Debug.Log($"{_nextState}ステートに更新");
    }

    /// <summary>
    /// 攻撃ステートを設定
    /// </summary>
    /// <param name="_atack">攻撃</param>
    private void SetAtackState(AtackState _atack)
    {
        atackState = _atack;

        animator.SetInteger("AtackValue",(int)_atack + 1);
        // +1としているのはAnimatorの各遷移条件が1から始まるため

        // 攻撃ステートに移る時に1回だけ呼ばれる処理
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
