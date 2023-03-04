using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        protected override void Start()
        {
            base.Start();
        }

        private void FixedUpdate()
        {
            var position = _rb.position;
            position.x += _direction * _speed;
            _rb.MovePosition(position);
        }
    }
}