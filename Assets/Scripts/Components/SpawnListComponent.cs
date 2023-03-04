using UnityEngine;
using System;
using System.Linq;

namespace PixelCrew.Components
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id);
            spawner?.Component.Spawn();

            // same as upper code
            //foreach(var data in _spawners)
            //{
            //    if (data.Id == id)
            //    {
            //        data.Component.Spawn();
            //        break;
            //    }
            //}
        }

        public void SpawnAll()
        {
            foreach (var spawnData in _spawners)
            {
                spawnData.Component.Spawn();
            }
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}