using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiInfomation :MonoBehaviour
{
    public HaiAction HaiJudge(string str)
    {
        HaiAction action = new();

        // �����Ɣv�^�C�v�̊m�F
        action.num = NumJudge(str);
        action.type= TypeJudge(str);
        action.isRed = RedJudge(str);

        return action;
    }

    int  NumJudge(string str)
    {
        int num = 0;

        if (str.IndexOf("��", 0) > -1)
        {
            num = 1;
        }
        else if(str.IndexOf("��", 0) > -1)
        {
            num = 2;
        }
        else if (str.IndexOf("�O", 0) > -1)
        {
            num = 3;
        }
        else if (str.IndexOf("�l", 0) > -1)
        {
            num = 4;
        }
        else if (str.IndexOf("��", 0) > -1)
        {
            num = 5;
        }
        else if (str.IndexOf("�Z", 0) > -1)
        {
            num = 6;
        }
        else if (str.IndexOf("��", 0) > -1)
        {
            num = 7;
        }
        else if (str.IndexOf("��", 0) > -1)
        {
            num = 8;
        }
        else if (str.IndexOf("��", 0) > -1)
        {
            num = 9;
        }

        return num;
    }

    TypeEnum.Type TypeJudge(string str)
    {
        TypeEnum.Type type = TypeEnum.Type.���̑�;

        if (str.IndexOf("��", 0) >-1)
        {
            type = TypeEnum.Type.���q;
        }
        else if (str.IndexOf("��", 0) > -1)
        {
            type = TypeEnum.Type.���q;
        }
        else if (str.IndexOf("��", 0) > -1)
        {
            type = TypeEnum.Type.�ݎq;
        }

        return type;
    }

    bool RedJudge(string str)
    {
        bool isRed = false;

        if (str.IndexOf("��", 0) > -1)
        {
            isRed = true;
        }

        return isRed;
    }
}
