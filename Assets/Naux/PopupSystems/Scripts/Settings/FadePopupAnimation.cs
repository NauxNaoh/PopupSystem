using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace N.PopupSystems
{
    public class FadePopupAnimation : IPopupAnimation
    {
        private readonly CanvasGroup cg;
        private readonly float duration;
        private readonly Ease ease;

        public FadePopupAnimation(CanvasGroup cg, PopupAnimationConfig cfg)
        {
            this.cg = cg;
            this.duration = cfg.Duration;
            this.ease = cfg.EaseType;
        }

        public async UniTask PlayAppear()
        {
            cg.alpha = 0;
            await cg.DOFade(1f, duration).SetEase(ease).ToUniTask();
        }

        public async UniTask PlayDisappear()
        {
            await cg.DOFade(0f, duration).SetEase(ease).ToUniTask();
        }
    }
}