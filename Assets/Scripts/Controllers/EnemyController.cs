using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController
{
    public float _speed;
    public float _health;
    public float _maxHealth;
    public RuntimeAnimatorController[] _animCon; //sprite
    public Rigidbody2D _target;

    Rigidbody2D _rigid;
    Collider2D _col;
    SpriteRenderer _sprite;
    Animator _anim;
    WaitForFixedUpdate wait;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        State = Define.State.Moving;
    }

    void FixedUpdate()
    {
        if (!GameManager._instance.isLive)
            return;
        if (_state == Define.State.Die || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dir = _target.transform.position - _rigid.transform.position;
        Vector2 nextVec = dir.normalized * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec); //플레이어의 키입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager._instance.isLive)
            return;
        if (_state == Define.State.Die)
            return;

        _sprite.flipX = _target.position.x < _rigid.position.x;
    }

    private void OnEnable()
    {
        _target = GameManager._instance.player.GetComponent<Rigidbody2D>();

        _col.enabled = true;
        _rigid.simulated = true;
        _sprite.sortingOrder = 2;
        State = Define.State.Moving;
        _health = _maxHealth;
    }

    public override void Init()
    {
    }

    public void SpawnDate(SpawnData data)
    {
        // TODO Debug.Log(data);
        _anim.runtimeAnimatorController = _animCon[data.spriteType];
        _speed = data.speed;
        _maxHealth = data.health;
        _health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || _state == Define.State.Die)
            return;

        _health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine("KnockBackCoroutine");

        if (_health > 0 )
            Hit();
        else
        {
            _col.enabled = false;
            _rigid.simulated = false;
            _sprite.sortingOrder = 1;
            State = Define.State.Die;
            GameManager._instance.kill++;
            GameManager._instance.GetExp();

            if (GameManager._instance.isLive)
            {
                AudioManager._instance.PlayerSfx(AudioManager.Sfx.Dead);
            }
        }
    }

    IEnumerator KnockBackCoroutine()
    {
        yield return wait;
        Vector3 playerPos = GameManager._instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Hit()
    {
        State = Define.State.Hit;

        AudioManager._instance.PlayerSfx(AudioManager.Sfx.Hit);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    protected override void UpdateDie()
    {
        //Destroy(_rigid);
    }
}
