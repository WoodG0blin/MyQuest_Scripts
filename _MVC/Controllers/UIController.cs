using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WG_Game
{
    public class UIController : Controller
    {
        private Button _restart;

        public enum DisplayParameter { Message = 0, Concentration = 1, GameOver = 100}
        private Dictionary<DisplayParameter, UIView> displays;

        public UIController(Transform mainUI) : base(mainUI)
        {
            _executeType = ExecuteType.None;
            Initiate(mainUI);
        }

        public UIController() : this(GameObject.Find("===UI").transform) { }

        private void Initiate(Transform mainUI)
        {
            _restart = mainUI.Find("Restart").GetComponent<Button>();
            _restart.onClick.AddListener(_gameController.RestartGame);
            _restart.gameObject.SetActive(false);

            displays = new Dictionary<DisplayParameter, UIView>();
            displays.Add(DisplayParameter.Message, mainUI.Find("Info").GetComponent<UIView>());
            displays.Add(DisplayParameter.Concentration, mainUI.Find("Concentration").GetComponent<UIView>());
            displays.Add(DisplayParameter.GameOver, mainUI.Find("GameOver").GetComponent<UIView>());
        }

        //public void DisplayConcentration(float val) { _concentration.Display($"{val:F0}"); }
        //public void DisplayInfo(string info) { _info.Display(info); }

        public void Display(DisplayParameter param, string value) { displays[param].Display(value); }

        public void SetDisplay(DisplayParameter param, UIView display)
        {
            if(displays.ContainsKey(param)) displays[param] = display;
            else displays.Add(param, display);
        }

        public void GameOver()
        {
            _restart.gameObject.SetActive(true);
            Display(DisplayParameter.GameOver, "GAME OVER!");
        }
    }
}
