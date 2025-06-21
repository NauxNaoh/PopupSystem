using UnityEngine;

namespace N.PopupSystems
{
    public static class PopupAnimationFactory
    {
        public static IPopupAnimation Create(CanvasGroup cg, PopupAnimationConfig cfg)
        {
            switch (cfg.Type)
            {
                case PopupAnimationType.Fade:
                default:
                    return new FadePopupAnimation(cg, cfg);
                case PopupAnimationType.ScaleUp:
                case PopupAnimationType.ScaleDown:
                    return new ScalePopupAnimation(cg, cfg);
                case PopupAnimationType.SlideFromTop:
                case PopupAnimationType.SlideFromBottom:
                case PopupAnimationType.SlideFromLeft:
                case PopupAnimationType.SlideFromRight:
                    return new SlidePopupAnimation(cg, cfg);
                case PopupAnimationType.SideBar:
                    return new SideBarPopupAnimation(cg, cfg);
            }
        }
    }
}