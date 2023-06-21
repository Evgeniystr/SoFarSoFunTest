using SoFarSoFun.Data;
using UnityEngine;
using Zenject;

namespace SoFarSoFun.Services
{
    public class CameraService
    {
        private Transform _mainCameraRoot;
        private InputService _inputService;

        float _rotationSensetivity = 1f;//

        public CameraService(
            [Inject(Id = Constants.MainCameraRoot)] Transform mainCameraRoot,
            InputService inputService)
        {
            _mainCameraRoot = mainCameraRoot;
            _inputService = inputService;

            _inputService.OnHorizontalMouseInput += Rotate;
        }

        private void Rotate(float HorizontalMousePos)
        {
            var ratationShift = HorizontalMousePos * _rotationSensetivity;
            _mainCameraRoot.Rotate(Vector3.up, ratationShift);
        }
    }
}