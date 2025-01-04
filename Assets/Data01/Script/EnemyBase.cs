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
    [Header("最大Hp")]          [SerializeField] private   float maxHp = 100;
    [Header("攻撃クールタイム")][SerializeField] protected float atackCoolTime = 3;
    [Header("ダメ―ジ表記")]    [SerializeField] private GameObject damageTextPrefab;
    [Header("パリィ可能エフェクト")][SerializeField] private ParticleSystem parryEfect;

    [Header("行動パターン範囲")][SerializeField] protected Range range;
    [System.Serializable] protected struct Range { public float chase, atack; }

    [Header("巡回速度")][SerializeField] protected Speed speed;
    [System.Serializable] protected struct Speed { public float patrol, chase, zero; }

    //-----privateField--------------------------------------------------------------
    private Generic.ParamateValue hpValue;
    private CapsuleCollider capsuleCollider;
    private TMP_Text damageText;

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------
    protected Transform playerPos;  // プレイヤー座標
    protected Transform enemyPos;   // 敵座標
    protected bool canDamageAnim = true;   // ダメージアニメーションを再生するか
    protected NavMeshAgent navMesh;
    protected Animator animator;
    protected new Rigidbody rigidbody;
    protected float curIdleTime = 0;  // 待機時間
    protected bool isDie = false;


    #region システム

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
        if (AnimationEnd("Die")) // 死ぬアニメーションが終わったら消す
        {
            Debug.Log($"{gameObject.name}を倒した");
            Destroy(this.gameObject);
        }

        if (isDie) { return; }  // 死んだら何もしない
    }

    /// <summary>
    /// プレイヤーとの距離を返す
    /// </summary>
    /// <returns>距離</returns>
    protected float DistanceFromPlayer()
    {
        return Vector3.Distance(playerPos.position, enemyPos.position);
    }
    #endregion


    #region エネミーの制御

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="_damage">ダメージ</param>
    public virtual void TakeDamage(int _damage)
    {
        if (isDie) { return; }

        hpValue.cur += _damage;

        // 受けたダメージを反映
        damageText.text = $"{Mathf.Abs(_damage)}";
        // UIのポップアップ位置
        Vector3 popTextPos = new Vector3(enemyPos.position.x, 2.5f, enemyPos.position.z);
        // ダメージUI生成
        Instantiate(damageTextPrefab, popTextPos, Quaternion.identity);

        if (hpValue.cur <= hpValue.min)
        {
            Die();
        }
    }

    /// <summary>
    /// パリィされる
    /// </summary>
    public virtual void TakeParry()
    {
        if(isDie) { return; }

    }

    /// <summary>
    /// HPが0になったら
    /// </summary>
    private void Die()
    {
        isDie = true;
        animator.SetTrigger("Die");
        AreaManager.enemyList.Remove(this.gameObject);
        capsuleCollider.enabled = false;
    }

    #endregion


    #region アニメーションEvent

    /// <summary>
    /// パリィエフェクトを出す
    /// </summary>
    private void PlayParryEfect()
    {
        parryEfect.Play();
    }

    /// <summary>
    /// アニメーションの終了
    /// </summary>
    /// <param name="_animName">ステート名</param>
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
