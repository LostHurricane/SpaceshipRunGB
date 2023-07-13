using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public enum UpdatePhase
    {
        Update,
        FixedUpdate,
        LateUpdate,
        PreRender,
        PostRender,
    }
}