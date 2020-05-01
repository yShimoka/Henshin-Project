// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class MoveTo: Henshin.State.Directions.Transformations.Scene.Delay {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Target position of the selected actor.
            /// </summary>
            [System.NonSerialized]
            public Vector2 Target;
            
            /// <summary>
            /// Stores the starting position of the actor.
            /// </summary>
            [System.NonSerialized]
            public Vector2 Start;
            
            /// <summary>
            /// Easing function used for the movement.
            /// </summary>
            [System.NonSerializedAttribute]
            public View.Misc.EasingFunction.Ease EaseMode;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructors --
            public MoveTo(Transformation from): base(@from: from) {}
            public MoveTo() {}
    // --- /Methods ---
}
}