using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class SinusoidalProjectile : BaseProjectile
    {
        [SerializeField] float _frequency = 1f;
        [SerializeField] float _amplitude = 1f;

        private float _originalY;
        private float _time;

        protected override void Start()
        {
            base.Start();
            _originalY = _rb.position.y;
        }

        private void FixedUpdate()
        {
            var position = _rb.position;
            position.x += _direction * _speed;
            position.y = _originalY + Mathf.Sin(_frequency * _time) * _amplitude;
            _rb.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}
