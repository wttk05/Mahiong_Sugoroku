using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEnum : MonoBehaviour
{
    public enum HaiType
    {
        // ���q
        �ꓛ,
        ��,
        �O��,
        �l��,
        �ܓ�,
        �Ԍܓ�,
        �Z��,
        ����,
        ����,
        �㓛,
        // ���q
        ���,
        ���,
        �O��,
        �l��,
        �܍�,
        �Ԍ܍�,
        �Z��,
        ����,
        ����,
        ���,
        // �ݎq
        ����,
        ����,
        �O��,
        �l��,
        ����,
        �Ԍ���,
        �Z��,
        ����,
        ����,
        ����,
        // ���v
        ��,
        �,
        ��
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
        ���q,
        ���q,
        �ݎq,
        ���̑�
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


