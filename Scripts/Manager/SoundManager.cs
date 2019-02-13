using UnityEngine;
using System.Collections;
using System;

namespace GraduationProject
{
		public class SoundManager : MonoBehaviour {

		private AudioSource BGMsource;
		private AudioSource[] SEsources;

		private AudioClip[] BGM;
		private AudioClip[] SE;

		// Use this for initialization
		void Awake () {

			BGMsource = gameObject.AddComponent <AudioSource> ();
			BGMsource.loop = true;	

			SEsources = new AudioSource[15];
			for (int i=0; i<SEsources.Length; i++)
			{
				SEsources[i] = gameObject.AddComponent <AudioSource> ();
			}


			BGM = new AudioClip[4]{Resources.Load ("Sound/BGM/title", typeof (AudioClip)) as AudioClip, 
				Resources.Load ("Sound/BGM/gallery", typeof (AudioClip)) as AudioClip,
				Resources.Load ("Sound/BGM/stage01", typeof (AudioClip)) as AudioClip,
				Resources.Load ("Sound/BGM/stage02", typeof (AudioClip)) as AudioClip,
			};

			SE = new AudioClip[5]{Resources.Load ("Sound/SE/button", typeof(AudioClip)) as AudioClip,
				Resources.Load ("Sound/SE/change_select", typeof(AudioClip)) as AudioClip,
				Resources.Load ("Sound/SE/cancel4", typeof(AudioClip)) as AudioClip,
				Resources.Load ("Sound/SE/waring", typeof(AudioClip)) as AudioClip,
				Resources.Load ("Sound/BGM/syutujin", typeof(AudioClip)) as AudioClip,
			};
		}

		// Update is called once per frame
		void Update () {

		}

		public void SetCurrentSE (string[] filePath)
		{
			SE = new AudioClip[filePath.Length];

			for (int i=0; i<SE.Length; i++)
			{
				SE[i] = Resources.Load ("Sound/SE/"+filePath[i], typeof (AudioClip)) as AudioClip;
			}
		}

		public void PlayBGM (int index)
		{
			if( 0 > index || BGM.Length <= index ){
				return;
			}
			// 同じBGMの場合は何もしない
			if( BGMsource.clip == BGM[index] ){
				return;
			}
			BGMsource.Stop();

			BGMsource.clip = BGM[index];

			BGMsource.Play();
		}

		public void StopBGM()
		{
			BGMsource.Stop();

			BGMsource.clip = null;
		}

		public void PlaySE (int index)
		{
			if( 0 > index || SE.Length <= index )
				return;
			
			// 再生中で無いAudioSouceで鳴らす
			foreach (AudioSource source in SEsources)
			{
				if(!source.isPlaying)
				{
					source.clip = SE[index];
					source.Play();
					break;
				} 
			}
		}

		public void PlaySEOnce (int index)
		{
			if( 0 > index || SE.Length <= index )
				return;

			if (!SEsources[SEsources.Length-1].isPlaying)
			{
				SEsources[SEsources.Length-1].clip = SE[index];
				SEsources[SEsources.Length-1].Play();
			}
		}

		public void PlayHitSE ()
		{
			for (int i=SEsources.Length-5; i<SEsources.Length; i++)
			{
				if(!SEsources[i].isPlaying)
				{
					SEsources[i].clip = SE[SE.Length-1];
					SEsources[i].Play();
					break;
				} 
			}
		}

		public void PlaySE (AudioClip clip)
		{		
			// 再生中で無いAudioSouceで鳴らす
			foreach (AudioSource source in SEsources)
			{
				if(!source.isPlaying)
				{
					source.clip = clip;
					source.Play();
					break;
				} 
			}
		}

		public void StopSE(){
			// 全てのSE用のAudioSouceを停止する
			foreach (AudioSource source in SEsources)
			{
				source.Stop ();
				source.clip = null;
			}
		}
	}
}
