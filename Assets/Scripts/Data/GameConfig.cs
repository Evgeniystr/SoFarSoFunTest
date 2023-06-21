using UnityEngine;

namespace SoFarSoFun.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "SoFarSoFun/GameConfigs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public Color[] Colors;
        public int BallsCount;
        public float PushForce;
        public float ComboTime;
    }
}