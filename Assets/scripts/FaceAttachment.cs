using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FaceAttachment : MonoBehaviour
{
    private ARFaceManager faceManager;
    public GameObject glassesPrefab;
    public Vector3 positionOffset = new Vector3(0, 0.02f, 0.1f); // Adjust as needed
    public Vector3 rotationOffset = new Vector3(0, 0, 0); // Adjust as needed
    private GameObject currentGlasses;
    private ARFace currentFace;

    private float initialTouchPositionY;
    private float touchSensitivity = 0.001f; // Adjust sensitivity as needed

    void Start()
    {
        faceManager = FindObjectOfType<ARFaceManager>();

        faceManager.facesChanged += OnFacesChanged;

        foreach (var face in faceManager.trackables)
        {
            AttachGlasses(face);
        }
    }

    void OnFacesChanged(ARFacesChangedEventArgs eventArgs)
    {
        foreach (var face in eventArgs.added)
        {
            AttachGlasses(face);
        }
    }

    void AttachGlasses(ARFace face)
    {
        currentFace = face;
        currentGlasses = Instantiate(glassesPrefab, face.transform);
        UpdateGlassesPosition(currentGlasses, face);
    }

    void UpdateGlassesPosition(GameObject glasses, ARFace face)
    {
        Transform leftEyeTransform = face.leftEye;
        Transform rightEyeTransform = face.rightEye;

        if (leftEyeTransform != null && rightEyeTransform != null)
        {
            Vector3 midpoint = (leftEyeTransform.position + rightEyeTransform.position) / 2;
            glasses.transform.position = midpoint + positionOffset;
            glasses.transform.rotation = face.transform.rotation * Quaternion.Euler(rotationOffset);
        }
        else
        {
            glasses.transform.localPosition = new Vector3(0, 0, 0);
            glasses.transform.localRotation = Quaternion.identity;
        }
    }

    void Update()
    {
        HandleTouchInput();

        foreach (var face in faceManager.trackables)
        {
            if (face.transform.childCount > 0)
            {
                var glasses = face.transform.GetChild(0).gameObject;
                UpdateGlassesPosition(glasses, face);
            }
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    initialTouchPositionY = touch.position.y;
                    break;

                case TouchPhase.Moved:
                    float touchDelta = touch.position.y - initialTouchPositionY;
                    positionOffset.y += touchDelta * touchSensitivity;
                    initialTouchPositionY = touch.position.y; // Update initial position for continuous movement
                    UpdateGlassesPosition(currentGlasses, currentFace);
                    break;

                case TouchPhase.Ended:
                    initialTouchPositionY = 0;
                    break;
            }
        }
    }
}
