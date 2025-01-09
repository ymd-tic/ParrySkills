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
    private EnemyArea enemyArea;    // スポーンしたエリア
    private bool isGoalPoint;       // 目標地点に着いたかのフラグ
    private Coroutine coroutine;    // コルーチン

    private enum AIState // 状態パターン
    {
        Idle,       // 待機
        Wait,       // 見渡す
        Patrol,     // 巡回
        Chase,      // 追跡
        Atack,      // 攻撃
        Damage,     // ダメージ
        KnockBack,  // ノックバック
        Retreat     // 後退
    }
    AIState aiState = AIState.Patrol;

    private enum AtackState // 攻撃パターン
    {
        Melee1,     // 近接1
    }
    private AtackState atackState = AtackState.Melee1;

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------




    #region システム

    protected override void Start()
    {
        base.Start();

        enemyArea = this.transform.parent.GetComponent<EnemyArea>();
        // 目的地をエリア内に設定
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


    #region 状態ステート

    private void Idle()
    {
        // プレイヤーとの距離が追跡範囲外なら
        if (DistanceFromPlayer() > range.chase)
        {
            ChangeAIState(AIState.Patrol);
            return;
        }
        // プレイヤーとの距離が攻撃範囲外なら
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

    private void Wait()
    {
        if (!isGoalPoint)
        {
            // 一定時間待つ→次の地点に移動
            coroutine = StartCoroutine(SetNextPatrolPoint());
        }

        // プレイヤーとの距離が追跡範囲無内なら
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
        // 目的地に近づいたら
        if (navMesh.remainingDistance < 2f)
        {
            ChangeAIState(AIState.Wait);
        }

        // プレイヤーとの距離が追跡範囲内なら
        if (DistanceFromPlayer() <= range.chase)
        {
            ChangeAIState(AIState.Chase);
            return;
        }
    }

    private void Chase()
    {
        navMesh.destination = playerPos.position;

        // プレイヤーとの距離が追跡範囲外なら
        if (DistanceFromPlayer() > range.chase)
        {
            ChangeAIState(AIState.Patrol);
            return;
        }

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
        switch(atackState)
        {
            case AtackState.Melee1:
                Melee1();
                break;
        }

        // 攻撃モーションの終了
        if (AnimationEnd("Atack"))
        {
            // プレイヤーとの距離が
            // 追跡範囲外
            if (DistanceFromPlayer() > range.chase)
            {
                ChangeAIState(AIState.Patrol);
            }
            // 攻撃範囲外
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

    private void KnockBack()
    {
        if(AnimationEnd("KnockBack"))
        {
            rigidbody.isKinematic = true;
            canDamageAnim = true;

            // プレイヤーとの距離が追跡範囲内なら
            if (DistanceFromPlayer() <= range.chase)
            {   // かつ攻撃範囲内なら
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
        // プレイヤーから離れる方向に移動
        Vector3 directionAwayFromPlayer = (transform.position - playerPos.position).normalized;
        Vector3 retreatPosition = transform.position + directionAwayFromPlayer * range.leave;

        navMesh.destination = retreatPosition;

        // 攻撃クールタイムがなくなったら

        curIdleTime += Time.deltaTime;
        if (curIdleTime > atackCoolTime)
        {
            ChangeAIState(AIState.Atack);
            canDamageAnim = false;
            curIdleTime = 0;
            return;
        }

        // 一定の距離を取ったらIdle状態に遷移
        if (DistanceFromPlayer() >= range.leave)
        {
            ChangeAIState(AIState.Idle);
        }
    }
    #endregion

    #region 攻撃パターン

    private void Melee1()
    {

    }

    #endregion

    #region エネミーの制御

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="_damage">ダメージ</param>
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
    /// 状態ステートを変える
    /// ステート毎のアニメーションを再生
    /// </summary>
    /// <param name="_nextState">次のステート</param>
    private void ChangeAIState(AIState _nextState)
    {
        if(isDie) {return;}

        aiState = _nextState;   // ステート更新

        animator.SetTrigger($"{_nextState}");   // アニメーション更新

        foreach (var animState in animator.parameters)
        {
            // トリガー以外はスキップ
            if (animState.type != AnimatorControllerParameterType.Trigger) { continue; }

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
                // 攻撃をランダムで選択
                AtackState atack = EnumGeneric.GetRandom<AtackState>();
                SetAtackState(atack);

                // 攻撃クールタイムをランダムで設定
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

        //Debug.Log($"{_nextState}ステートに更新");
    }

    /// <summary>
    /// 攻撃ステートを設定
    /// </summary>
    /// <param name="_atack">攻撃</param>
    private void SetAtackState(AtackState _atack)
    {
        atackState = _atack;

        animator.SetInteger("AtackValue", (int)_atack + 1);
        // +1としているのはAnimatorの各遷移条件が1から始まるため

        // 攻撃ステートに移る時に1回だけ呼ばれる処理
        switch (_atack)
        {
            case AtackState.Melee1:
                atackPower = Generic.RandomPointRange(-10.0f, 2.0f);
                break;
        }
    }

    #endregion


    #region コルーチン

    /// <summary>
    /// 巡回地点に着いたら一定時間待機してから次の地点を決める
    /// </summary>
    /// <returns></returns>
    IEnumerator SetNextPatrolPoint()
    {
        float waitTime = 5.5f; // 待機時間
        isGoalPoint = true;
        yield return new WaitForSeconds(waitTime);
        ChangeAIState(AIState.Patrol);
        isGoalPoint = false;
    }

    #endregion
}