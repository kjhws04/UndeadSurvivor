using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    #region Slovel
    private float slovel_Range = 1.5f; //플레이어와 삽 거리
    #endregion
    #region Fire
    private float timmer;
    private float fireAttackSpeed = 15f;
    private float fireSpawnSpeed = 0.5f;
    #endregion

    PlayerController _player;

    private void Awake()
    {
        _player = GameManager._instance.player;
    }

    private void Update()
    {
        switch (id)
        {
            case 0: //회전 삽
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timmer += Time.deltaTime;
                if (timmer > speed)
                {
                    timmer = 0f;
                    Fire();
                }
                break;
        }

        //if (Input.GetButtonDown("Jump"))
        //{
        //    LevelUp(15f, count + 1);
        //}
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count = count;

        if (id == 0)
            Batch();

        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager._instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager._instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0: //회전 삽
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = fireSpawnSpeed * Character.WeaponRate;
                break;
        }

        //Hand Set
        Hand hand = _player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch() //생성한 무기 배치
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet_Shovl;

            if (i < transform.childCount)
                bullet_Shovl = transform.GetChild(i);
            else
            {
                bullet_Shovl = GameManager._instance.pool.Get(prefabId).transform;
                bullet_Shovl.parent = transform;
            }

            bullet_Shovl.localPosition = Vector3.zero;
            bullet_Shovl.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet_Shovl.Rotate(rotVec);
            bullet_Shovl.Translate(bullet_Shovl.up * slovel_Range, Space.World);

            bullet_Shovl.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -1 is Infinity per.
        }
    }

    void Fire()
    {
        if (!_player.scanner.nearestTarget) //몬스터 존재 확인
            return;

        Vector3 targetPos = _player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet_Fire = GameManager._instance.pool.Get(prefabId).transform;
        bullet_Fire.position = transform.position;

        bullet_Fire.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet_Fire.GetComponent<Bullet>().Init(damage, count, dir * fireAttackSpeed);

        AudioManager._instance.PlayerSfx(AudioManager.Sfx.Range);
    }
}
