// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Scale: Henshin.State.Directions.Transformations.Scene.Delay {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Target scale of the selected actor.
            /// </summary>
            [System.NonSerialized]
            public Vector2 Target;
            
            /// <summary>
            /// Stores the starting scale of the actor.
            /// </summary>
            [System.NonSerialized]
            public Vector2 Start;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructors --
            public Scale(Transformation from): base(@from: from) {}
            public Scale() {}
    // --- /Methods ---
}
}