// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Flip: Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// The flag set if the object should be flipped vertically.
            /// </summary>
            [System.NonSerialized]
            public bool Vertical;
            
            /// <summary>
            /// The flag set if the object should be flipped horizontally.
            /// </summary>
            [System.NonSerialized]
            public bool Horizontal;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructors --
            public Flip(Transformation from): base(@from: from) {}
            public Flip() {}
    // --- /Methods ---
}
}