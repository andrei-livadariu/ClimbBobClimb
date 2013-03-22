using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public string gameScene;
	public GUIStyle buttonStyle;
	public Texture background;
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		if(GUI.Button(new Rect((Screen.width/10) * 7.5f, (Screen.height/5) *4, 130, 50), "", buttonStyle))
		{
			StartGame();
		}
	}
	
	void StartGame()
	{
		AudioManager am = GameObject.Find("AudioManager").transform.GetComponent<AudioManager>();
		am.PlaySound( am.sound_click, 1f );
		
		am.PlayGameplayMusic();
		
		Application.LoadLevel(gameScene);
	}
}