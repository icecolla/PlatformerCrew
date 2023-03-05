using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew
{
    public class RandomSpawner : MonoBehaviour
    {
        [Header("Spawn bound:")]
        [SerializeField] private float _sectorAngle = 60;
        [SerializeField] private float _sectorRotation;

        //[Header("Spawn params:")]
        //[Space]
        //[SerializeField] private GameObject _particle;

        [SerializeField] private float _waitTime = 0.1f;
        [SerializeField] private float _speed = 6;
        //[SerializeField] private float _itemPerBurst = 2;
        //[SerializeField] private float _numParticles = 200;

        //private void Start()
        //{
        //    Restart();
        //}

        private Coroutine _routine;

        private void OnDisable()
        {
            TryStopRoutine();
        }

        private void OnDestroy()
        {
            TryStopRoutine();
        }

        public void DropImidiate(GameObject[] items)
        {
            foreach (var item in items)
            {
                Spawn(item);
            }
        }

        private void TryStopRoutine()
        {
            if (_routine != null)
                StopCoroutine(_routine);
        }

        //[ContextMenu("Restart")]
        //public void Restart()
        //{
        //    TryStopRoutine();

        //    _routine = StartCoroutine(StartSpawn());
        //}

        public void StartDrop(GameObject[] items)
        {
            TryStopRoutine();

            _routine = StartCoroutine(StartSpawn(items));
        }

        //private IEnumerator StartSpawn()
        //{
        //    for (var i = 0; i < _numParticles; i++)
        //    {
        //        for (var j = 0; j < _itemPerBurst; j++)
        //        {
        //            Spawn();
        //        }
        //        yield return new WaitForSeconds(_waitTime);
        //    }
        //}

        private IEnumerator StartSpawn(GameObject[] particles)
        {
            for (var i = 0; i < particles.Length; i++)
            {
                Spawn(particles[i]);

                //for (var j = 0; j < _itemPerBurst && i < particles.Length; j++)
                //{
                //    Spawn(particles[i]);
                //    i++;
                //}

                yield return new WaitForSeconds(_waitTime);
            }
        }

        //[ContextMenu("Spawn one")]
        //private void Spawn()
        //{
        //    var instance = Instantiate(_particle, transform.position, Quaternion.identity);
        //    var rigidBody = instance.GetComponent<Rigidbody2D>();

        //    var randomAngle = Random.Range(0, _sectorAngle);
        //    var forceVector = AngleToVectorInSector(randomAngle);
        //    rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        //}

        private void Spawn(GameObject particle)
        {
            var instance = Instantiate(particle, transform.position, Quaternion.identity);
            var rigidBody = instance.GetComponent<Rigidbody2D>();

            var randomAngle = Random.Range(0, _sectorAngle);
            var forceVector = AngleToVectorInSector(randomAngle);
            rigidBody.AddForce(forceVector * _speed, ForceMode2D.Impulse);
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;

            var middleAngleDelta = (180 - _sectorRotation - _sectorAngle) / 2;

            var rightBound = GetUnitOnCircle(middleAngleDelta);
            Handles.DrawLine(position, position + rightBound);

            var leftBound = GetUnitOnCircle(middleAngleDelta + _sectorAngle);
            Handles.DrawLine(position, position + leftBound);
            Handles.DrawWireArc(position, Vector3.forward, rightBound, _sectorAngle, _sectorRotation);

            Handles.color = new Color(1f, 1f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.forward, rightBound, _sectorAngle, _sectorRotation);
        }

        private Vector2 AngleToVectorInSector(float angle)
        {
            var angleMiddleDelta = (180 - _sectorRotation - _sectorAngle) / 2;
            return GetUnitOnCircle(angle + angleMiddleDelta);
        }

        private Vector3 GetUnitOnCircle(float angleDegrees)
        {
            var angleRadians = angleDegrees * Mathf.PI / 180.0f;

            var x = Mathf.Cos(angleRadians);
            var y = Mathf.Sin(angleRadians);

            return new Vector3(x, y, 0);
        }
    }
}
