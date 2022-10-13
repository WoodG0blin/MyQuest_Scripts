using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public interface ICollectable
    {
        public void Collect(Action<InteractionResult> result);
    }
}
