using UnityEngine;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;
        private void Awake()
        {
            var bootstrapper = FindFirstObjectByType<GameBootstrapper>();

            if (bootstrapper != null) return;

            bootstrapper = Instantiate(BootstrapperPrefab);
        }
    }
}