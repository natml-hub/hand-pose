/* 
*   Hand Pose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System;
    using NatML.Features;
    using NatML.Internal;
    using NatML.Types;

    /// <summary>
    /// Hand pose predictor.
    /// This predictor only supports predicting a single hand.
    /// </summary>
    public sealed partial class HandPosePredictor : IMLPredictor<HandPosePredictor.Hand> {

        #region --Client API--
        /// <summary>
        /// Create a hand pose predictor.
        /// </summary>
        /// <param name="model">Hand landmark ML model.</param>
        public HandPosePredictor (MLModel model) => this.model = model as MLEdgeModel;

        /// <summary>
        /// Detect hand pose in an image.
        /// </summary>
        /// <param name="inputs">Input image.</param>
        /// <returns>Detected hand.</returns>
        public Hand Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"Hand pose predictor expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0];
            if (!MLImageType.FromType(input.type))
                throw new ArgumentException(@"Hand pose predictor expects an an array or image feature", nameof(inputs));  
            // Predict
            var inputType = model.inputs[0] as MLImageType;
            using var inputFeature = (input as IMLEdgeFeature).Create(inputType);
            using var outputFeatures = model.Predict(inputFeature);
            // Marshal
            var scoreFeature = new MLArrayFeature<float>(outputFeatures[0]);
            var handednessFeature = new MLArrayFeature<float>(outputFeatures[1]);
            var anchorsFeature = new MLArrayFeature<float>(outputFeatures[2]);
            var score = scoreFeature[0];
            var handedness = handednessFeature[0] >= 0.5f ? Handedness.Right : Handedness.Left;
            var anchors = anchorsFeature.ToArray();
            // Return
            var result = new Hand(anchors, score, handedness, inputType.height);
            return result;
        }
        #endregion


        #region --Operations--
        private readonly MLEdgeModel model;

        void IDisposable.Dispose () { } // Not used
        #endregion
    }
}