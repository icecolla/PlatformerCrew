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

        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.Data.Inventory.OnChanged += PrintHandlerDebug;

            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Hp);
            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
            _session.Data.Inventory.OnChanged -= PrintHandlerDebug;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
        }

        private void PrintHandlerDebug(string id, int value)
        {
            Debug.Log($"Inventory changed: {id}: {value}");
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

            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void AddInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        //public void AddCoins(int coins)
        //{
        //    _session.Data.Coins += coins;
        //    Debug.Log($"{coins} coins added. Total coins {_session.Data.Coins}");
        //}

        private void SpawnCoins()
        {
            var numCoinsToSpawn = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToSpawn);

            var burst = _coinHitParticles.emission.GetBurst(0);
            burst.count = numCoinsToSpawn;
            _coinHitParticles.emission.SetBurst(0, burst);

            _coinHitParticles.gameObject.SetActive(true);
            _coinHitParticles.Play();
        }

        public override void AttackAnimation()
        {
            if (SwordCount <= 0) return;

            base.AttackAnimation();
        }

        //public void ArmHero()
        //{
        //    _session.Data.IsArmed = true;
        //    UpdateHeroWeapon();
        //}

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
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