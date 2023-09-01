using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health,
    }

    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                LevelUp();
                break;
            case InfoType.Level:
                LevelBar();
                break;
            case InfoType.Kill:
                KillInfo();
                break;
            case InfoType.Time:
                Time();
                break;
            case InfoType.Health:
                HealthBar();
                break;
            default:
                break;
        }
    }

    private void LevelUp()
    {
        float curExp = GameManager._instance.exp;
        float maxExp = GameManager._instance.nextExp[Mathf.Min(GameManager._instance.level, GameManager._instance.nextExp.Length - 1)];
        mySlider.value = curExp / maxExp;
    }

    private void LevelBar()
    {
        myText.text = string.Format("Lv.{0:F0}", GameManager._instance.level);
    }

    private void KillInfo()
    {
        myText.text = string.Format("{0:F0}", GameManager._instance.kill);
    }

    private void Time()
    {
        float remainTime = GameManager._instance.maxGameTime - GameManager._instance.gameTime;
        int min = Mathf.FloorToInt(remainTime / 60);
        int sec = Mathf.FloorToInt(remainTime % 60);

        myText.text = string.Format("{0:D2}:{1:D2}", min, sec); //D는 자리수 고정
    }

    private void HealthBar()
    {
        float curHealth = GameManager._instance.health;
        float maxHealth = GameManager._instance.maxHealth;
        mySlider.value = curHealth / maxHealth;
    }
}
