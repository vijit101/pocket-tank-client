using PocketTanks.Generics;
using System;
using UnityEngine;
namespace PocketTanks.Screens
{
    public class ScreenService : MonoSingletongeneric<ScreenService>
    {
        public BaseScreen[] allGameScreens;
        private BaseScreen currentActiveScreen;
        public BaseScreen GetActiveScreen { get { return currentActiveScreen; } }
        private void Start()
        {
            currentActiveScreen = allGameScreens[0];
            currentActiveScreen.gameObject.SetActive(true);
        }
        public void ChangeToScreen(ScreenType screenType)
        {
            BaseScreen screenToShow = Array.Find(allGameScreens, x => x.screenType.Equals(screenType));
            if (screenToShow != null)
            {
                currentActiveScreen.gameObject.SetActive(false);
                currentActiveScreen = screenToShow;
                currentActiveScreen.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogFormat("screenType Does not Exist or Screen type set to None or Add in Screen to ScreenService");
            }
        }

        public BaseScreen GetAnyBaseScreen(ScreenType screenType)
        {
            BaseScreen screenToReturn = Array.Find(allGameScreens, x => x.screenType.Equals(screenType));
            if (screenToReturn != null)
            {
                return screenToReturn;
            }
            else
            {
                Debug.LogFormat("screenType Does not Exist or Screen type set to None or Add in Screen to ScreenService");
                return null;
            }
        }
    }

}
