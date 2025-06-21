using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace N.PopupSystems
{
    public class ConfirmPopupBase : PopupBase
    {
        [SerializeField] private TextMeshProUGUI tmpContent;
        [SerializeField] private TextMeshProUGUI tmpConfirm;
        [SerializeField] private TextMeshProUGUI tmpCancel;
        [SerializeField] private Button btnConfirm;
        [SerializeField] private Button btnCancel;
        [SerializeField] private RectTransform[] layouts;

        UnityAction _confirmCB;
        UnityAction _cancelCB;

        protected override void SetPopupTypeInternal() => SetPopupType(PopupType.ConfirmPopup);

        public void Setup(string content, UnityAction confirmCB, UnityAction cancelCB, string confirmText, string cancelText)
        {
            tmpContent.text = content;
            tmpConfirm.text = confirmText;
            tmpCancel.text = cancelText;
            _confirmCB = confirmCB;
            _cancelCB = cancelCB;
        }

        protected override void HandleBeforeShow()
        {
            if (PopupState != PopupState.Closed) return;

            RegisterEventButton(btnConfirm, OnClickConfirm);
            RegisterEventButton(btnCancel, OnClickCancel);
            _ = RefreshLayout();
        }

        protected override void HandleAfterHide()
        {
            RegisterEventButton(btnConfirm, null);
            RegisterEventButton(btnCancel, null);
            _confirmCB = null;
            _cancelCB = null;
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

        void OnClickCancel()
        {
            if (PopupState != PopupState.Opened) return;
            _cancelCB?.Invoke();
            _cancelCB = null;
            CloseSelf(null);
        }
    }
}