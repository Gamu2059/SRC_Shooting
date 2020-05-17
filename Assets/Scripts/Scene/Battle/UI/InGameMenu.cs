using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public void OnSubmitRetry()
    {
        var scene = BaseSceneManager.Instance.CurrentScene;
        BaseSceneManager.Instance.LoadScene(scene.Scene);
    }

    public void OnSubmitRetire()
    {
        BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
    }
}
