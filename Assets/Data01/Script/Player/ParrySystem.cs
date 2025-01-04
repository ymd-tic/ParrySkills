using System.Collections;
using UnityEngine;

public class ParrySystem : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("�p���B�����G�t�F�N�g")][SerializeField] ParticleSystem parryEfect;


    //-----privateField--------------------------------------------------------------
    private PlayerCtrl playerCtrl;


    //-----publicField---------------------------------------------------------------
    public static bool parrySuccess = false;    // �p���B�����������t���O


    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �V�X�e��

    private void Start()
    {
        playerCtrl = this.transform.parent.GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �q�b�g�����^�O��EnemyAtack�����m
        if (!other.CompareTag("EnemyAtack")) { return; }
        if (parrySuccess) {return; }

        CompreteParry(other);
    }

    #endregion


    #region �p���B����

    /// <summary>
    /// �p���B�̐���
    /// </summary>
    /// <param name="_other">�q�b�g���������蔻��</param>
    private void CompreteParry(Collider _other)
    {
        parrySuccess = true;
        Debug.Log("�p���B����");
        // �A�h���i�����Q�[�W�𑝂₷
        this.transform.parent.GetComponent<SkillCtrl>().AdrenalineGaugeCalculation(15f);
        // �G���m�b�N�o�b�N������
        _other.GetComponent<EnemyAtack>().enemy.TakeParry();

        // �G�t�F�N�g����
        Vector3 efectPos = this.transform.position;
        efectPos.y = 1.5f;
        Instantiate(parryEfect, efectPos, Quaternion.identity);

        StartCoroutine(ReseetParryFlag());
    }

    #endregion

    /// <summary>
    /// �p���B�t���O�̃��Z�b�g
    /// </summary>
    /// <returns></returns>
    IEnumerator ReseetParryFlag()
    {
        yield return new WaitForSeconds(0.2f);
        parrySuccess = false;
    }
}
