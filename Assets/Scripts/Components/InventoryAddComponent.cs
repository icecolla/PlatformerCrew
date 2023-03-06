using PixelCrew.Creatures;
//using PixelCrew.Model.Defenitions;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<Hero>();

            if (hero != null)
            {
                hero.AddInventory(_id, _count);
            }
        }
    }
}