using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace N.PopupSystems
{
    public abstract class PopupHandlerBase : MonoBehaviour
    {
        [Header("Popup Handler Base (Stack)")]
        [SerializeField] protected GameObject objectFade;
        [SerializeField] private Transform popupParent;
        [HideInInspector] public bool Initialized;

        private readonly Dictionary<PopupType, PopupBase> dictPopup = new();
        private readonly Stack<PopupBase> stackPopups = new();
        private bool isAnimating;


        protected virtual void Start()
        {
            Initialize();
            SetActive(objectFade, false);
            Initialized = true;
        }

        private void Initialize()
        {
            for (int i = 0, cnt = popupParent.childCount; i < cnt; i++)
            {
                if (!popupParent.GetChild(i).TryGetComponent<PopupBase>(out var popup)) continue;
                popup.Initialize(this);

                if (dictPopup.ContainsKey(popup.PopupType))
                    Debug.LogWarning($"[{name}] Duplicate popup type {popup.PopupType}, skipping.");
                else
                    dictPopup[popup.PopupType] = popup;
            }
        }

        private void SetActive(GameObject go, bool status)
        {
            if (go && go.activeSelf != status)
                go.SetActive(status);
        }

        public PopupBase GetPopupWithType(PopupType type)
            => dictPopup.TryGetValue(type, out var popup) ? popup : null;

        public T GetPopupWithType<T>(PopupType type) where T : PopupBase
            => dictPopup.TryGetValue(type, out var popup) ? (T)popup : null;

        public virtual async UniTask ShowPopupType(PopupType type, UnityAction cbAfterShow = null, UnityAction cbAfterHide = null, bool forceHide = true)
        {
            if (isAnimating)
            {
                Debug.LogWarning($"[{name}] ShowPopupType({type}) blocked: animating.");
                return;
            }

            if (!dictPopup.TryGetValue(type, out var popup))
            {
                Debug.LogWarning($"[{name}] ShowPopupType({type}) failed: no such popup.");
                return;
            }

            if (popup.PopupState is PopupState.Showing or PopupState.Opened)
            {
                Debug.LogWarning($"[{name}] ShowPopupType({type}) aborted: already showing.");
                return;
            }

            isAnimating = true;
            SetActive(objectFade, true);

            if (forceHide && stackPopups.Count > 0)
                await stackPopups.Peek().ForceHide();

            await popup.OnShow(cbAfterShow, cbAfterHide);
            stackPopups.Push(popup);
            isAnimating = false;
        }

        public virtual async UniTask HidePopupType(PopupType type, UnityAction cbAfterHide = null, bool forceShow = true)
        {
            if (isAnimating)
            {
                Debug.LogWarning($"[{name}] HidePopupType({type}) blocked: animating.");
                return;
            }

            if (!dictPopup.TryGetValue(type, out var popup))
            {
                Debug.LogWarning($"[{name}] HidePopupType({type}) failed: no such popup.");
                return;
            }

            if (popup.PopupState is PopupState.Hiding or PopupState.Closed)
            {
                Debug.LogWarning($"[{name}] HidePopupType({type}) aborted: not showing.");
                return;
            }

            isAnimating = true;

            if (stackPopups.Count <= 0 || stackPopups.Peek().PopupType != type)
                Debug.LogWarning($"[{name}] HidePopupType({type}): stack top mismatch.");
            else
                stackPopups.Pop();

            await popup.OnHide(cbAfterHide);

            if (forceShow && stackPopups.Count > 0)
                await stackPopups.Peek().ForceShow();

            SetActive(objectFade, stackPopups.Count > 0);
            isAnimating = false;
        }

        public async UniTask CloseAllPopups(UnityAction cbAfterAllHidden = null)
        {
            while (stackPopups.Count > 0)
            {
                var p = stackPopups.Pop();
                await p.ForceHide();
            }
            SetActive(objectFade, false);
            cbAfterAllHidden?.Invoke();
        }
    }
}