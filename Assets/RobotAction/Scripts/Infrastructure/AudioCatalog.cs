using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Infrastructure
{
    [Serializable]
    public struct AudioInfo
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField, Range(0f, 1f)] private float _volume;

        public readonly AudioClip Clip => _clip;
        public readonly float Volume => _volume;

        public AudioInfo(AudioClip clip, float volume)
        {
            _clip = clip;
            _volume = volume;
        }
    }

    [CreateAssetMenu(fileName = "AudioCatalog", menuName = "Scriptable Objects/AudioCatalog")]
    public class AudioCatalog : ScriptableObject
    {
        [SerializeField] private AudioInfo[] _audioInfos;

        private Dictionary<string,AudioInfo> _audioInfoTableById;
        private bool _isSetUp;

        public void SetUpDictionary()
        {
            if (_isSetUp) return;

            _audioInfoTableById = new Dictionary<string,AudioInfo>(_audioInfos.Length);

            foreach (var audioInfo in _audioInfos)
            {
                if(audioInfo.Clip == null)
                {
                    Debug.LogWarning($"AudioCatalog: {name}内に未割り当てのAudioInfoが存在します");
                    continue;
                }

                if(!_audioInfoTableById.TryAdd(audioInfo.Clip.name,audioInfo))
                {
                    Debug.LogWarning($"AudioCatalog: {name}内でAudioClipの重複が起こっています");
                }
            }

            _isSetUp = true;
        }

        /// <summary>
        /// Clio名をIdとして、ClipとVolumeを内包したAudioInfoを取得します
        /// <br/>注意点として引数が不正な場合はnullを返せないため例外を投げます。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public AudioInfo GetInfoById(string id)
        {
            if(!_isSetUp)
            {
                throw new InvalidOperationException("AudioCatalog: SetUp前にGetInfoById(string)が実行されています。");
            }

            if(string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("AudioCatalog: GetInfoByIdの引数がnullもしくは空です",id);
            }

            if(!_audioInfoTableById.TryGetValue(id,out var info))
            {
                throw new ArgumentException("AudioCatalog: GetInfoByIdの引数のAudioは未登録です\nId: ", id);
            }

            return info;
        }
    }
}
