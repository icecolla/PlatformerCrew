using UnityEngine;

namespace PixelCrew.Components
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layer;
        [SerializeField] protected bool _isTouchingLayer;
        public bool IsTouchingLayer => _isTouchingLayer;
    }
}