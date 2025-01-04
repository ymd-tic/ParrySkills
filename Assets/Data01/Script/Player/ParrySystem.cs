using System.Collections;
using UnityEngine;

public class ParrySystem : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("パリィ成功エフェクト")][SerializeField] ParticleSystem parryEfect;


    //-----privateField--------------------------------------------------------------
    private PlayerCtrl playerCtrl;


    //-----publicField---------------------------------------------------------------
    public static bool parrySuccess = false;    // パリィ成功したかフラグ


    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region システム

    private void Start()
    {
        playerCtrl = this.transform.parent.GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ヒットしたタグがEnemyAtackか検知
        if (!other.CompareTag("EnemyAtack")) { return; }
        if (parrySuccess) {return; }

        CompreteParry(other);
    }

    #endregion


    #region パリィ制御

    /// <summary>
    /// パリィの成功
    /// </summary>
    /// <param name="_other">ヒットした当たり判定</param>
    private void CompreteParry(Collider _other)
    {
        parrySuccess = true;
        Debug.Log("パリィ成功");
        // アドレナリンゲージを増やす
        this.transform.parent.GetComponent<SkillCtrl>().AdrenalineGaugeCalculation(15f);
        // 敵をノックバックさせる
        _other.GetComponent<EnemyAtack>().enemy.TakeParry();

        // エフェクト生成
        Vector3 efectPos = this.transform.position;
        efectPos.y = 1.5f;
        Instantiate(parryEfect, efectPos, Quaternion.identity);

        StartCoroutine(ReseetParryFlag());
    }

    #endregion

    /// <summary>
    /// パリィフラグのリセット
    /// </summary>
    /// <returns></returns>
    IEnumerator ReseetParryFlag()
    {
        yield return new WaitForSeconds(0.2f);
        parrySuccess = false;
    }
}
