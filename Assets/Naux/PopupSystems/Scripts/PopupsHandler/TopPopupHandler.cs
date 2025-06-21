namespace N.PopupSystems
{
    public class TopPopupHandler : PopupHandlerBase
    {
        public static TopPopupHandler Instance { get; private set; }
        void Awake() => Instance = this;
    }
}