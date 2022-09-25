# Hand Pose
Hand pose detection from MediaPipe. This predictor implements the [hand pose model](https://google.github.io/mediapipe/solutions/hands.html), but not the palm detector. It only supports detecting a single hand in the image.

## Detecting Hand Pose in an Image
First, create the predictor:
```csharp
// Fetch the model data from NatML
var modelData = await MLModelData.FromHub("@natsuite/hand-pose");
// Deserialize the model
var model = modelData.Deserialize();
// Create the predictor
var predictor = new HandPosePredictor(model);
```

Then detect the hand pose in an image:
```csharp
Texture2D image = ...; // Can also be a `WebCamTexture` or pixel buffer
// Detect hand pose in an image
HandPosePredictor.Hand hand = predictor.Predict(image);
```

## Requirements
- Unity 2021.2+

## Quick Tips
- Discover more ML models on [NatML Hub](https://hub.natml.ai).
- See the [NatML documentation](https://docs.natml.ai/unity)
- Join the [NatML community on Discord](https://hub.natml.ai/community)
- Check out the [NatML blog](https://blog.natml.ai)
- Contact us at [hi@natml.ai](mailto:hi@natml.ai)

Thank you very much!