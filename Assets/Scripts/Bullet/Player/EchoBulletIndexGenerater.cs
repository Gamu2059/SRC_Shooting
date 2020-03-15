using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// EchoBulletのインデックスを管理するクラス。
/// </summary>
public class EchoBulletIndexGenerater
{
    public const int INDEX_MAX_VALUE = 10000;

    private int m_CurrentIndex = 0;
    private Dictionary<int, List<BattleRealCharaController>> m_HitHistory;

    public static EchoBulletIndexGenerater Instance {
        get;
        private set;
    }

    private EchoBulletIndexGenerater()
    {
        m_CurrentIndex = 0;
        m_HitHistory = new Dictionary<int, List<BattleRealCharaController>>();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public static void OnInitialize()
    {
        if (CheckExistInstance())
        {
            return;
        }
        else
        {
            Instance = new EchoBulletIndexGenerater();
        }
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public static void OnFinalize()
    {
        Instance.m_HitHistory.Clear();
        Instance.m_HitHistory = null;
        Instance = null;
    }

    /// <summary>
    /// このクラスのインスタンスが存在するかどうかを取得する。
    /// </summary>
    public static bool CheckExistInstance()
    {
        return Instance != null;
    }

    /// <summary>
    /// インデックスを生成する。
    /// </summary>
    public int GenerateBulletIndex()
    {
        m_CurrentIndex++;

        if (m_CurrentIndex >= INDEX_MAX_VALUE)
        {
            m_CurrentIndex = 0;
        }
        return m_CurrentIndex;
    }

    /// <summary>
    /// 指定したインデックスの被弾履歴をリフレッシュする。
    /// </summary>
    public void RefreshBulletIndex(int index)
    {
        if (!m_HitHistory.ContainsKey(index))
        {
            return;
        }

        m_HitHistory[index].Clear();
        m_HitHistory[index] = null;
        m_HitHistory.Remove(index);
    }

    /// <summary>
    /// 指定したインデックスの弾に対して被弾したキャラを登録する。
    /// </summary>
    public void RegisterHitChara(int index, BattleRealCharaController chara)
    {
        if (chara == null)
        {
            return;
        }

        if (!m_HitHistory.ContainsKey(index))
        {
            m_HitHistory.Add(index, new List<BattleRealCharaController>());
        }

        if (m_HitHistory[index].Contains(chara))
        {
            return;
        }

        m_HitHistory[index].Add(chara);
    }

    /// <summary>
    /// 指定したインデックスに対してキャラが登録されているかどうかを返す。
    /// </summary>
    public bool IsRegisteredChara(int index, BattleRealCharaController chara)
    {
        if (chara == null || !m_HitHistory.ContainsKey(index))
        {
            return false;
        }

        var history = m_HitHistory[index];
        if (history == null)
        {
            return false;
        }

        return history.Contains(chara);
    }
}
