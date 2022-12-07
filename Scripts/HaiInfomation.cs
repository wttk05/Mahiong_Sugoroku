using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiInfomation :MonoBehaviour
{
    public HaiAction HaiJudge(string str)
    {
        HaiAction action = new();

        // 数字と牌タイプの確認
        action.num = NumJudge(str);
        action.type= TypeJudge(str);
        action.isRed = RedJudge(str);

        return action;
    }

    int  NumJudge(string str)
    {
        int num = 0;

        if (str.IndexOf("一", 0) > -1)
        {
            num = 1;
        }
        else if(str.IndexOf("二", 0) > -1)
        {
            num = 2;
        }
        else if (str.IndexOf("三", 0) > -1)
        {
            num = 3;
        }
        else if (str.IndexOf("四", 0) > -1)
        {
            num = 4;
        }
        else if (str.IndexOf("五", 0) > -1)
        {
            num = 5;
        }
        else if (str.IndexOf("六", 0) > -1)
        {
            num = 6;
        }
        else if (str.IndexOf("七", 0) > -1)
        {
            num = 7;
        }
        else if (str.IndexOf("八", 0) > -1)
        {
            num = 8;
        }
        else if (str.IndexOf("九", 0) > -1)
        {
            num = 9;
        }

        return num;
    }

    TypeEnum.Type TypeJudge(string str)
    {
        TypeEnum.Type type = TypeEnum.Type.その他;

        if (str.IndexOf("筒", 0) >-1)
        {
            type = TypeEnum.Type.筒子;
        }
        else if (str.IndexOf("索", 0) > -1)
        {
            type = TypeEnum.Type.索子;
        }
        else if (str.IndexOf("萬", 0) > -1)
        {
            type = TypeEnum.Type.萬子;
        }

        return type;
    }

    bool RedJudge(string str)
    {
        bool isRed = false;

        if (str.IndexOf("赤", 0) > -1)
        {
            isRed = true;
        }

        return isRed;
    }
}
