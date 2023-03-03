/* 
*   Hand Pose
*   Copyright © 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System;
    using System.Threading.Tasks;
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
        /// Detect hand pose in an image.
        /// </summary>
        /// <param name="inputs">Input image.</param>
        /// <returns>Detected hand.</returns>
        public Hand Predict (params MLFeature[] inputs) {
            // Pre-process image
            var input = inputs[0];
            if (input is MLImageFeature imageFeature) {
                (imageFeature.mean, imageFeature.std) = model.normalization;
                imageFeature.aspectMode = model.aspectMode;
            }  
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

        /// <summary>
        /// Dispose the model and release resources.
        /// </summary>
        public void Dispose () => model.Dispose();

        /// <summary>
        /// Create a hand pose predictor.
        /// </summary>
        /// <param name="configuration">Edge model configuration.</param>
        /// <param name="accessKey">NatML access key.</param>
        public static async Task<HandPosePredictor> Create (
            MLEdgeModel.Configuration configuration = null,
            string accessKey = null
        ) {
            var model = await MLEdgeModel.Create("@natsuite/hand-pose", configuration, accessKey);
            var predictor = new HandPosePredictor(model);
            return predictor;
        }
        #endregion


        #region --Operations--
        private readonly MLEdgeModel model;
        private HandPosePredictor (MLModel model) => this.model = model as MLEdgeModel;
        #endregion
    }
}