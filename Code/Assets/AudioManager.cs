using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioClip music_gameplay;
	public AudioClip music_menu;
	public AudioClip sound_click;
	public AudioClip sound_jump;
	
	float timer = 2f;
	
	AudioSource a1;
	AudioSource a2;
	
	void Start () 
	{
		GameObject.DontDestroyOnLoad(gameObject);
		a1 = gameObject.AddComponent<AudioSource>();
		a2 = gameObject.AddComponent<AudioSource>();
		
		a1.clip = music_menu;
		a2.clip = music_gameplay;
		a1.loop = true;
		a2.loop = true;
		a1.volume = 1f;
		a2.volume = 0f;
		a1.Play();
		a2.Play();
	}
	
	public void PlayGameplayMusic()
	{
		StartCoroutine("GamePlayMusic");
	}
	
	IEnumerator GamePlayMusic()
	{
		float temp_timer = timer;
		while(temp_timer > 0)
		{
			a1.volume = temp_timer / timer;
			a2.volume = (timer - temp_timer) / timer;
			temp_timer -= Time.deltaTime;
			yield return null;
		}
		a1.volume = 0f;
		a2.volume = 1f;
		yield return null;
	}
	
	public void PlaySound(AudioClip audioC, float _volume)
	{
		GameObject sound = new GameObject();
		AudioSource audioS = sound.AddComponent<AudioSource>();
		audioS.PlayOneShot(audioC, _volume);
		GameObject.DontDestroyOnLoad(audioS);
		GameObject.Destroy(audioS, audioC.length + 1f);
	}
}
