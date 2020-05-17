using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
public class SelectController : MonoBehaviour
{
    public void OnSelected()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
