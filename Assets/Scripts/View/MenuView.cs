using SoFarSoFun.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SoFarSoFun.View
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _viewport;
        [SerializeField]
        private Button _startButton;
        [SerializeField]
        private Button _exitButton;

        [Inject]
        private GameService _gameService;
        [Inject]
        private InputService _inputService;

        private void Start()
        {
            _startButton.onClick.AddListener(StartGame);
            _exitButton.onClick.AddListener(Exit);
            _inputService.SetInputActive(false);
        }


        private void StartGame()
        {
            _gameService.StartNewGame();
            _inputService.SetInputActive(true);
            _viewport.SetActive(false);
        }

        private void Exit()
        {
            EditorApplication.isPlaying = false;
        }
    }
}