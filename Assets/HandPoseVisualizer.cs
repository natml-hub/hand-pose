/*
*   Hand Pose
*   Copyright © 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using Vision;

    /// <summary>
    /// Hand skeleton visualizer.
    /// </summary>
    public sealed class HandPoseVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// Render a hand.
        /// </summary>
        /// <param name="image">Image which hand is detected from.</param>
        /// <param name="hand">Hand to render.</param>
        public void Render (HandPosePredictor.Hand hand) {
            // Clear current
            foreach (var t in currentHand)
                GameObject.Destroy(t.gameObject);
            currentHand.Clear();
            // Create anchors
            foreach (var p in hand) {
                var anchor = Instantiate(anchorPrefab, p, Quaternion.identity, transform);
                anchor.gameObject.SetActive(true);
                currentHand.Add(anchor);
            }
            // Create bones
            foreach (var positions in new [] {
                new [] { hand.wrist, hand.thumbCMC, hand.thumbMCP, hand.thumbIP, hand.thumbTip },
                new [] { hand.wrist, hand.indexMCP, hand.indexPIP, hand.indexDIP, hand.indexTip },
                new [] { hand.middleMCP, hand.middlePIP, hand.middleDIP, hand.middleTip },
                new [] { hand.ringMCP, hand.ringPIP, hand.ringDIP, hand.ringTip },
                new [] { hand.wrist, hand.pinkyMCP, hand.pinkyPIP, hand.pinkyDIP, hand.pinkyTip },
                new [] { hand.indexMCP, hand.middleMCP, hand.ringMCP, hand.pinkyMCP }
            }) {
                var bone = Instantiate(bonePrefab, transform.position, Quaternion.identity, transform);
                bone.gameObject.SetActive(true);
                bone.positionCount = positions.Length;
                bone.SetPositions(positions);
                currentHand.Add(bone.transform);
            }
        }
        #endregion


        #region --Operations--
        [SerializeField] Transform anchorPrefab;
        [SerializeField] LineRenderer bonePrefab;
        readonly List<Transform> currentHand = new List<Transform>();
        #endregion
    }
}