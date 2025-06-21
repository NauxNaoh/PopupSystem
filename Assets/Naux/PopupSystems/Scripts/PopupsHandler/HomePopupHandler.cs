using Cysharp.Threading.Tasks;

namespace N.PopupSystems
{
    public class HomePopupHandler : PopupHandlerBase
    {
        public static HomePopupHandler Instance { get; private set; }
        void Awake() => Instance = this;

        public static void ShowShop()
        {
            TopPopupHandler.Instance.ShowPopupType(PopupType.ShopViewPopup).Forget();
        }
    }
}