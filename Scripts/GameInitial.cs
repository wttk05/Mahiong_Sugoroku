using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitial : MonoBehaviour
{
    // game‚Ì‰ð‘œ“x‚ðŒÅ’è‚µ‚Ä‚¢‚é
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(896, 504, false, 30);
    }
}
