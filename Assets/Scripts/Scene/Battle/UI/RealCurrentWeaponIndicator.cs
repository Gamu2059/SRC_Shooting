#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RealCurrentWeaponIndicator : MonoBehaviour
{
    private enum E_WEAPON_TYPE{
        NORMAL_SHOT,
        LASER,
        BOMB,
    }

    [SerializeField]
    private Text m_OutText;

    private E_WEAPON_TYPE m_CurrentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        if(BattleRealPlayerManager.Instance != null){
            RegisterCurrentWeapon();
        }else{
            BattleRealPlayerManager.OnStartAction += RegisterCurrentWeapon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleRealManager.Instance == null){
            return;
        }

        string label = "";
        switch(DecideCurrentWeapon(BattleRealPlayerManager.Instance.IsNormalWeapon, BattleRealPlayerManager.Instance.IsLaserType)){
                case E_WEAPON_TYPE.NORMAL_SHOT:
                    label = "Shot";
                    break;
                case E_WEAPON_TYPE.LASER:
                    label = "Laser";
                    break;
                case E_WEAPON_TYPE.BOMB:
                    label = "Bomb";
                    break;
                default:
                    label = "ERROR";
                    break;
        }

        if(m_OutText.text != label){
            m_OutText.text = label;
        }
    }

    private void RegisterCurrentWeapon(){
        var initialWeaponType = DecideCurrentWeapon(BattleRealPlayerManager.Instance.IsNormalWeapon, BattleRealPlayerManager.Instance.IsLaserType);
        m_CurrentWeapon = initialWeaponType;
        switch(DecideCurrentWeapon(BattleRealPlayerManager.Instance.IsNormalWeapon, BattleRealPlayerManager.Instance.IsLaserType)){
                case E_WEAPON_TYPE.NORMAL_SHOT:
                    m_OutText.text = "Shot";
                    break;
                case E_WEAPON_TYPE.LASER:
                    m_OutText.text = "Laser";
                    break;
                case E_WEAPON_TYPE.BOMB:
                    m_OutText.text = "Bomb";
                    break;
                default:
                    m_OutText.text = "ERROR";
                    break;
            }      
    }

    private E_WEAPON_TYPE DecideCurrentWeapon(bool isShot, bool isLaser){
        if(!isShot){
            if(isLaser){
                return E_WEAPON_TYPE.LASER;
            }else{
                return E_WEAPON_TYPE.BOMB;
            }
        }

        return E_WEAPON_TYPE.NORMAL_SHOT;
    }
}
