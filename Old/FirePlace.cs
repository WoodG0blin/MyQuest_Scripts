using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG_Game
{
    public class FirePlace : Item
    {
        protected override void Start()
        {
            base.Start();
            type = Elements.Fire;
        }
    }
}
