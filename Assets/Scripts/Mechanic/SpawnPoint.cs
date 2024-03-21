using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SpawnPoint : MonoBehaviour
    {
        private void Start()
        {
            var player = FindObjectOfType<SimpleLook>();
            player.transform.position = transform.position;
        }
    }
}
