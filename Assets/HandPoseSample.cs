/* 
*   Hand Pose
*   Copyright © 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using NatML.VideoKit;
    using NatML.Vision;
    using NatML.Visualizers;

    public sealed class HandPoseSample : MonoBehaviour {

        [Header(@"Camera")]
        public VideoKitCameraManager cameraManager;

        [Header(@"UI")]
        public RawImage rawImage;
        public AspectRatioFitter aspectFitter;
        public HandPoseVisualizer visualizer;

        private HandPosePredictor predictor;

        private async void Start () {
            // Create the hand pose predictor
            predictor = await HandPosePredictor.Create();
            // Listen for camera frames
            cameraManager.OnCameraFrame.AddListener(OnCameraFrame);
        }

        private void OnCameraFrame (CameraFrame frame) {
            // Predict
            var hand = predictor.Predict(frame);
            // Visualize
            visualizer.Render(hand);
        }

        void OnDisable () {
            // Stop listening for camera frames
            cameraManager.OnCameraFrame.RemoveListener(OnCameraFrame);
            // Dispose the predictor
            predictor?.Dispose();
        }
    }
}