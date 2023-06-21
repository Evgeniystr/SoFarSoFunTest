using SoFarSoFun.Data;
using SoFarSoFun.ViewModel;
using UnityEngine;
using Zenject;

namespace SoFarSoFun.Services
{
    public class PushBallService
    {
        private Transform _mainCameraRoot;
        private InputService _inputService;
        private Camera _camera;

        private const float _raycastDistance = 100f;
        private LayerMask _raycastMask;

        public PushBallService(
            [Inject(Id = Constants.MainCameraRoot)] Transform mainCameraRoot,
            InputService inputService,
            Camera camera)
        {
            _mainCameraRoot = mainCameraRoot;
            _inputService = inputService;
            _camera = camera;

            _inputService.OnLMBClick += Push;

            _raycastMask = LayerMask.GetMask(Constants.BallLayerName);
        }


        private void Push(Vector3 mousePos)
        {
            var ray = _camera.ScreenPointToRay(mousePos);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _raycastDistance, _raycastMask))
            {
                var ballVM = hit.collider.GetComponent<BallViewModel>();

                var direction = _camera.transform.forward;
                direction.y = 0;
                direction.Normalize();

                ballVM.Push(direction);
            }
        }
    }
}