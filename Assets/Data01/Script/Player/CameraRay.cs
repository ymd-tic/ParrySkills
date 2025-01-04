using System.Collections.Generic;
using UnityEngine;
using System.Linq;  // Except


public class CameraRay : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    [Header("被写体")][SerializeField] private Transform subject;
    [Header("被写体のレイヤー")][SerializeField] private List<string> coverLayerNameList;

    //-----privateField--------------------------------------------------------------
    private int layerMask;


    //-----publicField---------------------------------------------------------------
    public List<Renderer> rendererHitsList = new List<Renderer>();
    public Renderer[] rendererHitsPrevs;



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region システム

    void Start()
    {
        // 遮蔽物のレイヤーマスクを、レイヤー名のリストから合成する。
        layerMask = 0;
        foreach (string layerName in coverLayerNameList)
        {
            layerMask |= 1 << LayerMask.NameToLayer(layerName);
        }

    }

    void Update()
    {
        // カメラと被写体を結ぶ ray を作成
        Vector3 difference = (subject.transform.position - this.transform.position);
        Vector3 direction = difference.normalized;
        Ray ray = new Ray(transform.position, direction);

        // 前回の結果を退避してから、Raycast して今回の遮蔽物のリストを取得する
        RaycastHit[] hits = Physics.RaycastAll(ray, difference.magnitude, layerMask);


        rendererHitsPrevs = rendererHitsList.ToArray();
        rendererHitsList.Clear();
        // 遮蔽物は一時的にすべて描画機能を無効にする。
        foreach (RaycastHit hit in hits)
        {
            // 遮蔽物が被写体の場合は例外とする
            if (hit.collider.gameObject == subject)
            {
                continue;
            }

            // 遮蔽物の Renderer コンポーネントを無効にする
            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                rendererHitsList.Add(renderer);
                renderer.enabled = false;
            }
        }

        // 前回まで対象で、今回対象でなくなったものは、表示を元に戻す。
        foreach (Renderer renderer in rendererHitsPrevs.Except<Renderer>(rendererHitsList))
        {
            // 遮蔽物でなくなった Renderer コンポーネントを有効にする
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }

    }
}

#endregion

