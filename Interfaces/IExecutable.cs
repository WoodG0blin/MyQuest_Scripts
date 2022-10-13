using System;

namespace WG_Game
{
    public interface IExecutable
    {
        ExecuteType ExType { get; }
        //void Awake();
        //void Update();
        //void FixedUpdate();

        Action GetExecute(ExecuteType t);
    }
}
