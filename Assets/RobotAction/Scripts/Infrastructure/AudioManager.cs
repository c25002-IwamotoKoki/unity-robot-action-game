using RobotAction.Core.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Infrastructure
{
    public class AudioManager : MonoBehaviour,IAudioService
    {
        [SerializeField] private AudioSource _seSource;
        [SerializeField] private AudioClip[] _clips;

        private Dictionary<string, AudioClip> _seTableById;

        private void Awake()
        {
            _seTableById = new Dictionary<string, AudioClip>(_clips.Length);

            foreach (var clip in _clips)
            {
                if (!_seTableById.TryAdd(clip.name,clip))
                {
                    Debug.LogWarning("ìØÇ∂AudioClip,Ç‡ÇµÇ≠ÇÕìØÇ∂ñºëOÇÃAudioClipÇ™ClipsÇ…ê›íËÇ≥ÇÍÇƒÇ¢Ç‹Ç∑");
                }
            }
        }

        public void PlaySE(string seId)
        {
            if (string.IsNullOrEmpty(seId)) return;

            if(_seTableById.TryGetValue(seId, out AudioClip clip))
            {
                _seSource.PlayOneShot(clip);
            }
        }
    }
}
