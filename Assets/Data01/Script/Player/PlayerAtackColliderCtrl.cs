using UnityEngine;

public class PlayerAtackColliderCtrl : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("�U������")][SerializeField] BoxCollider atackCollider;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �R���C�_�[����

    public void ColliderOn()
    {
        atackCollider.enabled = true;
    }

    public void ColliderOff()
    {
        atackCollider.enabled = false;

    }

    #endregion
}
