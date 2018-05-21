using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
	public Slider sliderObject;
	public Text sliderText;
	public string prefix;
	public string preferenceKey;

	void Start ()
	{
		if (PlayerPrefs.HasKey (preferenceKey))
			sliderObject.value = PlayerPrefs.GetFloat (preferenceKey) * 100;

		sliderText.text = prefix + sliderObject.value + "/" + sliderObject.maxValue; 
	}

	private void Update ()
	{
		sliderText.text = prefix + sliderObject.value + "/" + sliderObject.maxValue; 
	}
}
