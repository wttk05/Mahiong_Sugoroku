using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitial : MonoBehaviour
{
    // game�̉𑜓x���Œ肵�Ă���
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(896, 504, false, 30);
    }
}
