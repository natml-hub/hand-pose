# Hand Pose
Hand pose detection from MediaPipe. This predictor implements the [hand pose model](https://google.github.io/mediapipe/solutions/hands.html), but not the palm detector. It only supports detecting a single hand in the image.

## Installing Hand Pose
Add the following items to your Unity project's `Packages/manifest.json`:
```json
{
  "scopedRegistries": [
    {
      "name": "NatML",
      "url": "https://registry.npmjs.com",
      "scopes": ["ai.natml"]
    }
  ],
  "dependencies": {
    "ai.natml.vision.hand-pose": "1.0.1"
  }
}
```

## Detecting Hand Pose in an Image
First, create the predictor:
```csharp
// Create the hand pose predictor
var predictor = await HandPosePredictor.Create();
```

Then detect the hand pose in an image:
```csharp
Texture2D image = ...;
// Detect hand pose in an image
HandPosePredictor.Hand hand = predictor.Predict(image);
```

## Requirements
- Unity 2021.2+

## Quick Tips
- Discover more ML models on [NatML Hub](https://hub.natml.ai).
- See the [NatML documentation](https://docs.natml.ai/unity)
- Join the [NatML community on Discord](https://natml.ai/community)
- Contact us at [hi@natml.ai](mailto:hi@natml.ai)

Thank you very much!