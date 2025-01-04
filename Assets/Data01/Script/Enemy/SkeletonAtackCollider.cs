using UnityEngine;

public class SkeletonAtackCollider : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("通常攻撃")][SerializeField] BoxCollider boxCollider;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region コライダー制御

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
