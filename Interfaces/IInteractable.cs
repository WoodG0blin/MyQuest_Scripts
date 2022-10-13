using UnityEngine;
using System;

namespace WG_Game
{
    public interface IInteractable
    {
        bool IsRespondable { get; }
        void RequestResponse(Action<InteractionResult> SendResult);
    }
}
