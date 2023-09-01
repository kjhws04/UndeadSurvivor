using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{

    [Header("Stste & Pos Info")]
    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    protected Animator anim;

	public virtual Define.State State
	{
		get { return _state; }
		set
		{
			_state = value;

			anim = GetComponent<Animator>();
			switch (_state)
			{
				case Define.State.Die:
					anim.CrossFade("Die", 0.1f);
					break;
				case Define.State.Idle:
					anim.CrossFade("Idle", 0.1f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("Run", 0.1f);
					break;
                case Define.State.Hit:
                    anim.SetTrigger("Hit");
                    break;
            }
		}
	}
	void Start()
	{
		Init();
	}

	void Update()
	{
		if (!GameManager._instance.isLive)
			return;

		switch (State)
		{
			case Define.State.Die:
				UpdateDie();
				break;
			case Define.State.Moving:
				UpdateMoving();
				break;
			case Define.State.Idle:
				UpdateIdle();
				break;
			case Define.State.Hit:
				UpdateHit();
				break;
		}
		UpdatePotionCoolTime();
	}

	public abstract void Init();

	protected virtual void UpdateDie() { }
	protected virtual void UpdateMoving() { }
	protected virtual void UpdateIdle() { }
	protected virtual void UpdateHit() { }


	protected virtual void UpdatePotionCoolTime() { }
}
