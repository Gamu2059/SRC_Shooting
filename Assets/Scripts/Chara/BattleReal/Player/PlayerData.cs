using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int m_Last {get; private set;}

    public PlayerData(int last){
        m_Last = last;
    }

    public PlayerData(){

    }

    public void SetLast(int last){
        m_Last = last;
    }

    public void IncreaseLast(){
        m_Last++;
    }

    public void DecreaseLast(){
        m_Last--;
    }
}
