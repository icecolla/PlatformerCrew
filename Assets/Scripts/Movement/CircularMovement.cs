using UnityEngine;

namespace PixelCrew.Components.Movement
{
    public class CircularMovement : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 1f;

        private Vector2[] _position;
        private Rigidbody2D[] _bodies;
        private float _time;

        private void Awake()
        {
            UpdateContent();
        }

        private void UpdateContent()
        {
            _bodies = GetComponentsInChildren<Rigidbody2D>();
            _position = new Vector2[_bodies.Length];
        }

        private void Update()
        {
            CalculatePositions();
            var isAllDead = true;
            for (int i = 0; i < _bodies.Length; i++)
            {
                if (_bodies[i])
                {
                    _bodies[i].MovePosition(_position[i]);
                    isAllDead = false;
                }
            }

            if (isAllDead)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }

            _time += Time.deltaTime;
        }

        private void CalculatePositions()
        {
            var step = 2 * Mathf.PI / _bodies.Length;
            Vector2 containerPosition = transform.position;

            for (int i = 0; i < _bodies.Length; i++)
            {
                var angle = step * i;
                var pos = new Vector2(
                    Mathf.Cos(angle + _time * _speed) * _radius,
                    Mathf.Sin(angle + _time * _speed) * _radius
                    );
                _position[i] = containerPosition + pos;
            }
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateContent();
            CalculatePositions();

            for (int i = 0; i < _bodies.Length; i++)
            {
                _bodies[i].transform.position = _position[i];
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }
#endif
    }
}