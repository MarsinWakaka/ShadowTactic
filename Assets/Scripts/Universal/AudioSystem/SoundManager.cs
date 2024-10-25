using System;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Universal.AudioSystem
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        [SerializeField] AudioSource bgmSource;
        [SerializeField] AudioSource sfxSource;

        /// <summary>
        /// 不知道是不是游戏开始运行就会加载全部资源，有必要优化下性能
        /// </summary>
        [Header("BGM列表")]
        [SerializeField] AudioData[] bgm;

        [Header("UI Sound列表")]
        [SerializeField] AudioData highlightUISFX;
        [SerializeField] AudioData chooseUISFX;
        [SerializeField] AudioData NextSceneAudio;

        public AudioSource BgmSource { get => bgmSource; set => bgmSource = value; }
        public AudioSource SfxSource { get => sfxSource; set => sfxSource = value; }

        public void PlayBGM(int index)
        {
            BgmSource.clip = bgm[index].clip;
            BgmSource.loop = true;
            BgmSource.Play();
        }

        public void PlayBGM()
        {
            PlayBGM(Random.Range(0, bgm.Length - 1));
        }

        public void PlaySFX(AudioSource source, AudioClip sfxAudio)
        {
            SfxSource.PlayOneShot(sfxAudio);
        }

        public void PlayUIEnterSFX()
        {
            PlaySFX(sfxSource, highlightUISFX);
        }

        public void PlayUIChooseSFX()
        {
            PlaySFX(sfxSource, chooseUISFX);
        }

        public void PlaySFX(AudioSource source, AudioClip[] sfxAudio)
        {
            source.PlayOneShot(sfxAudio[Random.Range(0, sfxAudio.Length - 1)]);
        }

        public void PlaySFX(AudioSource source, AudioData sfxAudio)
        {
            source.PlayOneShot(sfxAudio.clip, sfxAudio.volume);
        }

        /// <summary>
        /// 随机播放数组中的一个音频
        /// </summary>
        /// <param name="sfxAudio"></param>
        public void PlaySFX(AudioSource source, AudioData[] sfxAudio)
        {
            if (sfxAudio != null)
            {
                PlaySFX(source, sfxAudio[Random.Range(0, sfxAudio.Length - 1)]);
            }
            else
            {
                Debug.LogError("Audio data not found for the specified actionAudio: ");
            }
        }

        internal void PlayNextLevelSFX()
        {
            PlaySFX(sfxSource, NextSceneAudio);
        }

        private void OnDestroy()
        {
            // 关闭所有音频
            bgmSource.Stop();
            sfxSource.Stop();
        }
    }
}
