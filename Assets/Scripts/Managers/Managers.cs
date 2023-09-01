using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    #region Contents
    GameManager _game = new GameManager();

    public static GameManager Game { get { return Instance._game; } }
    #endregion

    #region Core
    PoolManager _pool = new PoolManager();

    public static PoolManager Pool { get { return Instance._pool; } }
    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    static void Init()
    {
        if(s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();

            #region
            //초기화할 매니저 스크립트
            #endregion
        }
    }

    public static void Clear()
    {
    }
}
