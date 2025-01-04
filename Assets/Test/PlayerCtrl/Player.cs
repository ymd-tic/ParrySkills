using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーカメラ")] Camera cam;
    [SerializeField, Tooltip("移動速度")]         float moveSpeed;
    [SerializeField, Tooltip("方向転換する速度")] float rotSpeed;

    CharacterController controller;

    private float horizontal; // X軸
    private float vertical;   // Z軸

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // カメラの前方向
        Vector3 cameraForward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        
        // プレイヤーの前方向
        Vector3 moveForward = cameraForward * vertical + cam.transform.right * horizontal;

        // キャラクターコントローラーで移動
        controller.Move(moveSpeed * Time.deltaTime * moveForward);

        // プレイヤーが動いていたら
        if(moveForward != Vector3.zero)
        {
            MoveRotation(moveForward);
        }
    }

    /// <summary>
    /// InputSystemから呼び出す
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }

    /// <summary>
    /// プレイヤーを入力方向に徐々に回転させる
    /// </summary>
    /// <param name="moveForward"></param> プレイヤーの進行方向
    void MoveRotation(Vector3 moveForward)
    {
        // 回転する角度を計算
        Quaternion deg = Quaternion.LookRotation(moveForward);

        // 回転に速度を付ける
        transform.rotation = Quaternion.Lerp(transform.rotation, deg, Time.deltaTime * rotSpeed);
    }

    ///// <summary>
    ///// 攻撃したタイミングで敵の方向を向く
    ///// </summary>
    ///// <param name="context"></param>
    //public void OnAtack(InputAction.CallbackContext context)
    //{
    //    transform.LookAt(target);
    //}
}
