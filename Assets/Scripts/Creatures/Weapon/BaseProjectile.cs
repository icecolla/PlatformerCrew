using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed = 1f;

        protected Rigidbody2D _rb;
        protected int _direction;

        protected virtual void Start()
        {
            _direction = transform.lossyScale.x > 0 ? 1 : -1;
            _rb = GetComponent<Rigidbody2D>();
        }
    }
}
