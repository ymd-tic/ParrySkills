using UnityEngine;

public class SkeletonAtackCollider : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("�ʏ�U��")][SerializeField] BoxCollider boxCollider;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �R���C�_�[����

    public void ColliderOn()
    {
        boxCollider.enabled = true;
    }

    public void ColliderOff()
    {
        boxCollider.enabled = false;

    }

    #endregion
}
