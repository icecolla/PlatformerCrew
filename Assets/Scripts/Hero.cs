using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _jumpPower = 1f;
        [SerializeField] private float _damageJumpPower = 1.5f;
        [SerializeField] private float _slamDownVelocity = .8f;

        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerMask _groundLayer;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private Animator _animator;
        private static readonly int isGroundKey = Animator.StringToHash("is-ground");
        private static readonly int isRunningKey = Animator.StringToHash("is-running");
        private static readonly int verticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int hitKey = Animator.StringToHash("hit");
        private static readonly int deadHitKey = Animator.StringToHash("dead-hit");
        private static readonly int attack1Key = Animator.StringToHash("attack 1");

        [SerializeField] private float _interactionRadius = 1f;
        [SerializeField] private LayerMask _interactionLayer;
        private Collider2D[] _interactResult = new Collider2D[1];

        [Space][Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpGroundedParticle;
        [SerializeField] private SpawnComponent _jumpParticle;

        [SerializeField] private ParticleSystem _coinHitParticles;

        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] private int _damage = 1;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        private GameSession _session;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _jumpGroundedParticle.Spawn();
                }
            }
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;

            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpPower;
                _jumpParticle.Spawn();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpPower;
                _jumpParticle.Spawn();
                _allowDoubleJump = false;
            }

            return yVelocity;
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }
            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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
            _isJumping = false;
            _animator.SetTrigger(hitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpPower);

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        public void DeadHit()
        {
            GetComponent<PlayerInput>().enabled = false;
            _damageJumpPower = 0f;
            _animator.SetTrigger(deadHitKey);
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

        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }

        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;

            Debug.Log($"{coins} coins added. Total coins {_session.Data.Coins}");
        }

        private void SpawnCoins()
        {
            var numCoinsToSpawn = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToSpawn;

            var burst = _coinHitParticles.emission.GetBurst(0);
            burst.count = numCoinsToSpawn;
            _coinHitParticles.emission.SetBurst(0, burst);

            _coinHitParticles.gameObject.SetActive(true);
            _coinHitParticles.Play();
        }

        public void AttackAnimation()
        {
            if (!_session.Data.IsArmed) return;

            _animator.SetTrigger(attack1Key);
        }

        public void OnAttackRange()
        {
            var gos = _attackRange.GetObjectInRange();

            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();

                if (hp != null && go.CompareTag("Enemy"))
                {
                    hp.ModifyHealth(-_damage);
                }
            }
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }
    }
}