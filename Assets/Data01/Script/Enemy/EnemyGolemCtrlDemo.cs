//using UnityEngine;
//using UnityEngine.UI;

//public class EnemyGolemCtrlDemo : EnemyBase
//{

//    //-----SerializeField------------------------------------------------------------
//    [Header("HPゲージ")][SerializeField] Slider hpGage;


//    //-----privateField--------------------------------------------------------------
//    private bool isFindPlayer = false;  // プレイヤーを見つけたか


//    //-----publicField---------------------------------------------------------------



//    //-----staticField---------------------------------------------------------------



//    //-----protectedField------------------------------------------------------------



//    enum AIState
//    {
//        Idle,
//        Chase,
//        Atack
//    }
//    AIState aiState = AIState.Idle;

//    enum AtackState
//    {
//        MeleeOne,
//        MeleeTwo,
//        Jump
//    }
//    AtackState atackState = AtackState.MeleeOne;

//    protected override void Start()
//    {
//        hpGage.value = hpValue.max;
//        base.Start();
//        isAnimCanDamage = false;
//    }

//    protected override void Update()
//    {
//        base.Update();

//        switch (aiState)
//        {
//            case AIState.Idle:
//                Idle();
//                break;

//            case AIState.Chase:
//                Chase();
//                break;

//            case AIState.Atack:
//                Atack();
//                break;
//        }
//    }

//    /// <summary>
//    /// その場に待機
//    /// </summary>
//    private void Idle()
//    {
//        agent.destination = enemyPos.position;

//        //プレイヤーとの距離が追跡範囲内かつ攻撃範囲外
//        if(Vector3.Distance(enemyPos.position,playerPos.position ) <= chaseRange&&
//            Vector3.Distance(enemyPos.position, playerPos.position) > atackRange)
//        {
//            isFindPlayer = true;
//            animator.SetBool("Idle", false);
//            animator.SetBool("Chase", true);
//            aiState = AIState.Chase;
//        }

//        if(!isFindPlayer) {return;}

//        nextAtackTime += Time.deltaTime;
//        if (nextAtackTime > atackCoolTIme)
//        {
//            nextAtackTime = 0;
//            transform.LookAt(playerPos.position);
//            AtackSet();
//        }
//    }

//    /// <summary>
//    /// プレイヤーを追跡
//    /// </summary>
//    private void Chase()
//    {
//        agent.speed = 4.0f;
//        agent.destination = playerPos.position;

//        // プレイヤーが攻撃範囲に入ったらステートをIdleにする
//        if ( Vector3.Distance(enemyPos.position,playerPos.position) <= atackRange)
//        {
//            animator.SetBool("Idle", true);
//            aiState = AIState.Idle;
//            return;
//        }
//    }

//    private void AtackSet()
//    {
//        int nextAtack = Random.Range(1, 4);
//        animator.SetInteger("Atack", nextAtack);
//        atackState = (AtackState)nextAtack - 1;
//        aiState = AIState.Atack;
//    }

//    /// <summary>
//    /// 攻撃パターン
//    /// </summary>
//    private void Atack()
//    {
//        if (agent.enabled)
//        {
//            agent.destination = enemyPos.position;
//        }

//        switch (atackState)
//        {
//            case AtackState.MeleeOne:
//                MeleeOne();
//                break;

//            case AtackState.MeleeTwo:
//                MeleeTwo();
//                break;

//            case AtackState.Jump:
//                Jump();
//                break;
//        }

//        if (isAnimAtackEnd)
//        {
//            isAnimAtackEnd = false;
//            animator.SetInteger("Atack",0);
//            aiState = AIState.Idle;
//        }
//    }

//    /// <summary>
//    /// 各攻撃
//    /// </summary>
//    private void MeleeOne()
//    {
//    }

//    private void MeleeTwo()
//    {
//    }
//    private void Jump()
//    {
//    }

//    public override void TakeDamage(int _damage)
//    {
//        base.TakeDamage(_damage);
//        StartCoroutine(new Generic.CalcuRation().ValueFluctuation(_damage,hpGage,hpValue));
//    }

//    protected override void KnockBackEnd()
//    {
//        base.KnockBackEnd();
//        isAnimAtackEnd = false;
//        animator.SetBool("Idle", true);
//        aiState = AIState.Idle;
//    }

//    protected override void Die()
//    {
//        base.Die();
//        MainGameSceneConttroller.GameFinish("Clear");
//    }
//}