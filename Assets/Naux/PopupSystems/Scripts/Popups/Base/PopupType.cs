namespace N.PopupSystems
{
    public enum PopupState { None, Showing, Opened, Hiding, Closed }

    public enum PopupType
    {
        None,
        NotificationPopup,
        ConfirmPopup,
        OptionPurchasePopup,
        SettingPopup,
        UserProfilePopup,

        HomeViewPopup,
        CustomViewPopup, 
        InventoryViewPopup,
        ShopViewPopup,
        QuestViewPopup,
        LootBoxPopup,
        
        TutorialPopup,
        GiftViewPopup,

        UpgradePopup,
    }
}