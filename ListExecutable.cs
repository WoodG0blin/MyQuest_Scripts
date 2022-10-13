using System;

namespace WG_Game
{
    public enum ExecuteType { Graphics = 0, Physics = 1 , Both = 2, None = 3}
    public class ListExecutable
    {
        private IExecutable[] _update;
        private IExecutable[] _fixedupdate;

        private Action _graphics;
        private Action _physics;

        public ListExecutable()
        {
            _update = new IExecutable[0];
            _fixedupdate = new IExecutable[0];
            _graphics = () => { };
            _physics = () => { };
        }

        //public IExecutable[] Graphics { get { return _update; } }
        //public IExecutable[] Physics { get { return _fixedupdate; } }

        public void Add(IExecutable ex)
        {
            switch(ex.ExType)
            {
                case ExecuteType.Graphics: _graphics += ex.GetExecute(ExecuteType.Graphics); break;
                case ExecuteType.Physics: _physics += ex.GetExecute(ExecuteType.Physics); break;
                case ExecuteType.Both: _graphics += ex.GetExecute(ExecuteType.Graphics); _physics += ex.GetExecute(ExecuteType.Physics); break;
                default: break;
            }

            //if(t == 0)
            //{
            //    Array.Resize(ref _update, _update.Length+1);
            //    _update[_update.Length] = ex;
            //}
            //else
            //{
            //    Array.Resize(ref _fixedupdate, _fixedupdate.Length + 1);
            //    _fixedupdate[_fixedupdate.Length] = ex;
            //}
        }

        public void Remove(IExecutable ex)
        {
            switch (ex.ExType)
            {
                case ExecuteType.Graphics: _graphics -= ex.GetExecute(ExecuteType.Graphics); break;
                case ExecuteType.Physics: _physics -= ex.GetExecute(ExecuteType.Physics); break;
                case ExecuteType.Both: _graphics -= ex.GetExecute(ExecuteType.Graphics); _physics += ex.GetExecute(ExecuteType.Physics); break;
                default: break;
            }
        }

        public void Execute(ExecuteType t)
        {
            if (t == ExecuteType.Graphics) _graphics();
            if (t == ExecuteType.Physics) _physics();
        }
    }
}
