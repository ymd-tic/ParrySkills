using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitManager : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------


    //-----privateField--------------------------------------------------------------
    private PlayerCtrl playerCtrl;


    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region システム

    private void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (!this.enabled) { return; }
        if (!_other.gameObject.CompareTag("EnemyAtack")) { return; }
        StartCoroutine(ExeDamage(_other));
    }

    private void OnControllerColliderHit(ControllerColliderHit _hit)
    {
        if (!this.enabled) { return; }

        if (_hit.collider.isTrigger)
        {
            OnTriggerEnter(_hit.collider);
        }
    }
    #endregion

    /// <summary>
    /// パリィ・ヒット判定の実行順用
    /// </summary>
    IEnumerator ExeDamage(Collider _other)
    {
        yield return new WaitForSecondsRealtime(0.02f);

        if (!ParrySystem.parrySuccess)
        {
            EnemyBase enemy = _other.GetComponent<EnemyAtack>().enemy;
            playerCtrl.TakeDamage(enemy.atackPower);
        }
    }
}


