﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealityCollective.ServiceFramework.Services;
using ServiceFrameworkExtensions.Services;
using TMPro;
using UnityEngine;
using YoloHolo.Services;
using YoloHolo.Utilities;

namespace YoloHolo.YoloLabeling
{
    public class YoloObjectLabeler : MonoBehaviour
    {
        [SerializeField]
        private GameObject labelObject;

        [SerializeField]
        private GameObject labelObjectAr;

        private bool isArabic = false;

        [SerializeField]
        private Vector2Int yoloImageSize = new(320, 256);

        [SerializeField]
        private float virtualProjectionPlaneWidth = 1.356f;

        [SerializeField]
        private float minIdenticalLabelDistance = 0.3f;

        [SerializeField]
        private float labelNotSeenTimeOut = 5f;

        [SerializeField]
        private Renderer debugRenderer;
        
        [SerializeField]
        private TextMeshPro debugText;

        private Vector2Int actualCameraSize;

        private IYoloProcessor yoloProcessor;
        
        private IFileLoggerService fileLoggerService;

        private readonly List<YoloGameObject> yoloGameObjects = new();
        private IImageAcquiringService imageAcquirer;

        public void SetArabic()
        {
            isArabic = true;
        }
        public void SetEnglish()
        {
            isArabic = false;
        }

        private async Task Start()
        {
            yoloProcessor = ServiceManager.Instance.GetService<IYoloProcessor>();
            imageAcquirer = ServiceManager.Instance.GetService<IImageAcquiringService>();

            imageAcquirer.Initialize(new Vector2Int(yoloImageSize.x, yoloImageSize.y));

            StartRecognizingAsync();
        }

        private Texture2D oldTexture;
        private async Task StartRecognizingAsync()
        {
            debugText.SetText("Started Recognizer");

            await Task.Delay(1000);
            do
            {
                actualCameraSize = new Vector2Int(imageAcquirer.ActualCameraSize.x, imageAcquirer.ActualCameraSize.y);
                if (actualCameraSize == Vector2Int.zero)
                {
                    debugText.SetText("Waiting for camera size");
                    await Task.Delay(100);
                }
            } 
            while (actualCameraSize == Vector2Int.zero);
            
            debugText.SetText($"Got camera size: {actualCameraSize.x} x {actualCameraSize.y}");
            
            while (true)
            {
                if (!Application.isPlaying)
                {
                    return;
                }
                
                var cameraTransform = Camera.main.CopyCameraTransForm();
                var texture = await imageAcquirer.GetImage();

                try
                {
                    if (debugRenderer != null && debugRenderer.gameObject.activeInHierarchy)
                    {
                        debugRenderer.material.mainTexture = texture;
                    }

                    await Task.Delay(32);
                    if (oldTexture != null)
                    {
                        Destroy(oldTexture);
                    }
                    var foundObjects = await yoloProcessor.RecognizeObjects(texture, isArabic);

                    for (int i = foundObjects.Count - 1; i >= 0; i--)
                    {
                        if (foundObjects[i].MostLikelyObject == null)
                        {
                            foundObjects.RemoveAt(i);
                        }
                    }

                    if (foundObjects.Count > 0)
                    {
                        debugText.SetText(foundObjects.Aggregate("Found: ", (current, obj) => current + $"{obj.MostLikelyObject} : {obj.Confidence}\n {obj.MostLikelyObject.ToString()}"));
                    }
                    else
                    {
                        debugText.SetText("No objects found");
                    }
#if UNITY_EDITOR                    
                    for (var index = 0; index < foundObjects.Count; index++)
                    {
                        var obj = foundObjects[index];
                        Debug.Log($"Found: {index} {obj.MostLikelyObject} : {obj.Confidence}");
                    }
#endif

                    ShowRecognitions(foundObjects, cameraTransform);
                    Destroy(cameraTransform.gameObject);
                    oldTexture = texture;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Exception running recognizer {ex.Message} { ex.StackTrace}");
                    debugText.SetText($"Exception running recognizer {ex.Message}");
                }
            }
        }

        private void ShowRecognitions(List<YoloItem> recognitions, Transform cameraTransform)
        {
            foreach (var recognition in recognitions)
            {
                var newObj = new YoloGameObject(recognition, cameraTransform,
                    actualCameraSize, yoloImageSize, virtualProjectionPlaneWidth);
                if (newObj.PositionInSpace != null && !HasBeenSeenBefore(newObj))
                {
                    yoloGameObjects.Add(newObj);
                    if (isArabic)
                    {
                        newObj.DisplayObject = Instantiate(labelObjectAr,
                        newObj.PositionInSpace.Value, Quaternion.identity);
                    }
                    else
                    {
                        newObj.DisplayObject = Instantiate(labelObject,
                            newObj.PositionInSpace.Value, Quaternion.identity);
                    }
                    newObj.DisplayObject.transform.parent = transform;
                    var labelController = newObj.DisplayObject.GetComponent<ObjectLabelController>();
                    labelController.SetText(newObj.Food.ToString());
                    labelController.SetFoodData(newObj.Food.ToFoodData());
                }
            }

            for (var i = yoloGameObjects.Count - 1; i >= 0; i--)
            {
                if (Time.time - yoloGameObjects[i].TimeLastSeen > labelNotSeenTimeOut)
                {
                    Destroy(yoloGameObjects[i].DisplayObject);
                    yoloGameObjects.RemoveAt(i);
                }
            }
        }

        private bool HasBeenSeenBefore(YoloGameObject obj)
        {
            var seenBefore = yoloGameObjects.FirstOrDefault(
                ylo => ylo.Food.Name == obj.Food.Name &&
                Vector3.Distance(obj.PositionInSpace.Value,
                    ylo.PositionInSpace.Value) < minIdenticalLabelDistance);
            if (seenBefore != null)
            {
                seenBefore.TimeLastSeen = Time.time;
            }
            return seenBefore != null;
        }
    }
}
