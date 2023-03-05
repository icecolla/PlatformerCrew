using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class SeashellAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;

        [Header("Bite Attack")]
        [SerializeField] private Cooldown _biteCooldown;
        [SerializeField] private CheckCircleOverlap _biteAttack;
        [SerializeField] private LayerCheck _biteCanAttack;

        [Header("Fire Attack")]
        [SerializeField] private Cooldown _fireCooldown;
        [SerializeField] private SpawnComponent _fireAttack;

        private Animator _animator;
        private static readonly int isHitKey = Animator.StringToHash("hit");
        private static readonly int isBiteKey = Animator.StringToHash("bite");
        private static readonly int isFireKey = Animator.StringToHash("fire");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_biteCanAttack.IsTouchingLayer)
                {
                    if (_biteCooldown.IsReady)
                    {
                        BiteAttack();
                    }

                    return;
                }

                if (_fireCooldown.IsReady)
                {
                    FireAttack();
                }
            }
        }

        private void BiteAttack()
        {
            _biteCooldown.Reset();
            _animator.SetTrigger(isBiteKey);
        }

        private void FireAttack()
        {
            _fireCooldown.Reset();
            _animator.SetTrigger(isFireKey);
        }

        public void OnBiteAttack()
        {
            _biteAttack.Check();
        }

        public void OnFireAttack()
        {
            _fireAttack.Spawn();
        }
    }
}
