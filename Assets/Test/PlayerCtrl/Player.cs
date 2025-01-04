using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("�v���C���[�J����")] Camera cam;
    [SerializeField, Tooltip("�ړ����x")]         float moveSpeed;
    [SerializeField, Tooltip("�����]�����鑬�x")] float rotSpeed;

    CharacterController controller;

    private float horizontal; // X��
    private float vertical;   // Z��

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // �J�����̑O����
        Vector3 cameraForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        
        // �v���C���[�̑O����
        Vector3 moveForward = cameraForward * vertical + cam.transform.right * horizontal;

        // �L�����N�^�[�R���g���[���[�ňړ�
        controller.Move(moveSpeed * Time.deltaTime * moveForward);

        // �v���C���[�������Ă�����
        if(moveForward != Vector3.zero)
        {
            MoveRotation(moveForward);
        }
    }

    /// <summary>
    /// InputSystem����Ăяo��
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }

    /// <summary>
    /// �v���C���[����͕����ɏ��X�ɉ�]������
    /// </summary>
    /// <param name="moveForward"></param> �v���C���[�̐i�s����
    void MoveRotation(Vector3 moveForward)
    {
        // ��]����p�x���v�Z
        Quaternion deg = Quaternion.LookRotation(moveForward);

        // ��]�ɑ��x��t����
        transform.rotation = Quaternion.Lerp(transform.rotation, deg, Time.deltaTime * rotSpeed);
    }

    ///// <summary>
    ///// �U�������^�C�~���O�œG�̕���������
    ///// </summary>
    ///// <param name="context"></param>
    //public void OnAtack(InputAction.CallbackContext context)
    //{
    //    transform.LookAt(target);
    //}
}
