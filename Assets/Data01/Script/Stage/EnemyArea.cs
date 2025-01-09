using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("前後のバリア")]
    [SerializeField] private GameObject[] barrier = new GameObject[2];

    [Header("エネミー")]
    [SerializeField] private List<SpownEnemy> enemys = new List<SpownEnemy>();

    [System.Serializable]
    public class SpownEnemy // スポーンする敵の情報
    {
        public GameObject enemyObj; // エネミー
        public int spownValue;   // スポーン数
    }


    //-----privateField--------------------------------------------------------------
    private SphereCollider sphereCollider;
    private bool inArea = false; // エリアに入ったか判定
    private bool isAreaClear = false; // エリアのクリア判定

    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------


    #region システム
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }


    void Update()
    {
        // エリアに入っている場合
        if (inArea)
        {
            // エリア内全ての敵が倒されたら
            if(AreaManager.enemyList.Count <= 0)
            {
                isAreaClear = true;
            }
        }
        else { return; }

        // エリアをクリアしたら
        if(isAreaClear)
        {
            // エリアを解放
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

        // 敵をスポーン
        foreach (var _enemy in enemys)
        {
            for (int i = 0; i < _enemy.spownValue; i++)
            {
                AreaManager.enemyList.Add(Instantiate(_enemy.enemyObj, GetRandomPosInSphere(), Quaternion.identity,transform));
            }
        }

        // エリアを封鎖
        foreach(var _barrier in barrier)
        {
            _barrier.gameObject.SetActive(true);
        }

        inArea = true;
    }



    #endregion


    #region 機能

    /// <summary>
    /// 円内のランダムな座標を返す
    /// </summary>
    /// <returns>座標</returns>
    public Vector3 GetRandomPosInSphere()
    {
        // SphereColliderの中心と直径を求める
        Vector3 center = sphereCollider.transform.position + sphereCollider.center;
        float radius = sphereCollider.radius * sphereCollider.transform.localScale.x;

        // 向きと中心から
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
        float radomDistance = UnityEngine.Random.Range(5, radius);

        Vector3 randomPosition = center + randomDirection * radomDistance;
        randomPosition.y = 0;
        return randomPosition;
    }
    #endregion
}
