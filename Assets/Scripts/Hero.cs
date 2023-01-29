using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        private Vector2 _direction;

        private void Update()
        {
            Movement();
        }

        public void Movement()
        {
            if (_direction.magnitude > 0)
            {
                var delta = _direction * _speed * Time.deltaTime;
                transform.position += new Vector3(delta.x, delta.y,
                                                                                                    transform.position.z);
            }
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void SaySomething()
        {
            Debug.Log("Say something");
        }
    }
}