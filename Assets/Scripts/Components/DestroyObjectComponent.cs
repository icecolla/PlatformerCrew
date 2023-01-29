using UnityEngine;

namespace PixelCrew.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objToDestroy;

        public void DestroyObject()
        {
            Destroy(_objToDestroy);
        }
    }
}