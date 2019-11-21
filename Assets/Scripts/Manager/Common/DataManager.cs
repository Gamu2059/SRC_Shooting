using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体のデータを管理する。
/// </summary>
public class DataManager : ControllableObject
{
    public static DataManager Instance {
        get {
            if (GameManager.Instance == null)
            {
                return null;
            }

            return GameManager.Instance.DataManager;
        }
    }

    public BattleData BattleData { get; private set; }

    public DataManager(BattleRealPlayerLevelParamSet playerLevelParamSet)
    {
        BattleData = new BattleData(playerLevelParamSet);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }
}
