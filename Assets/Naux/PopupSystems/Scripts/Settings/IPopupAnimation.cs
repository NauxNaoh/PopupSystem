using Cysharp.Threading.Tasks;

namespace N.PopupSystems
{
    public interface IPopupAnimation
    {
        UniTask PlayAppear();
        UniTask PlayDisappear();
    }
}