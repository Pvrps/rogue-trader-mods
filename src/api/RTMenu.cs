using Kingmaker.Code.UI.MVVM;
using Kingmaker.Code.UI.MVVM.VM.ServiceWindows;

namespace Purps.RogueTrader.API.Menu
{
    public static class RTMenu
    {
        public static bool IsOpen()
        {
            return RootUIContext.Instance.IsBlockedFullScreenUIType();
        }
    }
}