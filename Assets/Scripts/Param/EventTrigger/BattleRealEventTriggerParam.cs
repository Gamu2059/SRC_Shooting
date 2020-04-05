using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Event/EventTrigger", fileName = "param.event_trigger.asset")]
public class BattleRealEventTriggerParam : ScriptableObject
{
    [Tooltip("イベントトリガの名前。ゲームデータとして使うのではなく、一覧の中で識別しやすくするために使用します。")]
    public string EventTriggerName;

    [Tooltip("イベントを実行してもこのトリガを削除しないかどうか。trueにする場合は、イベントが連続で発生しないよう発動条件を工夫しましょう。")]
    public bool DontDestroy = false;

    [Tooltip("イベントの発生条件")]
    public EventTriggerRootCondition Condition;

    [Tooltip("イベント内容")]
    public BattleRealEventContent[] Contents;
}
