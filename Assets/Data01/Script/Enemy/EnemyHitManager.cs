using UnityEngine;
using System.Collections;
public class EnemyHitManager : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------



    //-----privateField--------------------------------------------------------------
    private EnemyBase enemy;
    private bool canDamage = true; // ダメージを食らうか


    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region システム

    void Start()
    {
        enemy = GetComponent<EnemyBase>();
    }

    private new Collider collider = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtack") && canDamage)
        {
            collider = other;
            Invoke("ExecutionOrder", 0.04f);
        }
    }

    /// <summary>
    /// パリィ・ヒット判定の実行順用
    /// </summary>
    private void ExecutionOrder()
    {
        if (ParrySystem.parrySuccess) { return; }

        PlayerCtrl playerCtrl = collider.transform.root.GetComponent<PlayerCtrl>();

        collider.transform.root.GetComponent<SkillCtrl>().AdrenalineGaugeCalculation(3);
        enemy.TakeDamage(-playerCtrl.atackPower);
        StartCoroutine(CanDamage());
    }

    #endregion


    #region コルーチン

    /// <summary>
    /// 一定時間無敵
    /// </summary>
    /// <returns></returns>
    IEnumerator CanDamage()
    {
        float canDamageTime = 0.2f; // 無敵時間

        canDamage = false;
        yield return new WaitForSeconds(canDamageTime);
        canDamage = true;
    }

    #endregion
}
