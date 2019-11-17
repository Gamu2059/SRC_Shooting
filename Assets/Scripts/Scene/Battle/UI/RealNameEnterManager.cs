using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealNameEnterManager : MonoBehaviour
{
    public InputField m_InputField;
    public string m_EnterName{get; private set;}
    public bool m_NameEntered{get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        m_InputField = this.GetComponent<InputField>();
        this.gameObject.SetActive(false);
        m_NameEntered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeActive(bool active){
        this.gameObject.SetActive(active);
    }

    public void InputText(){
        m_EnterName = m_InputField.text;
        m_NameEntered = true;
    }

    private void OnEnable() {
        m_NameEntered = false;    
    }

    private void OnDisable() {
        m_NameEntered = false;    
    }
}
