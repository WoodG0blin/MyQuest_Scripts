using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WG_Game
{
    public class EventManager : IExecutable
    {
        public ExecuteType ExType { get => ExecuteType.Graphics; }

        protected List<GameEvent> events;
        protected List<GameEvent> clear_events;
        public GameEvent this[int index] { get => events[index]; }

        public EventManager()
        {
            events = new List<GameEvent>();
            clear_events = new List<GameEvent>();
        }

        public Action GetExecute(ExecuteType type) { return Execute; }

        public void RegisterEvent(GameEvent _event)
        {
            events.Add(_event);
            _event.OnRemove += Remove;
        }

        protected void Execute()
        {
            foreach (GameEvent _event in events) _event.Process();
            foreach(GameEvent _event in clear_events) events.Remove(_event);
            clear_events.Clear();
        }

        protected void Remove(GameEvent e) { clear_events.Add(e); }
    }
}
