using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiData
{
    public GameObject haiPrefab; // 牌のモデル
    public TypeEnum.HaiType haiType;// 牌の種類
    public bool doraDisplay = false; // ドラ表示牌であるか
    public bool dora = false;　// ドラであるか
    public bool redDora = false; // 赤ドラであるか
    public int haiNum; // すごろくの何番目であるか
    public bool isObverse; // 表向きになっているか
    public TypeEnum.Type type;

    //　么九牌とか筒子萬子などの情報も格納したほうが良い可能性あり
}

public class HaiAction
{
    public int num;
    public TypeEnum.Type type;
    public bool isRed;
}
