using RobotAction.Core.Audio;
using UnityEngine;

namespace RobotAction.Infrastructure
{
    public class AudioManager : MonoBehaviour,IAudioService
    {
        [SerializeField] private AudioSource _seSource;
        [SerializeField] private AudioCatalog _catalog;

        private void Awake()
        {
            _catalog.SetUpDictionary();
        }

        public void PlaySE(string seId)
        {
            if (string.IsNullOrEmpty(seId)) return;

            var info = _catalog.GetInfoById(seId);

            _seSource.PlayOneShot(info.Clip,info.Volume);
        }
    }
}
