using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class GameEvent
    {
        public enum EventType { Interaction, Action }
        protected Controller _sender;
        protected Transform _target;
        protected EventType _type;

        protected bool isProcessing = false;

        public event Action<GameEvent> OnRemove;

        public Controller Sender { get => _sender; }
        public Transform Target { get => _target; }

        public GameEvent(EventType type, Controller sender, Transform target)
        {
            _type = type;
            _sender = sender;
            _target = target;
        }

        public void Process()
        {
            if (!isProcessing)
            {
                switch (_type)
                {
                    case GameEvent.EventType.Interaction:
                        {
                            Controller _control;
                            if (ControllerSpawner.TryGetController(Target.tag, out _control) && _control is IInteractable)
                            {
                                _control.transform = Target;
                                (_control as IInteractable).RequestResponse(ReceiveInteractionResult);
                                isProcessing = true;
                            }
                            break;
                        }
                    case GameEvent.EventType.Action:
                        {
                            break;
                        }
                }
                OnRemove(this);
            }
        }

        protected void ReceiveInteractionResult(InteractionResult result)
        {
            //if(result != null) result.Send(Sender);
            OnRemove(this);
        }
    }

    public static class ControllerSpawner
    {
        private static Dictionary<string, Controller> controllers = new Dictionary<string, Controller>();

        static ControllerSpawner()
        {
            controllers.Add("AnimatedItem", new AnimatedController(null));
            //controllers.Add("SpawnItem", new SpawnController(null));
            //controllers.Add("Elemental", new ElementalController(null));
        }

        public static bool TryGetController(string tag, out Controller controller)
        {
            if (controllers.ContainsKey(tag))
            {
                controller = controllers[tag];
                return true;
            }
            controller = null;
            return false;
        }
    }

}
