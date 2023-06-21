using UnityEngine;

namespace SoFarSoFun.Pool
{
    public class TransitionMaterialsPool : APool<Material>
    {
        private Material _originalMaterial;

        public TransitionMaterialsPool(Material originalMaterial)
        {
            _originalMaterial = originalMaterial;
        }

        protected override Material CreateItem()
        {
            return new Material(_originalMaterial);
        }
    }
}