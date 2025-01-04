using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GolemAtackCollider : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    //[Header("�ʏ�U��1")][SerializeField] BoxCollider atackOneCollider;
    //[Header("�ʏ�U��2")][SerializeField] SphereCollider atackTwoCollider;
    //[Header("�ʏ�U��3")][SerializeField] SphereCollider atackThreeCollider;

    [Header("�U�������蔻��")]
    [SerializeField] new List<ColliderList> collider = new List<ColliderList>();

    [System.Serializable]
    public class ColliderList
    {
        [Header("�R���C�_�[")]
        public List<Collider> colliders = new List<Collider>();
    }
    

    //-----privateField--------------------------------------------------------------
    Animator animator;



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �V�X�e��

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    #endregion


    #region �A�j���[�V����Event

    ///// <summary>
    ///// ���݂̍U���p�^�[���ɂ���čU�������ON�EOFF
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
    ///// �����S��OFF
    ///// </summary>
    //void AtackColliderResset()
    //{
    //    atackOneCollider.enabled = false;
    //    atackTwoCollider.enabled = false;
    //    atackThreeCollider.enabled = false;
    //}

    #endregion

}
