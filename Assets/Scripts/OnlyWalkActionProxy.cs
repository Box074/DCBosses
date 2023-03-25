using DCBossesMod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyWalkActionProxy : MonoBehaviour
{
    public static System.Action<OnlyWalkActionProxy> onLoad;
    void Awake()
    {
#if BUILD_HKMOD
        gameObject.AddComponent<OnlyWalkAction>();
#endif
    }
}
