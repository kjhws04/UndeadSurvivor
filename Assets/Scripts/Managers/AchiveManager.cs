using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockChar;
    public GameObject[] unlockChar;

    public GameObject uiNotice;

    enum Achive { unlockGrapes, unlockOrange }
    Achive[] achives;
    WaitForSecondsRealtime wait;

    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 0);

        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    private void Start()
    {
        unlockCharacter();
    }

    void unlockCharacter()
    {
        for (int i = 0; i < lockChar.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockChar[i].SetActive(!isUnlock);
            unlockChar[i].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch (achive)
        {
            case Achive.unlockGrapes:
                isAchive = GameManager._instance.kill >= 100;
                break;
            case Achive.unlockOrange:
                isAchive = GameManager._instance.gameTime == GameManager._instance.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager._instance.PlayerSfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
