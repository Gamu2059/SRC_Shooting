using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シューティングパートに関するデータを管理する。
/// </summary>
public class DataManager : SingletonMonoBehavior<DataManager>
{
    #region Field Inspector

    [SerializeField]
    private DataManagerParamSet m_ParamSet;

    #endregion

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
    /// 残機オプション
    /// </summary>
    public int LifeOption;

    /// <summary>
    /// エナジーオプション
    /// </summary>
    public int EnergyOption;

    /// <summary>
    /// バトル用変数データ。
    /// </summary>
    public BattleData BattleData { get; private set; }
    
    /// <summary>
    /// バトルのリザルトデータを格納するもの。
    /// </summary>
    public BattleResultData BattleResultData { get; private set; }

    #endregion

    public override void OnInitialize()
    {
        base.OnInitialize();
        IsSelectedGame = false;
        GameMode = default;
        Difficulty = default;
        Chapter = default;
        LifeOption = 0;
        EnergyOption = 0;
    }

    public override void OnFinalize()
    {
        BattleResultData?.OnFinalize();
        BattleData?.OnFinalize();
        BattleResultData = null;
        BattleData = null;
        base.OnFinalize();
    }

    /// <summary>
    /// バトルパラメータセットを取得する。
    /// </summary>
    public BattleParamSet GetCurrentBattleParamSet()
    {
        return m_ParamSet.BattleParamSetHolder.GetBattleParamSet(Chapter, Difficulty);
    }

    /// <summary>
    /// シューティングパート開始時に呼び出す
    /// </summary>
    public void OnShootingStart()
    {
        ulong bestScore = 0;
        if (GameMode == E_GAME_MODE.CHAPTER)
        {
            bestScore = PlayerRecordManager.Instance.GetChapterModeBestScore(Chapter, Difficulty);
        }

        var data = new BattleData.DataOnConstructor
        {
            ConstParam = m_ParamSet.BattleConstantParam,
            LifeOption = LifeOption,
            EnergyOption = EnergyOption,
            BestScore = bestScore,
        };

        BattleData = new BattleData(data);
        BattleResultData = new BattleResultData(GameMode, Difficulty);
    }

    /// <summary>
    /// シューティングパート終了時に呼び出す
    /// </summary>
    public void OnShootingEnd()
    {
        Debug.Log("OnShootingEnd");
        if (GameMode == E_GAME_MODE.CHAPTER)
        {
            BattleResultData.SaveChapterResult(Chapter);
        }

        BattleResultData?.OnFinalize();
        BattleData?.OnFinalize();
        BattleResultData = null;
        BattleData = null;
    }

    /// <summary>
    /// ストーリーモードでもチャプターモードでも、チャプターごとに開始時に呼び出す
    /// </summary>
    public void OnChapterStart()
    {
        BattleData.InitDataOnChapterStart(new BattleData.DataOnChapterStart
        {
            AchievementParam = m_ParamSet.BattleAchievementParamSet.GetAchievementParam(Chapter, Difficulty),
            Chapter = Chapter,
        });
    }

    /// <summary>
    /// ストーリーモードでもチャプターモードでも、チャプターごとに終了時に呼び出す
    /// </summary>
    public void OnChapterEnd(bool isGameClear)
    {
        var rankParam = m_ParamSet.BattleRankParamSet.GetRankParam(Chapter, Difficulty);
        BattleResultData.AddChapterResult(Chapter, BattleData, rankParam, isGameClear);
    }

    /// <summary>
    /// ストーリーモードの終了時に呼び出す
    /// </summary>
    public void OnStoryEnd()
    {

    }

    /// <summary>
    /// 次のチャプターに直接遷移するかどうか。簡易実装
    /// </summary>
    public bool IsDirectTransitionNextChapter()
    {
        return GameMode == E_GAME_MODE.STORY && Chapter == E_CHAPTER.CHAPTER_0;
    }

    public BaseSceneManager.E_SCENE GetChapterScene(E_CHAPTER chapter)
    {
        switch (chapter)
        {
            case E_CHAPTER.CHAPTER_0:
                return BaseSceneManager.E_SCENE.STAGE0;
            case E_CHAPTER.CHAPTER_1:
                return BaseSceneManager.E_SCENE.STAGE1;
            case E_CHAPTER.CHAPTER_2:
                return BaseSceneManager.E_SCENE.STAGE2;
            case E_CHAPTER.CHAPTER_3:
                return BaseSceneManager.E_SCENE.STAGE3;
            case E_CHAPTER.CHAPTER_4:
                return BaseSceneManager.E_SCENE.STAGE4;
            case E_CHAPTER.CHAPTER_5:
                return BaseSceneManager.E_SCENE.STAGE5;
            case E_CHAPTER.CHAPTER_6:
                return BaseSceneManager.E_SCENE.STAGE6;
        }

        return BaseSceneManager.E_SCENE.DEFAULT;
    }

    /// <summary>
    /// 次のチャプターが存在するかどうか。簡易実装
    /// </summary>
    public bool ExistNextChapter()
    {
        switch (Chapter)
        {
            case E_CHAPTER.CHAPTER_0:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// 次のチャプターの値を取得する。簡易実装
    /// </summary>
    public E_CHAPTER GetNextChapter()
    {
        switch (Chapter)
        {
            case E_CHAPTER.CHAPTER_0:
                return E_CHAPTER.CHAPTER_1;
        }

        return E_CHAPTER.CHAPTER_0;
    }

    public string GetChapterString()
    {
        switch (Chapter)
        {
            case E_CHAPTER.CHAPTER_0:
                return "Chapter. 0";
            case E_CHAPTER.CHAPTER_1:
                return "Chapter. 1";
            case E_CHAPTER.CHAPTER_2:
                return "Chapter. 2";
            case E_CHAPTER.CHAPTER_3:
                return "Chapter. 3";
            case E_CHAPTER.CHAPTER_4:
                return "Chapter. 4";
            case E_CHAPTER.CHAPTER_5:
                return "Chapter. 5";
            case E_CHAPTER.CHAPTER_6:
                return "Final Chapter";
        }

        return "";
    }

    /// <summary>
    /// Achievementを無効にするかどうか
    /// </summary>
    public bool IsInvalidAchievement()
    {
        return Chapter == E_CHAPTER.CHAPTER_0;
    }

    /// <summary>
    /// 現在のチャプターに対応するシーンに遷移する
    /// </summary>
    public void TransitionToCurrentChapterScene()
    {
        var scene = GetChapterScene(Chapter);
        if (scene == BaseSceneManager.E_SCENE.DEFAULT)
        {
            Debug.LogWarning("Default scene is not chapter scene.");
            return;
        }

        BaseSceneManager.Instance.LoadScene(scene);
    }
}
