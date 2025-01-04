using System.Collections;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!this.enabled) { return; }
        if (!other.gameObject.CompareTag("EnemyAtack")) { return; }
        Invoke("ExecutionOrder", 0.02f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!this.enabled) { return; }

        if (hit.collider.isTrigger)
        {
            OnTriggerEnter(hit.collider);
        }
    }
    #endregion

    /// <summary>
    /// パリィ・ヒット判定の実行順用
    /// </summary>
    private void ExecutionOrder()
    {
        if (ParrySystem.parrySuccess) { return; }

        playerCtrl.TakeDamage(-15f);
    }
}


