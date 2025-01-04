using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GolemAtackCollider : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    //[Header("通常攻撃1")][SerializeField] BoxCollider atackOneCollider;
    //[Header("通常攻撃2")][SerializeField] SphereCollider atackTwoCollider;
    //[Header("通常攻撃3")][SerializeField] SphereCollider atackThreeCollider;

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

    ///// <summary>
    ///// 現在の攻撃パターンによって攻撃判定のON・OFF
    ///// </summary>
    ///// <param name="isEnabled">ON = true, OFF = false</param>
    //void ColliderOnOff(bool isEnabled)
    //{
    //    switch (animator.GetInteger("AtackValue"))
    //    {
    //        case 1:
    //            atackOneCollider.enabled = isEnabled;
    //            break;

    //        case 2:
    //            atackTwoCollider.enabled = isEnabled;
    //            break;

    //        case 3:
    //            atackThreeCollider.enabled = isEnabled;
    //            break;
    //    }
    //}

    ///// <summary>
    ///// 判定を全てOFF
    ///// </summary>
    //void AtackColliderResset()
    //{
    //    atackOneCollider.enabled = false;
    //    atackTwoCollider.enabled = false;
    //    atackThreeCollider.enabled = false;
    //}

    #endregion

}
