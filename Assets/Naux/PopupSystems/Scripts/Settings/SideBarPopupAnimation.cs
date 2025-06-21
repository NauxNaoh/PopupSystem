using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace N.PopupSystems
{
    public enum SideBarType { Top, Bottom, Left, Right, Custom }

    [Serializable]
    public class SideBarPopup
    {
        [Header("SideBar Settings")]
        public SideBarType sideBarType;
        public RectTransform rtSideBar;
    }

    [Serializable]
    public class SideBarPopupAnimation : IPopupAnimation
    {
        private readonly CanvasGroup cg;
        private readonly SideBarPopup[] sideBars;
        private readonly float duration;
        private readonly Ease ease;

        public SideBarPopupAnimation(CanvasGroup cg, PopupAnimationConfig cfg)
        {
            this.cg = cg;
            this.sideBars = cfg.SideBars;
            this.duration = cfg.Duration;
            this.ease = cfg.EaseType;
        }

        public async UniTask PlayAppear()
        {
            cg.alpha = 1;
            var tasks = new List<UniTask>();

            foreach (var s in sideBars)
            {
                var rt = s.rtSideBar;
                if (rt == null) continue;

                float off = s.sideBarType switch
                {
                    SideBarType.Top => rt.rect.height,
                    SideBarType.Bottom => -rt.rect.height,
                    SideBarType.Left => -rt.rect.width,
                    SideBarType.Right => rt.rect.width,
                    _ => 0
                };

                rt.anchoredPosition = s.sideBarType is SideBarType.Top or SideBarType.Bottom ? new(rt.anchoredPosition.x, off) : new(off, rt.anchoredPosition.y);

                var seq = DOTween.Sequence();
                _ = s.sideBarType is SideBarType.Top or SideBarType.Bottom
                    ? seq.Append(rt.DOAnchorPosY(0, duration).SetEase(ease))
                    : seq.Append(rt.DOAnchorPosX(0, duration).SetEase(ease));

                tasks.Add(seq.ToUniTask());
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask PlayDisappear()
        {
            var tasks = new List<UniTask>();

            foreach (var s in sideBars)
            {
                var rt = s.rtSideBar;
                if (rt == null) continue;

                float off = s.sideBarType switch
                {
                    SideBarType.Top => rt.rect.height,
                    SideBarType.Bottom => -rt.rect.height,
                    SideBarType.Left => -rt.rect.width,
                    SideBarType.Right => rt.rect.width,
                    _ => 0
                };

                var seq = DOTween.Sequence();
                if (s.sideBarType is SideBarType.Top or SideBarType.Bottom)
                    _ = seq.Append(rt.DOAnchorPosY(off, duration).SetEase(ease));
                else
                    _ = seq.Append(rt.DOAnchorPosX(off, duration).SetEase(ease));

                tasks.Add(seq.ToUniTask());
            }

            await UniTask.WhenAll(tasks);
            cg.alpha = 0;
        }
    }
}