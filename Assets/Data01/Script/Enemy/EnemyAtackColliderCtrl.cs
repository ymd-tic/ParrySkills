using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAtackColliderCtrl : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------


    [Header("攻撃当たり判定")]
    [SerializeField] new List<ColliderList> collider = new List<ColliderList>();

    [System.Serializable]
    public class ColliderList
    {
        [Header("コライダー")]
        public List<Collider> colliders = new List<Collider>();
    }
    

    //-----privateField--------------------------------------------------------------
    Animator animator;



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region システム

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    #endregion


    #region アニメーションEvent

    public void SetColliderOn()
    {
        int atackValue = animator.GetInteger("AtackValue");

        foreach (var col in collider[atackValue-1].colliders)
        {
            col.enabled = true;
        }
    }

    public void SetColliderOff()
    {
        int atackValue = animator.GetInteger("AtackValue");

        foreach (var col in collider[atackValue - 1].colliders)
        {
            col.enabled = false;
        }
    }
    #endregion

}
