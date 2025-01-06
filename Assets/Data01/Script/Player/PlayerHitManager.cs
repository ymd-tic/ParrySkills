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

    #region �V�X�e��

    private void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.enabled) { return; }
        if (!other.gameObject.CompareTag("EnemyAtack")) { return; }
        StartCoroutine(ExeDamage());
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
    IEnumerator ExeDamage()
    {
        yield return new WaitForSecondsRealtime(0.02f);

        if (!ParrySystem.parrySuccess)
        {
            playerCtrl.TakeDamage(-15f);
        }
    }
}


