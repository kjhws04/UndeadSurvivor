using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator _anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Character.Speed; 
        _anim.runtimeAnimatorController = animCon[GameManager._instance.playerId];  
    }

    private void FixedUpdate()
    {
        if (!GameManager._instance.isLive)
            return;

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue val)
    {
        if (_state == Define.State.Die || !GameManager._instance.isLive)
            return;
        inputVec = val.Get<Vector2>();
        State = Define.State.Moving;
    }

    private void LateUpdate()
    {
        if (!GameManager._instance.isLive)
            return;

        if (_state == Define.State.Die)
            return;

        if (inputVec.magnitude == 0)
            State = Define.State.Idle;

        if (inputVec.x != 0)
        {
            sprite.flipX = inputVec.x < 0;
        }
    }

    public override void Init()
    {

    }

    protected override void UpdateMoving()
    {
    }
    protected override void UpdateIdle()
    {
    }
    protected override void UpdateHit()
    {
    }
    protected override void UpdateDie()
    {
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager._instance.isLive)
            return;

        GameManager._instance.health -= Time.deltaTime * 10; //데미지입는 중 TODO
        
        if (GameManager._instance.health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            State = Define.State.Die;
            GameManager._instance.GameOver();
        }

    }
}
