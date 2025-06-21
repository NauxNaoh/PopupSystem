using System;
using DG.Tweening;
using UnityEngine;

namespace N.PopupSystems
{
    public enum PopupAnimationType
    {
        Fade,
        ScaleUp, ScaleDown,
        SlideFromTop, SlideFromBottom, SlideFromLeft, SlideFromRight,
        SideBar
    }

    [Serializable]
    public class PopupAnimationConfig
    {
        [Header("For all animation")]
        public PopupAnimationType Type = PopupAnimationType.Fade;
        public float Duration = 0.15f;
        public Ease EaseType = Ease.Linear;

        [Header("For Slide Type")]
        public Vector2 Offset = new(2160, 1080);

        [Header("For SideBar Type")]
        public SideBarPopup[] SideBars;
    }
}
