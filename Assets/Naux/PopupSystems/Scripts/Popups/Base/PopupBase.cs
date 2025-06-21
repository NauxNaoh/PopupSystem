using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace N.PopupSystems
{
    public abstract class PopupBase : MonoBehaviour
    {
        [Header("Popup Base")]
        [SerializeField] private CanvasGroup contentCG;
        [SerializeField] private PopupAnimationConfig animationConfig;

        private IPopupAnimation iPopupAnimation;
        private PopupHandlerBase popupHandler;
        protected UnityAction ClosedCB;

        public PopupType PopupType { get; private set; }
        public PopupState PopupState { get; private set; }

        public virtual void Initialize(PopupHandlerBase handler)
        {
            popupHandler = handler;
            SetPopupTypeInternal();
            iPopupAnimation = PopupAnimationFactory.Create(contentCG, animationConfig);

            SetActive(contentCG.gameObject, false);
            SetCanvasGroup(0, false, false);
            PopupState = PopupState.Closed;
        }

        protected abstract void HandleBeforeShow();
        protected abstract void HandleAfterHide();
        protected abstract void SetPopupTypeInternal();
        protected void SetPopupType(PopupType type) => PopupType = type;
        void SetActive(GameObject go, bool status) => go.SetActive(status);
        void SetCanvasGroup(float alpha, bool interact = true, bool blockRay = true)
        {
            if (contentCG == null)
            {
                Debug.LogWarning($"{nameof(contentCG)} is not assigned!");
                return;
            }

            contentCG.alpha = Mathf.Clamp01(alpha);
            contentCG.interactable = interact;
            contentCG.blocksRaycasts = blockRay;
        }

        protected void RegisterEventButton(Button btn, UnityAction act, bool clear = true)
        {
            if (!btn) return;
            if (clear) btn.onClick.RemoveAllListeners();
            if (act != null) btn.onClick.AddListener(act);
        }

        public async UniTask OnShow(UnityAction cbAfterShow, UnityAction closedCB)
        {
            PopupState = PopupState.Showing;
            SetCanvasGroup(0, false, false);
            SetActive(contentCG.gameObject, true);
            HandleBeforeShow();
            ClosedCB = closedCB;

            await iPopupAnimation.PlayAppear();
            SetCanvasGroup(1);
            PopupState = PopupState.Opened;
            cbAfterShow?.Invoke();
        }

        public async UniTask ForceShow()
        {
            PopupState = PopupState.Showing;
            SetCanvasGroup(0, false, false);
            SetActive(contentCG.gameObject, true);
            HandleBeforeShow();

            await iPopupAnimation.PlayAppear();
            SetCanvasGroup(1);
            PopupState = PopupState.Opened;
        }

        public async UniTask OnHide(UnityAction cbAfterHide)
        {
            PopupState = PopupState.Hiding;
            await iPopupAnimation.PlayDisappear();
            SetCanvasGroup(0, false, false);
            HandleAfterHide();
            SetActive(contentCG.gameObject, false);
            PopupState = PopupState.Closed;

            ClosedCB?.Invoke();
            ClosedCB = null;
            cbAfterHide?.Invoke();
        }

        public async UniTask ForceHide()
        {
            PopupState = PopupState.Hiding;
            await iPopupAnimation.PlayDisappear();
            SetCanvasGroup(0, false, false);
            HandleAfterHide();
            SetActive(contentCG.gameObject, false);
            PopupState = PopupState.Closed;
        }

        protected void CloseSelf(UnityAction cbAfterHide) => popupHandler.HidePopupType(PopupType, cbAfterHide).Forget();

        protected virtual void OnDestroy()
        {
            // remove all button listeners to avoid leaks
            if (contentCG == null) return;
            foreach (var btn in contentCG.GetComponentsInChildren<Button>())
                btn.onClick.RemoveAllListeners();
        }
    }
}