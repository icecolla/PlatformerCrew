using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        private Rigidbody2D _rb;
        private int _direction;

        private void Start()
        {
            _direction = transform.lossyScale.x > 0 ? 1 : -1;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var position = _rb.position;
            position.x += _direction * _speed;
            _rb.MovePosition(position);
        }
    }
}