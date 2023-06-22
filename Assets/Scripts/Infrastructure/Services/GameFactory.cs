using Constants;
using UnityEngine;

namespace Infrastructure.Services
{
    public class GameFactory : IService
    {
        private readonly AssetProvider _assetProvider;

        public GameFactory(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        public void CreateHero(GameObject at)
        {
            GameObject hero = _assetProvider.Instantiate(AssetPath.HeroPath, at: at.transform.position);
        }
    }
}