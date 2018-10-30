﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour {

	private float ambientIntensity;
	public float ambientIntensityDay;
	public float ambientIntensityNight;

	public Material fogmat;
	Camera playerCamera;
	public Shader replacementShader;
	private bool fogEnabled;

	void ToggleFog(bool enable) {
		/*if (enable) {
			if (replacementShader != null) {
				GetComponent<Camera> ().SetReplacementShader (replacementShader, "RenderType");
			}
		} else {
			GetComponent<Camera> ().ResetReplacementShader ();
		}*/
	}

	// Use this for initialization
	void Start () {
		ambientIntensity = ambientIntensityDay;
		Shader.SetGlobalFloat ("_Ambient", ambientIntensity);
		playerCamera = Camera.main;
		playerCamera.depthTextureMode = DepthTextureMode.Depth;
		fogEnabled = false;
		ToggleFog (fogEnabled);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.N) || Input.GetKeyUp(KeyCode.Joystick1Button4)) {
			if (ambientIntensity >= ambientIntensityDay)
				ambientIntensity = ambientIntensityNight;
			else if (ambientIntensity <= ambientIntensityNight)
				ambientIntensity = ambientIntensityDay;

			Shader.SetGlobalFloat ("_Ambient", ambientIntensity);
			Debug.Log (Shader.GetGlobalFloat ("_Ambient"));
		}

		if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.Joystick1Button5)) {
			fogEnabled = !fogEnabled;
			ToggleFog (fogEnabled);
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination){
		// https://docs.unity3d.com/540/Documentation/Manual/WritingImageEffects.html
		if (fogEnabled) {
			Graphics.Blit (source, destination, fogmat);
		} else {
			Graphics.Blit (source, destination);
		}
		//mat is the material which contains the shader
		//we are passing the destination RenderTexture to
	}
}