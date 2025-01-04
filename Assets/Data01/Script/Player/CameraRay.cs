using System.Collections.Generic;
using UnityEngine;
using System.Linq;  // Except


public class CameraRay : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    [Header("��ʑ�")][SerializeField] private Transform subject;
    [Header("��ʑ̂̃��C���[")][SerializeField] private List<string> coverLayerNameList;

    //-----privateField--------------------------------------------------------------
    private int layerMask;


    //-----publicField---------------------------------------------------------------
    public List<Renderer> rendererHitsList = new List<Renderer>();
    public Renderer[] rendererHitsPrevs;



    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �V�X�e��

    void Start()
    {
        // �Օ����̃��C���[�}�X�N���A���C���[���̃��X�g���獇������B
        layerMask = 0;
        foreach (string layerName in coverLayerNameList)
        {
            layerMask |= 1 << LayerMask.NameToLayer(layerName);
        }

    }

    void Update()
    {
        // �J�����Ɣ�ʑ̂����� ray ���쐬
        Vector3 difference = (subject.transform.position - this.transform.position);
        Vector3 direction = difference.normalized;
        Ray ray = new Ray(transform.position, direction);

        // �O��̌��ʂ�ޔ����Ă���ARaycast ���č���̎Օ����̃��X�g���擾����
        RaycastHit[] hits = Physics.RaycastAll(ray, difference.magnitude, layerMask);


        rendererHitsPrevs = rendererHitsList.ToArray();
        rendererHitsList.Clear();
        // �Օ����͈ꎞ�I�ɂ��ׂĕ`��@�\�𖳌��ɂ���B
        foreach (RaycastHit hit in hits)
        {
            // �Օ�������ʑ̂̏ꍇ�͗�O�Ƃ���
            if (hit.collider.gameObject == subject)
            {
                continue;
            }

            // �Օ����� Renderer �R���|�[�l���g�𖳌��ɂ���
            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                rendererHitsList.Add(renderer);
                renderer.enabled = false;
            }
        }

        // �O��܂őΏۂŁA����ΏۂłȂ��Ȃ������̂́A�\�������ɖ߂��B
        foreach (Renderer renderer in rendererHitsPrevs.Except<Renderer>(rendererHitsList))
        {
            // �Օ����łȂ��Ȃ��� Renderer �R���|�[�l���g��L���ɂ���
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }

    }
}

#endregion

