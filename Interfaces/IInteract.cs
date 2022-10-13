using UnityEngine;
using System;

namespace WG_Game
{
    public interface IInteract
    {
        bool IsRespondable { get; }
        void RequestInteraction(SceneObjectView sender, Action<InteractionResult> sendResult);
    }
}
