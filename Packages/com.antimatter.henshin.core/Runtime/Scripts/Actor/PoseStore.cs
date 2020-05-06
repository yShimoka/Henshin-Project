// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actor {

/// <summary>
/// Store of poses that can be used by all the actor instances.
/// </summary>
[CreateAssetMenu(menuName = "Henshin/Pose Store", fileName = "DATA_POSE_Store")]
public class PoseStore: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// List of all the poses.
            /// </summary>
            public List<Sprite> PoseList = new List<Sprite>();
    // --- /Attributes ---
}
}