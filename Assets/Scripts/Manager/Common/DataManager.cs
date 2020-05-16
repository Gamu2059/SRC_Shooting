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

    public static DataManager Builder(BattleConstantParam constantParam, BattleAchievementParamSet achievementParamSet)
    {
        var manager = Create();
        manager.OnInitialize();

        manager.IsSelectedGame = false;
        manager.BattleData = new BattleData(constantParam, achievementParamSet);

        return manager;
    }

    public override void OnFinalize()
    {

        base.OnFinalize();
    }

    /// <summary>
    /// バトルパラメータセットを取得する。
    /// </summary>
    public BattleParamSet GetCurrentBattleParamSet()
    {
        return GameManager.Instance.BattleParamSetHolder.GetBattleParamSet(Chapter, Difficulty);
    }

    /// <summary>
    /// ストーリーモードの開始時に呼び出す
    /// </summary>
    public void OnStoryStart()
    {
    }

    /// <summary>
    /// ストーリーモードの終了時に呼び出す
    /// </summary>
    public void OnStoryEnd()
    {
        //BattleResultData.AddStoryResult();
    }

    /// <summary>
    /// ストーリーモードでもチャプターモードでも、チャプターごとに開始時に呼び出す
    /// </summary>
    public void OnChapterStart()
    {
        BattleData.ResetDataOnChapterStart();
    }

    /// <summary>
    /// ストーリーモードでもチャプターモードでも、チャプターごとに終了時に呼び出す
    /// </summary>
    public void OnChapterEnd()
    {
        //BattleResultData.AddChapterResult();
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
