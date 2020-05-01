// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Pose: Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// The index of the new pose to use.
            /// </summary>
            [System.NonSerialized]
            public int PoseIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructors --
            public Pose(Transformation from): base(@from: from) {}
            public Pose() {}
    // --- /Methods ---
}
}