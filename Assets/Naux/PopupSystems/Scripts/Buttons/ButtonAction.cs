using UnityEngine;
using UnityEngine.UI;

namespace N.PopupSystems
{
    [RequireComponent(typeof(MultiImageButton))]
    public abstract class ButtonAction : NoticeUIBase
    {
        private Button btnClick;

        protected virtual void Awake()
        {
            btnClick = GetComponent<Button>();
            btnClick.onClick.RemoveAllListeners();
            btnClick.onClick.AddListener(OnClickedButton);
        }

        public abstract void OnClickedButton();
    }
}