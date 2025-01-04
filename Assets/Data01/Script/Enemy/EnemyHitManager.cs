using UnityEngine;
using System.Collections;
public class EnemyHitManager : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------



    //-----privateField--------------------------------------------------------------
    private EnemyBase enemy;
    private bool canDamage = true; // �_���[�W��H�炤��


    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �V�X�e��

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
    /// �p���B�E�q�b�g����̎��s���p
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


    #region �R���[�`��

    /// <summary>
    /// ��莞�Ԗ��G
    /// </summary>
    /// <returns></returns>
    IEnumerator CanDamage()
    {
        float canDamageTime = 0.2f; // ���G����

        canDamage = false;
        yield return new WaitForSeconds(canDamageTime);
        canDamage = true;
    }

    #endregion
}
