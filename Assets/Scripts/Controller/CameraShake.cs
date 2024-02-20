using Cinemachine;
using UnityEngine;

namespace Autophobia.PlayerComponents
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _playerCinemachine;
        [SerializeField] private float _frequency, _amplitude;
        
        private CinemachineBasicMultiChannelPerlin _shaker;
        private const byte _shakerOff = 0;
        
        private void Start()
        {
            _shaker = _playerCinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake(bool haveToShake)
        {
            _shaker.m_AmplitudeGain = haveToShake ? _amplitude : _shakerOff;
            _shaker.m_FrequencyGain = haveToShake ? _frequency : _shakerOff;
        }
    }
}