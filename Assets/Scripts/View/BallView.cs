using SoFarSoFun.ViewModel;
using UnityEngine;
using DG.Tweening;

namespace SoFarSoFun.View
{
    public class BallView : MonoBehaviour
    {
        [SerializeField]
        private BallViewModel _vm;
        [SerializeField]
        private MeshRenderer _meshRenderer;

        private Tween _transition;
        private const float _transitionDuration = 1f;

        private void Awake()
        {
            Initialize(_vm);
        }

        public void Initialize(BallViewModel vm)
        {
            _vm = vm;

            _vm.OnPositionSet += SetPosition;
            _vm.OnSetColor += SetColor;
            _vm.OnTransitionColor += StartTransition;
            _vm.OnCleanup += Cleanup;
        }

        public void Cleanup()
        {
            _vm.OnPositionSet -= SetPosition;
            _vm.OnSetColor -= SetColor;
            _vm.OnTransitionColor -= StartTransition;
            _vm.OnCleanup -= Cleanup;
        }

        private void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        private void SetColor(Material mat)
        {
            _meshRenderer.material = mat;
        }

        private void StartTransition(Material transitionMat, Color finishColor)
        {
            _transition.Kill();

            _meshRenderer.material = transitionMat;

            _transition = _meshRenderer.material.DOColor(finishColor, _transitionDuration)
                .OnComplete(() => _vm.ColorTransitionEnd(_meshRenderer.material));
        }
    }
}