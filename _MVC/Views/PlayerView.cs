using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class PlayerView : SceneObjectView, ICollect
    {
        public event Action<InteractionResult> OnCollect;
        public void Collect(InteractionResult result)
        {
            OnCollect?.Invoke(result);
        }
    }
}
