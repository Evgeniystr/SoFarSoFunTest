using SoFarSoFun.Data;
using SoFarSoFun.Services;
using SoFarSoFun.View;
using SoFarSoFun.ViewModel;
using UnityEngine;
using Zenject;

namespace SoFarSoFun.Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Transform _mainCameraRoot;
        [SerializeField]
        private Transform _floor;
        [SerializeField]
        private Transform _ballsParent;

        [SerializeField]
        private GameConfig _gameConfig;
        [SerializeField]
        private Material _originalBallMaterial;
        [SerializeField]
        private BallViewModel _ballPrefab;

        [SerializeField]
        private InputService _inputService;
        [SerializeField]
        private GameService _gameService;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(_camera).AsCached();
            Container.Bind<Transform>().WithId(Constants.MainCameraRoot).FromInstance(_mainCameraRoot).AsCached();
            Container.Bind<Transform>().WithId(Constants.Floor).FromInstance(_floor).AsCached();
            Container.Bind<Transform>().WithId(Constants.BallsParent).FromInstance(_ballsParent).AsCached();

            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsCached();
            Container.Bind<Material>().FromInstance(_originalBallMaterial).AsCached();
            Container.Bind<BallViewModel>().FromInstance(_ballPrefab).AsCached();

            Container.Bind<InputService>().FromInstance(_inputService).AsSingle();
            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<BallMaterialsService>().AsSingle();
            Container.Bind<PushBallService>().AsSingle().NonLazy();

            Container.Bind<CameraService>().AsSingle().NonLazy();
            Container.Bind<GameService>().FromInstance(_gameService).AsSingle();
        }
    }
}
