using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace N.PopupSystems
{
    public class SlidePopupAnimation : IPopupAnimation
    {
        readonly CanvasGroup cg;
        readonly RectTransform rt;
        readonly Vector2 offset;
        readonly float duration;
        readonly Ease ease;
        readonly PopupAnimationType type;

        public SlidePopupAnimation(CanvasGroup cg, PopupAnimationConfig cfg)
        {
            this.cg = cg;
            this.rt = cg.GetComponent<RectTransform>();
            this.offset = cfg.Offset;
            this.duration = cfg.Duration;
            this.ease = cfg.EaseType;
            this.type = cfg.Type;
        }

        public async UniTask PlayAppear()
        {
            rt.localPosition = Vector3.zero;
            cg.alpha = 1;

            Vector3 start = type switch
            {
                PopupAnimationType.SlideFromTop => new Vector3(0, +offset.y, 0),
                PopupAnimationType.SlideFromBottom => new Vector3(0, -offset.y, 0),
                PopupAnimationType.SlideFromLeft => new Vector3(-offset.x, 0, 0),
                PopupAnimationType.SlideFromRight => new Vector3(+offset.x, 0, 0),
                _ => Vector3.zero
            };

            rt.localPosition = start;
            await rt.DOLocalMove(Vector3.zero, duration).SetEase(ease).ToUniTask();
        }

        public async UniTask PlayDisappear()
        {
            Vector3 end = type switch
            {
                PopupAnimationType.SlideFromTop => new Vector3(0, +offset.y, 0),
                PopupAnimationType.SlideFromBottom => new Vector3(0, -offset.y, 0),
                PopupAnimationType.SlideFromLeft => new Vector3(-offset.x, 0, 0),
                PopupAnimationType.SlideFromRight => new Vector3(+offset.x, 0, 0),
                _ => Vector3.zero
            };

            await rt.DOLocalMove(end, duration).SetEase(ease).ToUniTask();
        }
    }
}