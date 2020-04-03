using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーケンス制御されるオブジェクト。
/// </summary>
public class BattleRealSequenceObjectController : ControllableMonoBehavior
{
    #region Field Inspector

    [SerializeField]
    private SequenceController m_SequenceController;

    #endregion

    #region Field

    public E_OBJECT_CYCLE Cycle;
    private List<ControllableMonoBehavior> AutoControlBehaviors;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_SequenceController.OnInitialize();
        m_SequenceController.OnEndSequence = Destroy;
    }

    public override void OnFinalize()
    {
        m_SequenceController.OnFinalize();
        AutoControlBehaviors?.Clear();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        AutoControlBehaviors = new List<ControllableMonoBehavior>();
        var behaviors = GetComponents<ControllableMonoBehavior>();
        foreach (var b in behaviors)
        {
            if (b is IAutoControlOnCharaController)
            {
                AutoControlBehaviors.Add(b);
                b.OnStart();
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_SequenceController.OnUpdate();
        foreach (var b in AutoControlBehaviors)
        {
            if (b is IAutoControlOnCharaController c && c.IsEnableController)
            {
                b.OnUpdate();
            }
        }
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_SequenceController.OnLateUpdate();
        foreach (var b in AutoControlBehaviors)
        {
            if (b is IAutoControlOnCharaController c && c.IsEnableController)
            {
                b.OnLateUpdate();
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_SequenceController.OnFixedUpdate();
        foreach (var b in AutoControlBehaviors)
        {
            if (b is IAutoControlOnCharaController c && c.IsEnableController)
            {
                b.OnFixedUpdate();
            }
        }
    }

    #endregion

    public void BuildSequence(SequenceGroup sequenceGroup)
    {
        m_SequenceController.BuildSequence(sequenceGroup);
    }

    public void Destroy()
    {
        if (Cycle == E_OBJECT_CYCLE.STANDBY_UPDATE || Cycle == E_OBJECT_CYCLE.UPDATE)
        {
            BattleRealSequenceObjectManager.Instance.CheckStandbyDestroy(this);
        }
    }
}
