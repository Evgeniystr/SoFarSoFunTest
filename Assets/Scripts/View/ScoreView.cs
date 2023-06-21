using SoFarSoFun.Services;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

namespace SoFarSoFun.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _score;
        [SerializeField]
        private TMP_Text _hiScore;

        [Inject]
        private ScoreService _scoreService;

        // Start is called before the first frame update
        void Start()
        {
            _score.text = "0";
            _hiScore.text = "0";

            _scoreService.OnScoreUpdate += UpdateScore;
        }

        private void UpdateScore(int score, int hiScore)
        {
            _score.text = score.ToString();
            _hiScore.text = hiScore.ToString();
        }
    }
}