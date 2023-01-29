using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _jumpPower = 1f;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        [SerializeField] private LayerCheck _groundCheck;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

            var isJumping = _direction.y > 0;
            if (isJumping)
            {
                if (IsGrounded() && _rigidbody.velocity.y <= 0.1f)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                }

            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }
        }

        private bool IsGrounded()
        {
            return _groundCheck.isTouchingLayer;
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void SaySomething()
        {
            Debug.Log("Hello Pixel Crew!");
        }
    }
}