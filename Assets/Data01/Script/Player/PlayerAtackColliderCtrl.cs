using UnityEngine;

public class PlayerAtackColliderCtrl : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("攻撃判定")][SerializeField] BoxCollider atackCollider;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region コライダー制御

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
