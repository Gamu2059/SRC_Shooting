using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードで表示される敵HPゲージの管理を行うマネージャ
/// </summary>
public class BattleRealHpIndicatorManager : ControllableObject
{
    public static BattleRealHpIndicatorManager Instance => BattleRealUiManager.Instance.BattleRealHpIndicatorManager;

    #region Field

    private Transform m_HpIndicatorHolder;

    /// <summary>
    /// STANDBY状態のHPゲージを保持するリスト
    /// </summary>
    private List<BattleRealHpIndicator> m_StandbyHpIndicators;

    public List<BattleRealHpIndicator> StandbyHpIndicators => m_StandbyHpIndicators;

    /// <summary>
    /// UPDATE状態のHPゲージを保持するリスト
    /// </summary>
    private List<BattleRealHpIndicator> m_UpdateHpIndicators;

    public List<BattleRealHpIndicator> UpdateHpIndicators => m_UpdateHpIndicators;

    /// <summary>
    /// POOL状態のHPゲージを保持するリスト
    /// </summary>
    private List<BattleRealHpIndicator> m_PoolHpIndicators;

    public List<BattleRealHpIndicator> PoolHpIndicators => m_PoolHpIndicators;

    /// <summary>
    /// GotoPool状態のHPゲージを保持するリスト
    /// </summary>
    private List<BattleRealHpIndicator> m_GotoPoolHpIndicators;

    public List<BattleRealHpIndicator> GotoPoolIndicators => m_GotoPoolHpIndicators;
    
    private List<BattleRealHpIndicator> m_BattleRealHpIndicatorPrefabs;


    #endregion

    #region GameCycle

    public override void OnInitialize(){
       m_StandbyHpIndicators = new List<BattleRealHpIndicator>();
       m_UpdateHpIndicators = new List<BattleRealHpIndicator>();
       m_PoolHpIndicators = new List<BattleRealHpIndicator>();
       m_GotoPoolHpIndicators = new List<BattleRealHpIndicator>();

       m_BattleRealHpIndicatorPrefabs = new List<BattleRealHpIndicator>();
    }

    public override void OnFinalize(){
        m_StandbyHpIndicators.Clear();
        m_UpdateHpIndicators.Clear();
        m_PoolHpIndicators.Clear();
        base.OnFinalize();
    }

    public override void OnStart(){
        base.OnStart();
        m_HpIndicatorHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.HP_INDICATOR);
    }

    public override void OnUpdate(){
        foreach(var hpIndicator in m_StandbyHpIndicators){
            if(hpIndicator == null){
                continue;
            }
            hpIndicator.OnStart();
        }

        GotoUpdateFromStandby();

        foreach (var hpIndicator in m_UpdateHpIndicators)
        {
            if(hpIndicator == null){
                continue;
            }

            hpIndicator.OnUpdate();
        }
    }

    public override void OnLateUpdate(){
        foreach (var hpIndicator in m_UpdateHpIndicators)
        {
            if(hpIndicator == null){
                continue;
            }
            
            hpIndicator.OnLateUpdate();
        }
    }

    #endregion

    #region Pooling

    /// <summary>
    /// 破棄フラグが立っているものをプールに戻す
    /// </summary>
    public void GotoPool(){
        GotoPoolFromUpdate();
    }

    /// <summary>
    /// UPDATE状態にする
    /// </summary>
    private void GotoUpdateFromStandby(){
        foreach(var hpIndicator in m_StandbyHpIndicators){
            if(hpIndicator == null){
                continue;
            }else if(hpIndicator.GetCycle() != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE){
                CheckPoolHpIndicator(hpIndicator);
            }

            hpIndicator.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
            m_UpdateHpIndicators.Add(hpIndicator);
        }

        m_StandbyHpIndicators.Clear();
    }

    /// <summary>
    /// POOL状態にする
    /// </summary>
    private void GotoPoolFromUpdate(){
        int cnt = m_GotoPoolHpIndicators.Count;

        for(int i=0;i<cnt;i++){
            int idx = cnt - i - 1;
            var hpIndicator = m_GotoPoolHpIndicators[idx];
            hpIndicator.OnFinalize();
            hpIndicator.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            hpIndicator.gameObject.SetActive(false);
            m_GotoPoolHpIndicators.RemoveAt(idx);
            m_UpdateHpIndicators.Remove(hpIndicator);
            m_PoolHpIndicators.Add(hpIndicator);
        }

        m_GotoPoolHpIndicators.Clear();
    }

    /// <summary>
    /// アイテムをSTANDBY状態にして制御下に入れる
    /// </summary>
    private void CheckStandbyHpIndicator(BattleRealHpIndicator hpIndicator){
        if(hpIndicator ==null || !m_PoolHpIndicators.Contains(hpIndicator)){
            Debug.LogError("指定されたアイテムを追加できませんでした");
            return;
        }

        m_PoolHpIndicators.Remove(hpIndicator);
        m_StandbyHpIndicators.Add(hpIndicator);
        hpIndicator.gameObject.SetActive(true);
        hpIndicator.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE);
        hpIndicator.OnInitialize();
    }

    /// <summary>
    /// 指定したアイテムを制御から外すためにチェックする
    /// </summary>
    /// <param name="hpIndicator"></param>
    public void CheckPoolHpIndicator(BattleRealHpIndicator hpIndicator){
        if(hpIndicator == null || !m_GotoPoolHpIndicators.Contains(hpIndicator)){
            Debug.LogError("指定したアイテムを削除できませんでした");
            return;
        }

        hpIndicator.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_GotoPoolHpIndicators.Add(hpIndicator);
    }

    /// <summary>
    /// プールされたアイテムと同じプレハブを生成する
    /// </summary>
    /// <returns></returns>
    private BattleRealHpIndicator GetHpIndicatorPrefab(){
        if(m_BattleRealHpIndicatorPrefabs == null){
            return null;
        }

        return m_BattleRealHpIndicatorPrefabs[0];
    }

    /// <summary>
    /// プールからアイテムを取得する
    /// 足りなければ生成する
    /// </summary>
    /// <returns></returns>
    private BattleRealHpIndicator GetPoolingHpIndicator(){
        BattleRealHpIndicator hpIndicator = null;

        foreach(var i in m_PoolHpIndicators){
            if(i!=null){
                hpIndicator = i;
                break;
            }
        }

        if(hpIndicator == null){
            var prefab = GetHpIndicatorPrefab();
            if(prefab == null){
                return null;
            }
            hpIndicator = GameObject.Instantiate(prefab);
            hpIndicator.transform.SetParent(m_HpIndicatorHolder);
            m_PoolHpIndicators.Add(hpIndicator);
        }

        return hpIndicator;
    }

    #endregion

    /// <summary>
    /// 指定した座標からアイテムを生成する
    /// </summary>
    /// <param name="enemyController"></param>
    public void CreateHpIndicator(BattleRealEnemyController enemyController){
        if(enemyController == null){
            return;
        }
        var hpIndicator = GetPoolingHpIndicator();
        if(hpIndicator == null){
            return;
        }
        var viewPortPos = BattleRealStageManager.Instance.CalcViewportPosFromWorldPosition(enemyController.transform.position);
        hpIndicator.transform.SetParent(m_HpIndicatorHolder);
        CheckStandbyHpIndicator(hpIndicator);
    }
}
