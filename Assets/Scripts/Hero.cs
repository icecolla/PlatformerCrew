using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _jumpPower = 1f;

        [SerializeField] private LayerCheck _groundCheck;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private SpriteRenderer _sprite;
        private Animator _animator;
        private static readonly int isGroundKey = Animator.StringToHash("is-ground");
        private static readonly int isRunningKey = Animator.StringToHash("is-running");
        private static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

            var isJumping = _direction.y > 0;
            var isGrounded = IsGrounded();

            if (isJumping)
            {
                if (isGrounded && _rigidbody.velocity.y <= 0.1f)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                }

            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }

            _animator.SetBool(isRunningKey, _direction.x != 0);
            _animator.SetBool(isGroundKey, isGrounded);
            _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
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