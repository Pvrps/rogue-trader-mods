using Kingmaker.Code.UI.MVVM;
using Kingmaker.Code.UI.MVVM.VM.ServiceWindows;

namespace Purps.RogueTrader.API.Menu
{
    public static class RTMenu
    {
        public static bool IsOpen()
        {
            return RootUIContext.Instance.IsBlockedFullScreenUIType()
            || RootUIContext.Instance.CurrentServiceWindow != ServiceWindowsType.None
            || RootUIContext.Instance.FullScreenUIType != Kingmaker.UI.Models.FullScreenUIType.Unknown
            || RootUIContext.Instance.GroupChangerIsShown
            || RootUIContext.Instance.IsInventoryShow
            || RootUIContext.Instance.IsMainMenu
            || RootUIContext.Instance.IsLoadingScreen
            || RootUIContext.Instance.IsCharInfoAbilitiesChooseMode
            || RootUIContext.Instance.IsCharInfoLevelProgression;
        }
    }
}