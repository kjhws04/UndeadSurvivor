using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool isLive;

    [Header("#Player info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 15, 21, 28, 36, 45, 55, 66, 78, 91, 105, 120, 136, 153, 171, 190, 210, 231, 253, 276 };

    //need Component
    public PoolManager pool;
    public PlayerController player;
    public Levelup_Popup UI_LevelUp;
    public Result uiResult;

    public Transform uiJoy;
    public GameObject enemyCleaner;

    GameObject _player;

    public GameObject GetPlayer() { return _player; }

    private void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 60; //60ÇÁ·¹ÀÓ
    }

    public void GameStart(int id)
    {
        //Init
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        //temp TODO
        UI_LevelUp.Select(4);
        Resume();

        AudioManager._instance.PlayBgm(true);
        AudioManager._instance.PlayerSfx(AudioManager.Sfx.Select);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            UI_LevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }

    public void GameVictory()
    {
        StartCoroutine("GameVictoryCoroutine");
    }

    IEnumerator GameVictoryCoroutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager._instance.PlayBgm(false);
        AudioManager._instance.PlayerSfx(AudioManager.Sfx.Win);
    }

    public void GameOver()
    {
        StartCoroutine("GameOverCoroutine");
    }
    
    IEnumerator GameOverCoroutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager._instance.PlayBgm(false);
        AudioManager._instance.PlayerSfx(AudioManager.Sfx.Lose);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0); //Scene TODO
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}
