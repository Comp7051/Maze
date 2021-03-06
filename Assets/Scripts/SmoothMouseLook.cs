﻿// http://wiki.unity3d.com/index.php/SmoothMouseLook

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;

	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	

	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;

	public float frameCounter = 20;

	Quaternion originalRotation;

    public bool affectOnlyCamera;

	void Update ()
	{
        if (axes == RotationAxes.MouseXAndY)
        {
            rotAverageY = 0f;
            rotAverageX = 0f;

            if (affectOnlyCamera) { 
                rotationY += -Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY += -Input.GetAxis("Vertical2") * sensitivityY;
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY += Input.GetAxis("Vertical2") * sensitivityY;
            }
            
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX += Input.GetAxis("Horizontal2") * sensitivityX;

            rotArrayY.Add(rotationY);
			rotArrayX.Add(rotationX);

			if (rotArrayY.Count >= frameCounter) {
				rotArrayY.RemoveAt(0);
			}
			if (rotArrayX.Count >= frameCounter) {
				rotArrayX.RemoveAt(0);
			}

			for(int j = 0; j < rotArrayY.Count; j++) {
				rotAverageY += rotArrayY[j];
			}
			for(int i = 0; i < rotArrayX.Count; i++) {
				rotAverageX += rotArrayX[i];
			}

			rotAverageY /= rotArrayY.Count;
			rotAverageX /= rotArrayX.Count;

			rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
			rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

			Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);

            if (affectOnlyCamera)
                Camera.main.transform.rotation = originalRotation * xQuaternion * yQuaternion;
            else
                transform.localRotation = originalRotation * xQuaternion * yQuaternion;



        }
        else if (axes == RotationAxes.MouseX)
		{			
			rotAverageX = 0f;

			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX += Input.GetAxis("Horizontal2") * sensitivityX;

            rotArrayX.Add(rotationX);

			if (rotArrayX.Count >= frameCounter) {
				rotArrayX.RemoveAt(0);
			}
			for(int i = 0; i < rotArrayX.Count; i++) {
				rotAverageX += rotArrayX[i];
			}
			rotAverageX /= rotArrayX.Count;

			rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

			Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);
            if (affectOnlyCamera)
                Camera.main.transform.rotation = originalRotation * xQuaternion;
            else
                transform.localRotation = originalRotation * xQuaternion;
        }
        else
		{			
			rotAverageY = 0f;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY += Input.GetAxis("Vertical2") * sensitivityY;

            rotArrayY.Add(rotationY);

			if (rotArrayY.Count >= frameCounter) {
				rotArrayY.RemoveAt(0);
			}
			for(int j = 0; j < rotArrayY.Count; j++) {
				rotAverageY += rotArrayY[j];
			}
			rotAverageY /= rotArrayY.Count;

			rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);

			Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
            if (affectOnlyCamera)
                Camera.main.transform.rotation = originalRotation * yQuaternion;
            else
                transform.localRotation = originalRotation * yQuaternion;
        }
	}

	public void ResetRotationToOriginal()
	{
		rotArrayX.Clear ();
		rotArrayY.Clear ();
		rotationX = 0.0F;
		rotationY = 0.0F;
		rotAverageX = 0.0F;
		rotAverageY = 0.0F;
        if (affectOnlyCamera)
            Camera.main.transform.rotation = originalRotation;
        else
            transform.localRotation = originalRotation;
	}

	void Start ()
	{		
		Rigidbody rb = GetComponent<Rigidbody>();	
		if (rb)
			rb.freezeRotation = true;
        if (affectOnlyCamera)
            originalRotation = Camera.main.transform.rotation;
        else
            originalRotation = transform.localRotation;
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}
}