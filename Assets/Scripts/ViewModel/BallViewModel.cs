using SoFarSoFun.Data;
using SoFarSoFun.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoFarSoFun.ViewModel
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallViewModel : MonoBehaviour
    {
        public event Action<Vector3> OnPositionSet;
        public event Action<Material> OnSetColor;
        public event Action<Material, Color> OnTransitionColor;//transition mat, color-to
        public event Action OnCleanup;

        public Material BallMaterial => _ballMaterial;

        private List<BallViewModel> _collidedWith;

        private Rigidbody _rigidbody;
        private Material _ballMaterial;

        private bool _isSpawnComplete;
        private Vector3 _spawnPosition;
        private GameConfig _gameConfig;
        private BallMaterialsService _ballMaterialsService;
        private ScoreService _scoreService;

        public void Initialize(
            Vector3 spawnPosition, 
            BallMaterialsService ballMaterialsService,
            ScoreService scoreService,
            GameConfig gameConfig)
        {
            _spawnPosition = spawnPosition;
            _gameConfig = gameConfig;
            _ballMaterialsService = ballMaterialsService;
            _scoreService = scoreService;

            _collidedWith = new List<BallViewModel>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Setup();
        }

        private void FixedUpdate()
        {
            if(_collidedWith.Count > 0)
                _collidedWith.Clear();
        }

        public void Setup()
        {
            OnPositionSet.Invoke(_spawnPosition);

            _ballMaterial = _ballMaterialsService.GetRandomColorMaterial();
            OnSetColor.Invoke(_ballMaterial);
        }


        private void StartColorTransition(Material transitTo)
        {
            var transitionMat = _ballMaterialsService.GetTransitionMaterial();
            transitionMat.color = _ballMaterial.color;

            _ballMaterial = transitTo;

            OnTransitionColor?.Invoke(transitionMat, transitTo.color);
        }

        public void ColorTransitionEnd(Material usedTransitionMaterial)
        {
            _ballMaterialsService.ReleaseTransitionMaterial(usedTransitionMaterial);

            OnSetColor.Invoke(_ballMaterial);
        }

        public void Push(Vector3 direction)
        {
            _isSpawnComplete = true;
            var force = direction * _gameConfig.PushForce;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isSpawnComplete)
                return;

            var layerName = LayerMask.LayerToName(collision.gameObject.layer);
            switch (layerName)
            {
                case Constants.BallLayerName:
                    var otherBallVM = collision.gameObject.GetComponent<BallViewModel>();
                    if (!_collidedWith.Contains(otherBallVM))
                    {
                        _collidedWith.Add(otherBallVM);
                        _scoreService.AddPoint();

                        if (BallMaterial == otherBallVM.BallMaterial)
                        {
                            var newColor = _ballMaterialsService.GetRandomColorMaterial(_ballMaterial);
                            StartColorTransition(newColor);

                            var otherNewColor = _ballMaterialsService.GetRandomColorMaterial(otherBallVM.BallMaterial);
                            otherBallVM.RegisterOutcomeContact(this, otherNewColor);
                        }
                        else
                        {
                            var otherBallTransitTo = BallMaterial;
                            StartColorTransition(otherBallVM.BallMaterial);
                            otherBallVM.RegisterOutcomeContact(this, otherBallTransitTo);
                        }
                    }
                    break;
                case Constants.WallLayerName:
                    var newColorMat = _ballMaterialsService.GetRandomColorMaterial(_ballMaterial);
                    StartColorTransition(newColorMat);
                break;
            }
        }

        public void RegisterOutcomeContact(BallViewModel otherBallVM, Material transitTo)
        {
            StartColorTransition(transitTo);

            _collidedWith.Add(otherBallVM);
        }

        public void Cleanup()
        {
            OnCleanup.Invoke();
        }
    }
}