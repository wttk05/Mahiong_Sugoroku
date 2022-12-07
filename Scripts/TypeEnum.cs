using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEnum : MonoBehaviour
{
    public enum HaiType
    {
        // 筒子
        一筒,
        二筒,
        三筒,
        四筒,
        五筒,
        赤五筒,
        六筒,
        七筒,
        八筒,
        九筒,
        // 索子
        一索,
        二索,
        三索,
        四索,
        五索,
        赤五索,
        六索,
        七索,
        八索,
        九索,
        // 萬子
        一萬,
        二萬,
        三萬,
        四萬,
        五萬,
        赤五萬,
        六萬,
        七萬,
        八萬,
        九萬,
        // 字牌
        白,
        發,
        中
    }

    public enum TurnState
    {
        Player1,
        Player2,
        Player3,
        Player4,
        GameSet
    }

    public enum Type
    {
        筒子,
        索子,
        萬子,
        その他
    }

    public enum GamePhase
    {

    }

    public enum Connection
    {
        Online,
        Offline
    }
}


