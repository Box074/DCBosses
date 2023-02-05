using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailParryFsm : MonoBehaviour
{
    public static System.Action<NailParryFsm> onLoad;
    void Awake()
    {
        onLoad?.Invoke(this);
    }
}
