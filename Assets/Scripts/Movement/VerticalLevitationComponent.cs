using UnityEngine;

namespace PixelCrew.Components.Movements
{
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] float _frequency = 1f;
        [SerializeField] float _amplitude = 1f;
        [SerializeField] private bool _randomize;

        private float _originalY;
        private Rigidbody2D _rb;
        private float _seed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _originalY = _rb.position.y;

            if (_randomize)
            {
                _seed = Random.value * Mathf.PI * 2;
            }
        }

        private void Update()
        {
            var position = _rb.position;
            position.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;
            _rb.MovePosition(position);
        }
    }
}