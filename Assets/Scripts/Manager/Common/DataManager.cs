using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体のデータを管理する。
/// </summary>
public class DataManager : Singleton<DataManager>
{
    #region Field

    /// <summary>
    /// ゲームモードや難易度の選択などを行ったかどうか
    /// </summary>
    public bool IsSelectedGame;

    /// <summary>
    /// ゲームモード
    /// </summary>
    public E_GAME_MODE GameMode;

    /// <summary>
    /// 難易度
    /// </summary>
    public E_DIFFICULTY Difficulty;

    /// <summary>
    /// ステージ
    /// </summary>
    public E_CHAPTER Chapter;

    /// <summary>
    /// バトルパラメータセット
    /// </summary>
    public BattleParamSet BattleParamSet { get; private set; }

    /// <summary>
    /// 読み取り専用のパラメータを格納したデータ。
    /// </summary>
    public BattleConstantParam BattleConstantParam { get; private set; }

    /// <summary>
    /// バトル用変数データ。
    /// </summary>
    public BattleData BattleData { get; private set; }
    
    /// <summary>
    /// バトルのリザルトデータを格納するもの。
    /// </summary>
    public BattleResultData BattleResultData { get; private set; }

    #endregion

    public static DataManager Builder(BattleConstantParam param)
    {
        var manager = Create();
        manager.OnInitialize();

        manager.IsSelectedGame = false;
        manager.BattleConstantParam = param;
        manager.BattleData = new BattleData(param);

        return manager;
    }

    public override void OnFinalize()
    {

        base.OnFinalize();
    }

    /// <summary>
    /// 最初のバトル開始時に呼び出す。
    /// </summary>
    public void OnBattleStart()
    {

    }
}
