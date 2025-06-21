using System;
using UnityEngine;
using UnityEngine.UI;

namespace N.PopupSystems
{
    public class MultiImageTargetGraphics : MonoBehaviour
    {
        [SerializeField] private Graphic[] targetGraphics;
        public Graphic[] GetTargetGraphics => targetGraphics;
    }
}