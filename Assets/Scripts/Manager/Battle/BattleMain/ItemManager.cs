using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムを管理するマネージャ。
/// </summary>
public class ItemManager : SingletonMonoBehavior<ItemManager>
{
    [SerializeField]
    private Transform m_ItemHolder;

    /// <summary>
    /// STANDBY状態のアイテムを保持するリスト。
    /// </summary>
    [SerializeField]
    private List<ItemController> m_StandbyItems;

    /// <summary>
    /// UPDATE状態のアイテムを保持するリスト。
    /// </summary>
    [SerializeField]
    private List<ItemController> m_UpdateItems;

    /// <summary>
    /// POOL状態のアイテムを保持するリスト。
    /// </summary>
    [SerializeField]
    private List<ItemController> m_PoolItems;

    /// <summary>
    /// UPDATE状態に遷移するアイテムのリスト。
    /// </summary>
    private List<ItemController> m_GotoUpdateItems;

    /// <summary>
    /// POOL状態に遷移するアイテムのリスト。
    /// </summary>
    private List<ItemController> m_GotoPoolItems;

    /// <summary>
    /// STANDBY状態のアイテムを保持するリストを取得する。
    /// </summary>
    public List<ItemController> GetStandbyItems()
    {
        return m_StandbyItems;
    }

    /// <summary>
    /// UPDATE状態のアイテムを保持するリストを取得する。
    /// </summary>
    public List<ItemController> GetUpdateItems()
    {
        return m_UpdateItems;
    }

    /// <summary>
    /// POOL状態のアイテムを保持するリストを取得する。
    /// </summary>
    public List<ItemController> GetPoolItems()
    {
        return m_PoolItems;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        m_StandbyItems = new List<ItemController>();
        m_UpdateItems = new List<ItemController>();
        m_PoolItems = new List<ItemController>();
        m_GotoUpdateItems = new List<ItemController>();
        m_GotoPoolItems = new List<ItemController>();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        m_StandbyItems.Clear();
        m_UpdateItems.Clear();
        m_PoolItems.Clear();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (StageManager.Instance != null && StageManager.Instance.GetBulletHolder() != null)
        {
            m_ItemHolder = StageManager.Instance.GetBulletHolder().transform;
        }
        else if (m_ItemHolder == null)
        {
            var obj = new GameObject("[ItemHolder]");
            obj.transform.position = Vector3.zero;
            m_ItemHolder = obj.transform;
        }
    }

    public override void OnUpdate()
    {
        // Start処理
        foreach (var bullet in m_StandbyItems)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnStart();
            m_GotoUpdateItems.Add(bullet);
        }

        GotoUpdateFromStandby();

        // Update処理
        foreach (var bullet in m_UpdateItems)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnUpdate();
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var bullet in m_UpdateItems)
        {
            if (bullet == null)
            {
                continue;
            }

            bullet.OnLateUpdate();
        }

        GotoPoolFromUpdate();
    }



    /// <summary>
    /// UPDATE状態にする。
    /// </summary>
    private void GotoUpdateFromStandby()
    {
        int count = m_GotoUpdateItems.Count;

        for (int i = 0; i < count; i++)
        {
            int idx = count - i - 1;
            var bullet = m_GotoUpdateItems[idx];
            m_GotoUpdateItems.RemoveAt(idx);
            m_StandbyItems.Remove(bullet);
            m_UpdateItems.Add(bullet);
            bullet.SetItemCycle(ItemController.E_ITEM_CYCLE.UPDATE);
        }

        m_GotoUpdateItems.Clear();
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
            var bullet = m_GotoPoolItems[idx];
            bullet.SetItemCycle(ItemController.E_ITEM_CYCLE.POOLED);
            m_GotoPoolItems.RemoveAt(idx);
            m_UpdateItems.Remove(bullet);
            m_PoolItems.Add(bullet);
        }

        m_GotoPoolItems.Clear();
    }

    /// <summary>
    /// アイテムをSTANDBY状態にして制御下に入れる。
    /// </summary>
    public void CheckStandbyItem(ItemController item)
    {
        if (item == null || !m_PoolItems.Contains(item))
        {
            Debug.LogError("指定されたアイテムを追加できませんでした。");
            return;
        }

        m_PoolItems.Remove(item);
        m_StandbyItems.Add(item);
        item.gameObject.SetActive(true);
        item.SetItemCycle(ItemController.E_ITEM_CYCLE.STANDBY_UPDATE);
        item.OnInitialize();
    }

    /// <summary>
    /// 指定したアイテムを制御から外すためにチェックする。
    /// </summary>
    public void CheckPoolItem(ItemController item)
    {
        if (item == null || m_GotoPoolItems.Contains(item))
        {
            Debug.LogError("指定したアイテムを削除できませんでした。");
            return;
        }

        item.SetItemCycle(ItemController.E_ITEM_CYCLE.STANDBY_POOL);
        item.OnFinalize();
        m_GotoPoolItems.Add(item);
        item.gameObject.SetActive(false);
    }

    /// <summary>
    /// プールからアイテムを取得する。
    /// 足りなければ生成する。
    /// </summary>
    /// <param name="itemPrefab">取得や生成の情報源となるアイテムのプレハブ</param>
    public ItemController GetPoolingItem(ItemController itemPrefab)
    {
        if (itemPrefab == null)
        {
            return null;
        }

        string bulletId = itemPrefab.GetItemGroupId();
        ItemController item = null;

        foreach (var i in m_PoolItems)
        {
            if (i != null && i.GetItemGroupId() == bulletId)
            {
                item = i;
                break;
            }
        }

        if (item == null)
        {
            item = Instantiate(itemPrefab);
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

    }
}
