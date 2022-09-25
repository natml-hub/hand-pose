/* 
*   Hand Pose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed partial class HandPosePredictor {

        /// <summary>
        /// Handedness.
        /// </summary>
        public enum Handedness {
            Left = 0,
            Right = 1
        }

        /// <summary>
        /// Detected hand.
        /// </summary>
        public readonly struct Hand : IReadOnlyList<Vector3> {

            #region --Client API--
            /// <summary>
            /// Number of keypoints in the hand.
            /// </summary>
            public int Count => data.Length / 3;

            /// <summary>
            /// Hand confidence score.
            /// </summary>
            public readonly float score;

            /// <summary>
            /// Hand handedness.
            /// </summary>
            public readonly Handedness handedness;
            
            /// <summary>
            /// </summary>
            public Vector3 wrist => this[0];

            /// <summary>
            /// </summary>
            public Vector3 thumbCMC => this[1];

            /// <summary>
            /// </summary>
            public Vector3 thumbMCP => this[2];

            /// <summary>
            /// </summary>
            public Vector3 thumbIP => this[3];

            /// <summary>
            /// </summary>
            public Vector3 thumbTip => this[4];

            /// <summary>
            /// </summary>
            public Vector3 indexMCP => this[5];

            /// <summary>
            /// </summary>
            public Vector3 indexPIP => this[6];

            /// <summary>
            /// </summary>
            public Vector3 indexDIP => this[7];

            /// <summary>
            /// </summary>
            public Vector3 indexTip => this[8];

            /// <summary>
            /// </summary>
            public Vector3 middleMCP => this[9];

            /// <summary>
            /// </summary>
            public Vector3 middlePIP => this[10];

            /// <summary>
            /// </summary>
            public Vector3 middleDIP => this[11];

            /// <summary>
            /// </summary>
            public Vector3 middleTip => this[12];

            /// <summary>
            /// </summary>
            public Vector3 ringMCP => this[13];

            /// <summary>
            /// </summary>
            public Vector3 ringPIP => this[14];

            /// <summary>
            /// </summary>
            public Vector3 ringDIP => this[15];

            /// <summary>
            /// </summary>
            public Vector3 ringTip => this[16];

            /// <summary>
            /// </summary>
            public Vector3 pinkyMCP => this[17];

            /// <summary>
            /// </summary>
            public Vector3 pinkyPIP => this[18];

            /// <summary>
            /// </summary>
            public Vector3 pinkyDIP => this[19];

            /// <summary>
            /// </summary>
            public Vector3 pinkyTip => this[20];

            /// <summary>
            /// Get the hand keypoint by its index.
            /// </summary>
            public Vector3 this [int idx] => new Vector3(data[idx * 3 + 0], height - data[idx * 3 + 1], data[idx * 3 + 2]);
            #endregion


            #region --Operations--
            private readonly float[] data;
            private readonly int height;

            internal Hand (float[] data, float score, Handedness handedness, int height) {
                this.data = data;
                this.score = score;
                this.handedness = handedness;
                this.height = height;
            }

            IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator () {
                for (var i = 0; i < Count; ++i)
                    yield return this[i];
            }

            IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<Vector3>).GetEnumerator();
            #endregion
        }
    }
}