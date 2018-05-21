using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManagerCode : MonoBehaviour
{
	public AudioSource music = null;
	public List<AudioSource> effects = new List<AudioSource> ();

	// Use this for initialization
	void Start ()
	{
		ApplySoundVolume ();
		ApplyMusicVolume ();
	}
	
	// Update is called once per frame
	void Update ()
	{		
	}

	public void ApplySoundVolume ()
	{
		float volume = 1.0f;
		if (PlayerPrefs.HasKey ("sound volume"))
			volume = PlayerPrefs.GetFloat ("sound volume");
		foreach (AudioSource effect in effects)
		{
			effect.volume = volume;
		}
	}

	public void ApplyMusicVolume ()
	{
		float volume = 0.30f;
		if (PlayerPrefs.HasKey ("music volume"))
			volume = PlayerPrefs.GetFloat ("music volume");
		Debug.Log (volume);
		if (music != null)
			music.volume = volume;
	}

	public void ChangeSoundVolume (GameObject soundObject)
	{
		PlayerPrefs.SetFloat ("sound volume", soundObject.GetComponent<UnityEngine.UI.Slider> ().value / 100.0f);
		PlayerPrefs.Save ();
		ApplySoundVolume ();
	}

	public void ChangeMusicVolume (GameObject musicObject)
	{
		PlayerPrefs.SetFloat ("music volume", musicObject.GetComponent<UnityEngine.UI.Slider> ().value / 100.0f);
		PlayerPrefs.Save ();
		ApplyMusicVolume ();
	}
}
