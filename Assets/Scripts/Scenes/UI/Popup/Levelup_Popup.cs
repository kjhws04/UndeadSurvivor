using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levelup_Popup : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager._instance.Stop();

        AudioManager._instance.PlayerSfx(AudioManager.Sfx.LevelUp);
        GameManager._instance.player.inputVec = Vector2.zero;
        AudioManager._instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager._instance.Resume();
        AudioManager._instance.PlayerSfx(AudioManager.Sfx.Select);
        AudioManager._instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        Debug.Log(items[index].name);
        items[index].OnClick();
    }

    void Next()
    {
        //1. 모든 아이템 비활성화
        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        //2. 그 중에서 랜덤 3개 아이템 활성화
        int[] rand = new int[3];
        bool[] check = new bool[items.Length];
        bool isTry = false;
        rand[0] = Random.Range(0, items.Length);

        for (int i = 0; i < rand.Length; i++)
        {
            do
            {
                isTry = false;
                rand[i] = Random.Range(0, items.Length);
                if (check[rand[i]]) isTry = true;
                else check[rand[i]] = true;
            } while (isTry);
        }

        for (int i = 0; i < rand.Length; i++)
        {
            Item randItem = items[rand[i]];
            //3. 만렙 아이템의 경우 소비아이템으로 대체

            if (randItem.level == randItem.data.damages.Length)
            {
                items[3].gameObject.SetActive(true); //TODO
            }
            else
            {
                randItem.gameObject.SetActive(true);
            }
        }
    }
}
