using System;
using UnityEngine;

namespace PixelCrew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;
        private float _timesUp;

        public bool IsReady => _timesUp <= Time.time;

        public void Reset()
        {
            _timesUp = Time.time + _value;
        }
    }
}