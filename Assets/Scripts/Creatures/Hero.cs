using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        [SerializeField] private float _slamDownVelocity = .8f;

        private bool _allowDoubleJump;

        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [SerializeField] private ParticleSystem _coinHitParticles;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        private GameSession _session;

        private static readonly int throwKey = Animator.StringToHash("throw");

        [SerializeField] private Cooldown _throwCooldown;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("Slamdown");
                }
            }
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!_isGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpPower;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        protected override float CalculateYVelocity()
        {
            if (_isGrounded)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }

        public void SaySomething()
        {
            Debug.Log("Hello Pixel Crew!");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        public void Interact()
        {
            _interactionCheck.Check();
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

        public override void AttackAnimation()
        {
            if (!_session.Data.IsArmed) return;

            base.AttackAnimation();
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

        public void ThrowAnimation()
        {
            if (_throwCooldown.IsReady)
            {
                _animator.SetTrigger(throwKey);
                _throwCooldown.Reset();
            }
        }

        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
        }
    }
}