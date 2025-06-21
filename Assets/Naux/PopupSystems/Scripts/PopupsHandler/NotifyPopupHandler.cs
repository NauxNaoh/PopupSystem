using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace N.PopupSystems
{
    public class NotifyPopupHandler : PopupHandlerBase
    {
        private readonly Queue<NotifyData> _queue = new();
        private bool isProcessing;

        public static NotifyPopupHandler Instance { get; private set; }
        void Awake() => Instance = this;

        protected class NotifyData
        {
            public PopupType Type;
            public string Content;
            public UnityAction ConfirmCB;
            public UnityAction CancelCB;
            public string ConfirmText;
            public string CancelText;
            public UnityAction AfterShowCB;

            // OptionPurchase-specific
            public PopupPurchaseType PurchaseType;
            public UnityAction CurrencyCB;
            public UnityAction IAPCB;
        }

        public void ShowPopupNotification(string content, UnityAction confirmCB, string confirmText = "Ok", UnityAction cbAfterShow = null)
        {
            _queue.Enqueue(new NotifyData
            {
                Type = PopupType.NotificationPopup,
                Content = content,
                ConfirmCB = confirmCB,
                ConfirmText = confirmText,
                AfterShowCB = cbAfterShow
            });
            TryProcessQueue();
        }

        public void ShowPopupConfirm(string content, UnityAction confirmCB, UnityAction cancelCB, string confirmText = "Accept", string cancelText = "Cancel", UnityAction cbAfterShow = null)
        {
            _queue.Enqueue(new NotifyData
            {
                Type = PopupType.ConfirmPopup,
                Content = content,
                ConfirmCB = confirmCB,
                CancelCB = cancelCB,
                ConfirmText = confirmText,
                CancelText = cancelText,
                AfterShowCB = cbAfterShow
            });
            TryProcessQueue();
        }

        public void ShowPopupOptionPurchase(string content, PopupPurchaseType mode, UnityAction confirmCB, UnityAction cancelCB, UnityAction currencyCB, UnityAction iapCB, UnityAction cbAfterShow = null)
        {
            _queue.Enqueue(new NotifyData
            {
                Type = PopupType.OptionPurchasePopup,
                Content = content,
                PurchaseType = mode,
                ConfirmCB = confirmCB,
                CancelCB = cancelCB,
                CurrencyCB = currencyCB,
                IAPCB = iapCB,
                AfterShowCB = cbAfterShow
            });
            TryProcessQueue();
        }

        void TryProcessQueue()
        {
            if (!isProcessing)
                _ = ProcessQueue();
        }

        async UniTask ProcessQueue()
        {
            isProcessing = true;
            while (_queue.Count > 0)
            {
                var data = _queue.Dequeue();
                var popup = GetPopupWithType(data.Type);
                if (popup == null)
                {
                    Debug.LogWarning($"[Notify] No popup for {data.Type}");
                    continue;
                }

                //Set data
                switch (popup)
                {
                    case NotifyPopupBase n:
                        n.Setup(data.Content, data.ConfirmCB, data.ConfirmText);
                        break;
                    case ConfirmPopupBase c:
                        c.Setup(data.Content, data.ConfirmCB, data.CancelCB, data.ConfirmText, data.CancelText);
                        break;
                    case OptionPurchasePopup o:
                        o.Setup(data.Content, data.PurchaseType, data.ConfirmCB, data.CancelCB, data.CurrencyCB, data.IAPCB);
                        break;
                    default:
                        Debug.LogWarning($"[Notify] Unsupported popup type {data.Type}");
                        break;
                }

                await ShowPopupType(data.Type, data.AfterShowCB, cbAfterHide: null, forceHide: true);
                await UniTask.WaitUntil(() => GetPopupWithType(data.Type).PopupState == PopupState.Closed);
            }

            isProcessing = false;
        }
    }
}