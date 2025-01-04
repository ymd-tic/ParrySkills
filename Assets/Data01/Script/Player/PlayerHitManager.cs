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

    #region �V�X�e��

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
    /// �p���B�E�q�b�g����̎��s���p
    /// </summary>
    private void ExecutionOrder()
    {
        if (ParrySystem.parrySuccess) { return; }

        playerCtrl.TakeDamage(-15f);
    }
}


