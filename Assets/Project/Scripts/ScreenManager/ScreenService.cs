using System;
using UnityEngine;

public class ScreenService : MonoSingletongeneric<ScreenService>
{
    public BaseScreen[] allGameScreens;
    private BaseScreen currentActiveScreen;
    private void Start()
    {
        currentActiveScreen = allGameScreens[0];
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
            Debug.LogError("screenType Does not Exist");
        }
    }
}
