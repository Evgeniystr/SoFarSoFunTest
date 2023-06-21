using SoFarSoFun.Data;
using SoFarSoFun.Pool;
using UnityEngine;

namespace SoFarSoFun.Services
{
    public class BallMaterialsService
    {
        private GameConfig _gameConfig;
        private Material _originalMaterial;
        private Material[] _colorMaterials;
        private TransitionMaterialsPool _transitionMaterialsPool;

        public BallMaterialsService(GameConfig gameConfig, Material originalMaterial) 
        {
            _gameConfig = gameConfig;
            _colorMaterials = new Material[gameConfig.Colors.Length];
            _originalMaterial = originalMaterial;

            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < _gameConfig.Colors.Length; i++)
            {
                _colorMaterials[i] = new Material(_originalMaterial);
                _colorMaterials[i].color = _gameConfig.Colors[i];
            }

            _transitionMaterialsPool = new TransitionMaterialsPool(_originalMaterial);
        }

        public Material GetRandomColorMaterial(Material exception = null)
        {
            var matIndex = Random.Range(0, _colorMaterials.Length);

            Material result;

            if(_colorMaterials[matIndex] != exception)
            {
                result = _colorMaterials[matIndex];
            }
            else
            {

                if(matIndex + 1 < _colorMaterials.Length)
                    result = _colorMaterials[matIndex + 1];
                else
                    result = _colorMaterials[0];
            }

            return result;
        }

        public Material GetTransitionMaterial()
        {
            return _transitionMaterialsPool.Get();
        }

        public void ReleaseTransitionMaterial(Material material)
        {
            _transitionMaterialsPool.ReleaseItem(material);
        }
    }
}