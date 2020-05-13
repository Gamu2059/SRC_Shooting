using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// SubmitアクションをClickアクションに変換する
/// </summary>
public class SelectableSubmitToClick : MonoBehaviour, ISubmitHandler
{
    public void OnSubmit(BaseEventData e)
    {
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }
}
