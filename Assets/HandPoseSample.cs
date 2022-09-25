/* 
*   Hand Pose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using NatML;
    using NatML.Devices;
    using NatML.Devices.Outputs;
    using NatML.Features;
    using NatML.Vision;
    using NatML.Visualizers;

    [MLModelDataEmbed("@natsuite/hand-pose")]
    public class HandPoseSample : MonoBehaviour {

        [Header(@"UI")]
        public RawImage rawImage;
        public AspectRatioFitter aspectFitter;
        public HandPoseVisualizer visualizer;

        private CameraDevice cameraDevice;
        private TextureOutput cameraTextureOutput;

        private MLModel model;
        private HandPosePredictor predictor;

        async void Start () {
            // Request camera permissions
            var permissionStatus = await MediaDeviceQuery.RequestPermissions<CameraDevice>();
            if (permissionStatus != PermissionStatus.Authorized) {
                Debug.LogError(@"User did not grant camera permissions");
                return;
            }
            // Get the default camera device
            var query = new MediaDeviceQuery(MediaDeviceCriteria.CameraDevice);
            cameraDevice = query.current as CameraDevice;
            // Start the camera preview
            cameraDevice.previewResolution = (640, 480);
            cameraTextureOutput = new TextureOutput();
            cameraDevice.StartRunning(cameraTextureOutput);
            // Display the camera preview
            var cameraTexture = await cameraTextureOutput;
            rawImage.texture = cameraTexture;
            aspectFitter.aspectRatio = (float)cameraTexture.width / cameraTexture.height;
            // Create the hand pose predictor
            var modelData = await MLModelData.FromHub("@natsuite/hand-pose");
            model = modelData.Deserialize();
            predictor = new HandPosePredictor(model);
        }

        void Update () {
            // Check that the predictor has been created
            if (predictor == null)
                return;
            // Predict
            var hand = predictor.Predict(cameraTextureOutput.texture);
            // Visualize
            visualizer.Render(hand);
        }

        void OnDisable () {
            // Dispose the model
            model?.Dispose();
        }
    }
}