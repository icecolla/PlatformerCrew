using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private float _damageJumpPower = 1.5f;

        [SerializeField] private LayerCheck _groundCheck;
        private bool _isGrounded;
        private bool _allowDoubleJump;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private SpriteRenderer _sprite;
        private Animator _animator;
        private static readonly int isGroundKey = Animator.StringToHash("is-ground");
        private static readonly int isRunningKey = Animator.StringToHash("is-running");
        private static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int hitKey = Animator.StringToHash("hit");

        [SerializeField] private float _interactionRadius = 1f;
        [SerializeField] private LayerMask _interactionLayer;
        private Collider2D[] _interactResult = new Collider2D[1];

        private int _coins = 0;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();

            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(isRunningKey, _direction.x != 0);
            _animator.SetBool(isGroundKey, _isGrounded);
            _animator.SetFloat(verticalVelocityKey, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;

            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpPower;
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpPower;
                _allowDoubleJump = false;
            }

            return yVelocity;
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true;


            if (isJumpPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
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

        public void TakeDamage()
        {
            _animator.SetTrigger(hitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpPower);
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactResult, _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        public void AddCoins(int coins)
        {
            _coins += coins;

            Debug.Log($"{coins} coins added. Total coins {_coins}");
        }
    }
}