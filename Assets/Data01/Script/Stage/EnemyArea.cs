using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("�O��̃o���A")]
    [SerializeField] private GameObject[] barrier = new GameObject[2];

    [Header("�G�l�~�[")]
    [SerializeField] private List<SpownEnemy> enemys = new List<SpownEnemy>();

    [System.Serializable]
    public class SpownEnemy // �X�|�[������G�̏��
    {
        public GameObject enemyObj; // �G�l�~�[
        public int spownValue;   // �X�|�[����
    }


    //-----privateField--------------------------------------------------------------
    private SphereCollider sphereCollider;
    private bool inArea = false; // �G���A�ɓ�����������
    private bool isAreaClear = false; // �G���A�̃N���A����

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------


    #region �V�X�e��
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }


    void Update()
    {
        // �G���A�ɓ����Ă���ꍇ
        if (inArea)
        {
            // �G���A���S�Ă̓G���|���ꂽ��
            if(AreaManager.enemyList.Count <= 0)
            {
                isAreaClear = true;
            }
        }
        else { return; }

        // �G���A���N���A������
        if(isAreaClear)
        {
            // �G���A�����
            foreach(var _barrier in barrier)
            {
                _barrier.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (inArea) { return; }

        if (!other.gameObject.CompareTag("Player")) { return; }

        // �G���X�|�[��
        foreach (var _enemy in enemys)
        {
            for (int i = 0; i < _enemy.spownValue; i++)
            {
                AreaManager.enemyList.Add(Instantiate(_enemy.enemyObj, GetRandomPosInSphere(), Quaternion.identity,transform));
            }
        }

        // �G���A�𕕍�
        foreach(var _barrier in barrier)
        {
            _barrier.gameObject.SetActive(true);
        }

        inArea = true;
    }



    #endregion


    #region �@�\

    /// <summary>
    /// �~���̃����_���ȍ��W��Ԃ�
    /// </summary>
    /// <returns>���W</returns>
    public Vector3 GetRandomPosInSphere()
    {
        // SphereCollider�̒��S�ƒ��a�����߂�
        Vector3 center = sphereCollider.transform.position + sphereCollider.center;
        float radius = sphereCollider.radius * sphereCollider.transform.localScale.x;

        // �����ƒ��S����
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
        float radomDistance = UnityEngine.Random.Range(5, radius);

        Vector3 randomPosition = center + randomDirection * radomDistance;
        randomPosition.y = 0;
        return randomPosition;
    }
    #endregion
}
