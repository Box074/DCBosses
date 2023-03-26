#if BUILD_HKMOD
using DCBossesMod;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailParryFsm : MonoBehaviour
{
#if BUILD_HKMOD
    void Awake()
    {
        var fsm = gameObject.AddComponent<PlayMakerFSM>();
        fsm.SetFsmTemplate(DeadCellsBosses.fsm_nail_clash_tink);
    }
#endif
}
