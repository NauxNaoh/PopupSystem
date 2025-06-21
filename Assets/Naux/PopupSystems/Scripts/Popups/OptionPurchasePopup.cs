using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace N.PopupSystems
{
    public enum PopupPurchaseType { NormalConfirm, CurrencyPurchase, IAPPurchase, BothPurchase }

    public class OptionPurchasePopup : PopupBase
    {
        [Header("Option Purchase UI")]
        [SerializeField] private TextMeshProUGUI tmpContent;
        [SerializeField] private TextMeshProUGUI tmpCurrencyPrice;
        [SerializeField] private TextMeshProUGUI tmpPurchaseIAP;
        [SerializeField] private Image currencyIcon;

        [Header("Buttons")]
        [SerializeField] private Button btnConfirm;
        [SerializeField] private Button btnCancel;
        [SerializeField] private Button btnPurchaseCurrency;
        [SerializeField] private Button btnPurchaseIAP;
        [SerializeField] private Button btnClose;

        [Header("Layout Refresh")]
        [SerializeField] private RectTransform[] layouts;

        private UnityAction _confirmCB;
        private UnityAction _cancelCB;
        private UnityAction _currencyCB;
        private UnityAction _iapCB;

        protected override void SetPopupTypeInternal() => SetPopupType(PopupType.OptionPurchasePopup);

        public void Setup(string content, PopupPurchaseType mode, UnityAction confirmCB = null, UnityAction cancelCB = null, UnityAction currencyCB = null, UnityAction iapCB = null)
        {
            tmpContent.text = content;

            _confirmCB = confirmCB;
            _cancelCB = cancelCB;
            _currencyCB = currencyCB;
            _iapCB = iapCB;

            btnConfirm.gameObject.SetActive(mode == PopupPurchaseType.NormalConfirm);
            btnPurchaseCurrency.gameObject.SetActive(mode == PopupPurchaseType.CurrencyPurchase || mode == PopupPurchaseType.BothPurchase);
            btnPurchaseIAP.gameObject.SetActive(mode == PopupPurchaseType.IAPPurchase || mode == PopupPurchaseType.BothPurchase);
            btnCancel.gameObject.SetActive(mode != PopupPurchaseType.BothPurchase);  // each Normal, Currency, IAP always has Cancel
            btnClose.gameObject.SetActive(mode == PopupPurchaseType.BothPurchase);
        }

        protected override void HandleBeforeShow()
        {
            if (PopupState != PopupState.Closed) return;

            RegisterEventButton(btnConfirm, OnClickConfirm);
            RegisterEventButton(btnPurchaseCurrency, OnClickCurrency);
            RegisterEventButton(btnPurchaseIAP, OnClickIAP);
            RegisterEventButton(btnCancel, OnClickCancel);
            RegisterEventButton(btnClose, OnClickCancel);
            _ = RefreshLayout();
        }

        protected override void HandleAfterHide()
        {
            RegisterEventButton(btnConfirm, null);
            RegisterEventButton(btnPurchaseCurrency, null);
            RegisterEventButton(btnPurchaseIAP, null);
            RegisterEventButton(btnCancel, null);
            RegisterEventButton(btnClose, null);
            _confirmCB = null;
            _cancelCB = null;
            _currencyCB = null;
            _iapCB = null;
        }

        async UniTask RefreshLayout()
        {
            await UniTask.NextFrame();
            UpdateCanvas(layouts);
        }

        private void OnClickConfirm()
        {
            if (PopupState != PopupState.Opened) return;
            _confirmCB?.Invoke();
            _confirmCB = null;
            CloseSelf(null);
        }

        private void OnClickCurrency()
        {
            if (PopupState != PopupState.Opened) return;
            _currencyCB?.Invoke();
            _currencyCB = null;
            CloseSelf(null);
        }

        private void OnClickIAP()
        {
            if (PopupState != PopupState.Opened) return;
            _iapCB?.Invoke();
            _iapCB = null;
            CloseSelf(null);
        }

        private void OnClickCancel()
        {
            if (PopupState != PopupState.Opened) return;
            _cancelCB?.Invoke();
            _cancelCB = null;
            CloseSelf(null);
        }
    }
}