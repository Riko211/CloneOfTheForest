using UnityEngine;

namespace Infrastructure.Services
{
    public class GameFactory : IService
    {
        private const string HeroPath = "Hero/hero";

        public void CreateHero(GameObject at)
        {
            GameObject hero = Instantiate(HeroPath, at: at.transform.position);
        }
        private static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
        private static GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }
    }
}