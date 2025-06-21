using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace N.PopupSystems
{
    public abstract class NoticeUIBase : MonoBehaviour
    {
        [Header("Notice Base UI")]
        [SerializeField] private bool activeNotice;
        [SerializeField] private Image imgNotice;
        [HideInInspector] public bool hasNotice;

        public UnityAction OnActionCheckNotice;

        protected virtual void OnEnable()
        {
            OnActionCheckNotice = OnNoticeChecking;
        }

        private void OnNoticeChecking()
        {
            hasNotice = activeNotice && IsValidNoticeCondition();
            if (imgNotice != null)
                imgNotice.gameObject.SetActive(hasNotice);
        }

        protected abstract bool IsValidNoticeCondition();
    }
}