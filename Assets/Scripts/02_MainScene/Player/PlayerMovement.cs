using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player에 적용
// Ctrler에 JoyStickZone 할당

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private JoyStickController Ctrler;

    private Rigidbody rigidbody;
    private Animator anim;
    Vector3 moveVec;

    //private static readonly int MoveState = Animator.StringToHash("Base Layer.move");

    private Player player;

    private void Awake()
    {
        PlayerMovementInit();
    }

    // 플레이어 움직임 관련 데이터 초기화
    private void PlayerMovementInit()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        player = GetComponent<Player>();
        player.speed = 1f;
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerAnim();
    }

    // 플레이어 이동
    private void PlayerMove()
    {
        // 조이스틱 움직임 값 받아오기
        float x = Ctrler.Horizontal;
        float z = Ctrler.Vertical;

        // 플레이어 이동
        moveVec = new Vector3(x, 0, z) * player.speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + moveVec);

        // 플레이어 회전
        Quaternion dirQuat = Quaternion.LookRotation(moveVec);
        Quaternion moveQuat = Quaternion.Slerp(rigidbody.rotation, dirQuat, 0.3f);
        rigidbody.MoveRotation(moveQuat);
    }

    // 플레이어 움직임 애니메이터
    private void PlayerAnim()
    {
        anim.SetFloat("move", moveVec.sqrMagnitude);
    }
}
