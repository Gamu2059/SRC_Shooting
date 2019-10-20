using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// リアルモードのアイテムを管理するマネージャ。
/// </summary>
public class BattleRealItemManager : ControllableObject, IColliderProcess
{
    public static BattleRealItemManager Instance => BattleRealManager.Instance.ItemManager;

    #region Field

    private BattleRealItemManagerParamSet m_ParamSet;

    private Transform m_ItemHolder;

    /// <summary>
    /// STANDBY状態のアイテムを保持するリスト。
    /// </summary>
    private List<BattleRealItemController> m_StandbyItems;

    /// <summary>
    /// UPDATE状態のアイテムを保持するリスト。
    /// </summary>
    private List<BattleRealItemController> m_UpdateItems;
    public List<BattleRealItemController> Items => m_UpdateItems;

    /// <summary>
    /// POOL状態のアイテムを保持するリスト。
    /// </summary>
    private List<BattleRealItemController> m_PoolItems;

    /// <summary>
    /// POOL状態に遷移するアイテムのリスト。
    /// </summary>
    private List<BattleRealItemController> m_GotoPoolItems;

    /// <summary>
    /// プレハブの対応ディクショナリ。
    /// </summary>
    private Dictionary<E_ITEM_TYPE, BattleRealItemController> m_ItemPrefabs;

    #endregion


    public BattleRealItemManager(BattleRealItemManagerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_StandbyItems = new List<BattleRealItemController>();
        m_UpdateItems = new List<BattleRealItemController>();
        m_PoolItems = new List<BattleRealItemController>();
        m_GotoPoolItems = new List<BattleRealItemController>();

        m_ItemPrefabs = new Dictionary<E_ITEM_TYPE, BattleRealItemController>();
        if (m_ParamSet != null)
        {
            foreach (var set in m_ParamSet.ItemPrefabSets)
            {
                m_ItemPrefabs.Add(set.ItemType, set.ItemPrefab);
            }
        }
    }

    public override void OnFinalize()
    {
        m_StandbyItems.Clear();
        m_UpdateItems.Clear();
        m_PoolItems.Clear();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        m_ItemHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.ITEM);
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var item in m_StandbyItems)
        {
            if (item == null)
            {
                continue;
            }

            item.OnStart();
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.OnLateUpdate();
        }
    }

    #endregion

    #region Impl IColliderProcess

    public void ClearColliderFlag()
    {
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.ClearColliderFlag();
        }
    }

    public void UpdateCollider()
    {
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.UpdateCollider();
        }
    }

    public void ProcessCollision()
    {
        foreach (var item in m_UpdateItems)
        {
            if (item == null)
            {
                continue;
            }

            item.ProcessCollision();
        }
    }

    #endregion

    /// <summary>
    /// 破棄フラグが立っているものをプールに戻す
    /// </summary>
    public void GotoPool()
    {
        GotoPoolFromUpdate();
    }

    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateFromStandby()
    {
        foreach (var item in m_StandbyItems)
        {
            if (item == null)
            {
                continue;
            }
            else if (item.GetCycle() != E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE)
            {
                CheckPoolItem(item);
            }

            item.SetCycle(E_POOLED_OBJECT_CYCLE.UPDATE);
            m_UpdateItems.Add(item);
        }

        m_StandbyItems.Clear();
    }

    /// <summary>
    /// POOL状態にする。
    /// </summary>
    private void GotoPoolFromUpdate()
    {
        int count = m_GotoPoolItems.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var item = m_GotoPoolItems[idx];
            item.OnFinalize();
            item.SetCycle(E_POOLED_OBJECT_CYCLE.POOLED);
            item.gameObject.SetActive(false);
            m_GotoPoolItems.RemoveAt(idx);
            m_UpdateItems.Remove(item);
            m_PoolItems.Add(item);
        }

        m_GotoPoolItems.Clear();
    }

    /// <summary>
    /// アイテムをSTANDBY状態にして制御下に入れる。
    /// </summary>
    private void CheckStandbyItem(BattleRealItemController item)
    {
        if (item == null || !m_PoolItems.Contains(item))
        {
            Debug.LogError("指定されたアイテムを追加できませんでした。");
            return;
        }

        m_PoolItems.Remove(item);
        m_StandbyItems.Add(item);
        item.gameObject.SetActive(true);
        item.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_UPDATE);
        item.OnInitialize();
    }

    /// <summary>
    /// 指定したアイテムを制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolItem(BattleRealItemController item)
    {
        if (item == null || m_GotoPoolItems.Contains(item))
        {
            Debug.LogError("指定したアイテムを削除できませんでした。");
            return;
        }

        item.SetCycle(E_POOLED_OBJECT_CYCLE.STANDBY_POOL);
        m_GotoPoolItems.Add(item);
    }

    private BattleRealItemController GetItemPrefab(E_ITEM_TYPE itemType)
    {
        if (m_ItemPrefabs == null || !m_ItemPrefabs.ContainsKey(itemType))
        {
            return null;
        }

        return m_ItemPrefabs[itemType];
    }

    /// <summary>
    /// プールからアイテムを取得する。
    /// 足りなければ生成する。
    /// </summary>
    private BattleRealItemController GetPoolingItem(E_ITEM_TYPE itemType)
    {
        BattleRealItemController item = null;

        foreach (var i in m_PoolItems)
        {
            if (i != null && i.ItemType == itemType)
            {
                item = i;
                break;
            }
        }

        if (item == null)
        {
            var prefab = GetItemPrefab(itemType);
            if (prefab == null)
            {
                return null;
            }

            item = GameObject.Instantiate(prefab);
            item.transform.SetParent(m_ItemHolder);
            m_PoolItems.Add(item);
        }

        return item;
    }

    /// <summary>
    /// 指定した座標から指定した情報でアイテムを生成する。
    /// </summary>
    public void CreateItem(Vector3 position, ItemCreateParam param)
    {
        if (param.ItemSpreadParams == null || param.ItemSpreadParams.Length < 1)
        {
            return;
        }

        for (int i = 0; i < param.ItemSpreadParams.Length; i++)
        {
            var spreadParam = param.ItemSpreadParams[i];

            for (int j = 0; j < spreadParam.Num; j++)
            {
                var item = GetPoolingItem(spreadParam.Type);
                if (item == null)
                {
                    continue;
                }

                var pos = position;
                item.transform.SetParent(m_ItemHolder);
                if (spreadParam.Type != param.CenterCreateItemType)
                {
                    var r = spreadParam.SpreadRadius;
                    var x = Random.Range(-r, r);
                    var zR = Mathf.Sqrt(r * r - x * x);
                    var z = Random.Range(-zR, zR);
                    pos += new Vector3(x, 0, z);
                }

                item.ChangeOrbital(m_ParamSet.OrbitalParam);
                item.SetParam(pos, spreadParam.Type, spreadParam.Point, m_ParamSet.AttractRate);
                CheckStandbyItem(item);
            }
        }
    }

    /// <summary>
    /// 全てのアイテムを吸引状態にする。
    /// </summary>
    public void AttractAllItem()
    {
        foreach (var item in m_UpdateItems)
        {
            item.AttractPlayer();
        }
    }

    public void OnAttractAction(E_INPUT_STATE state)
    {
        AttractAllItem();
    }

    /// <summary>
    /// アイテムがアイテムフィールドの範囲外に出ているかどうかを判定する。
    /// </summary>
    public bool IsOutOfField(BattleRealItemController item)
    {
        if (item == null)
        {
            return true;
        }

        var stageManager = BattleRealStageManager.Instance;
        var minPos = stageManager.MinLocalFieldPosition;
        var maxPos = stageManager.MaxLocalFieldPosition;
        minPos += m_ParamSet.MinOffsetFieldPosition;
        maxPos += m_ParamSet.MaxOffsetFieldPosition;

        var pos = item.transform.position;

        return pos.x < minPos.x || pos.x > maxPos.x || pos.z < minPos.y || pos.z > maxPos.y;
    }
}
