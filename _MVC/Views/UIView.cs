using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace WG_Game
{
    public class UIView : SceneObjectView
    {
        [SerializeField] TextMeshProUGUI _info;

        public override void Awake()
        {
            base.Awake();
            if (_info == null)
                if(!ViewTransform.TryGetComponent<TextMeshProUGUI>(out _info))
                    ViewTransform.Find("Value").TryGetComponent<TextMeshProUGUI>(out _info);
        }

        public void Display(string info) { _info.text = info; }
    }
}
