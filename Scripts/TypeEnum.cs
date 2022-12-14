using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEnum : MonoBehaviour
{
    public enum HaiType
    {
        // q
        κ,
        ρ,
        O,
        l,
        ά,
        Τά,
        Z,
        ΅,
        ͺ,
        γ,
        // υq
        κυ,
        ρυ,
        Oυ,
        lυ,
        άυ,
        Τάυ,
        Zυ,
        ΅υ,
        ͺυ,
        γυ,
        // δέq
        κδέ,
        ρδέ,
        Oδέ,
        lδέ,
        άδέ,
        Τάδέ,
        Zδέ,
        ΅δέ,
        ͺδέ,
        γδέ,
        // v
        ,
        α’,
        
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
        q,
        υq,
        δέq,
        »ΜΌ
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


