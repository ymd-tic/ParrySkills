using UnityEngine;

public class SkeletonAtackCollider : MonoBehaviour
{

    //-----SerializeField------------------------------------------------------------
    [Header("通常攻撃")][SerializeField] BoxCollider boxCollider;


    //-----privateField--------------------------------------------------------------



    //-----publicField---------------------------------------------------------------



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region アニメーションEvent

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
