using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;


public class VignetteModify : MonoBehaviour {
	public PostProcessingProfile profile;
	public float intensity = 1;

	VignetteModel.Settings s;

	void Start () {
		s = profile.vignette.settings;

		s.intensity = intensity;
		profile.vignette.settings = s;

		// v.settings reference!!!
		/*
		[ColorUsage(false)]
		[Tooltip("Vignette color. Use the alpha channel for transparency.")]
		public Color color;

		[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
		public Vector2 center;

		[Range(0f, 1f), Tooltip("Amount of vignetting on screen.")]
		public float intensity;

		[Range(0.01f, 1f), Tooltip("Smoothness of the vignette borders.")]
		public float smoothness;

		[Range(0f, 1f), Tooltip("Lower values will make a square-ish vignette.")]
		public float roundness;

		[Tooltip("A black and white mask to use as a vignette.")]
		public Texture mask;

		[Range(0f, 1f), Tooltip("Mask opacity.")]
		public float opacity;

		[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
		public bool rounded;
		*/
	}


	void Update () {
		//use the following 2 lines to change intensity
		s.intensity = intensity;
		profile.vignette.settings = s;
	}
}
