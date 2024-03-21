using UnityEngine;

namespace Roguelike
{
    [RequireComponent(typeof(Rigidbody))]
    public class Dropitem : MonoBehaviour
    {
        [SerializeField] private Item data;
        [SerializeField] private int amount = 1;
        private const string targetTag = "Player";

        public bool connectToSpawn = false;

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                //Add Item
                try
                {
                    GameplayManager.Instance.AddItem(data, amount);
                    if (connectToSpawn)
                    {
                        SpawnItem.Instance.RemoveBox();
                    }
                }
                catch (System.Exception)
                {
                    print("Null");
                }         
                Destroy(gameObject);
            }
        }

    }
}
