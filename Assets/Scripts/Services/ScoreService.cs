using DG.Tweening;
using SoFarSoFun.Data;
using System;

namespace SoFarSoFun.Services
{
    public class ScoreService
    {
        public Action<int, int> OnScoreUpdate;

        private int _hiScore;
        private int _currentScore;
        private float _comboTimeDuration;

        private float _comboTimer;
        private Tween _timerTween;

        public ScoreService(GameConfig gameConfig) 
        {
            _comboTimeDuration = gameConfig.ComboTime;
        }

        public void AddPoint()
        {
            _currentScore++;

            if(_currentScore > _hiScore)
                _hiScore = _currentScore;

            OnScoreUpdate?.Invoke(_currentScore, _hiScore);
            RunTimer();
        }
        
        private void RunTimer()
        {
            if(_timerTween != null)
                _timerTween.Kill();

            _timerTween = DOTween.To(() => _comboTimer, (val) => _comboTimer = val, _comboTimeDuration, _comboTimeDuration).
                OnComplete(() =>
                {
                    _currentScore = 0;
                    OnScoreUpdate?.Invoke(_currentScore, _hiScore);
                });
        }
    }
}