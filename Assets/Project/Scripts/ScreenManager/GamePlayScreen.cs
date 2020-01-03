using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PocketTanks.Screens
{
    public class GamePlayScreen : BaseScreen
    {
        public Slider powerSlider,angleSlider;
        public Button fireButton;

        public void DisableAllInput()
        {
            powerSlider.interactable = false;
            angleSlider.interactable = false;
            fireButton.interactable = false;
        }

        public void EnableAllInput()
        {
            powerSlider.interactable = true;
            angleSlider.interactable = true;
            fireButton.interactable = true;
        }

    }

}
