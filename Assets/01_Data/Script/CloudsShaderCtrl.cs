using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public Material cloudMaterial;

    void Update()
    {
        // Time.unscaledTimeをシェーダーに渡す
        cloudMaterial.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}