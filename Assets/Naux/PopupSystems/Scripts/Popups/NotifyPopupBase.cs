using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace N.PopupSystems
{
    public class NotifyPopupBase : PopupBase
    {
        [SerializeField] private TextMeshProUGUI tmpContent;
        [SerializeField] private TextMeshProUGUI tmpConfirm;
        [SerializeField] private Button btnConfirm;
        [SerializeField] private RectTransform[] layouts;

        UnityAction _confirmCB;

        protected override void SetPopupTypeInternal() => SetPopupType(PopupType.NotificationPopup);

        public void Setup(string content, UnityAction confirmCB, string confirmText)
        {
            tmpContent.text = content;
            tmpConfirm.text = confirmText;
            _confirmCB = confirmCB;
        }

        protected override void HandleBeforeShow()
        {
            RegisterEventButton(btnConfirm, OnClickConfirm);
            if (PopupState != PopupState.Closed) return;

            RegisterEventButton(btnConfirm, OnClickConfirm);
            _ = RefreshLayout();
        }

        protected override void HandleAfterHide()
        {
            RegisterEventButton(btnConfirm, null);
            _confirmCB = null;
        }

        async UniTask RefreshLayout()
        {
            await UniTask.NextFrame();
            UpdateCanvas(layouts);
        }

        void OnClickConfirm()
        {
            if (PopupState != PopupState.Opened) return;
            _confirmCB?.Invoke();
            _confirmCB = null;
            CloseSelf(null);
        }
    }
}