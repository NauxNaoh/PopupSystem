using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace N.PopupSystems
{
    public class ScalePopupAnimation : IPopupAnimation
    {
        readonly CanvasGroup cg;
        readonly Transform tf;
        readonly float duration;
        readonly Ease ease;
        readonly PopupAnimationType type;
        readonly Vector3 fromScale;
        readonly Vector3 toScale = Vector3.one;

        public ScalePopupAnimation(CanvasGroup cg, PopupAnimationConfig cfg)
        {
            this.cg = cg;
            this.tf = cg.transform;
            this.duration = cfg.Duration;
            this.ease = cfg.EaseType;
            this.type = cfg.Type;

            fromScale = type == PopupAnimationType.ScaleUp
                ? Vector3.one * 0.9f
                : Vector3.one * 1.1f;
        }

        public async UniTask PlayAppear()
        {
            tf.localScale = fromScale;
            cg.alpha = 1;
            await tf.DOScale(toScale, duration).SetEase(ease).ToUniTask();
        }

        public async UniTask PlayDisappear()
        {
            await tf.DOScale(fromScale, duration).SetEase(ease).ToUniTask();
        }
    }
}