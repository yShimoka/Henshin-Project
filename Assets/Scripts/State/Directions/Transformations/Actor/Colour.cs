// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Colour: Henshin.State.Directions.Transformations.Scene.Delay {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Target colour of the selected actor.
            /// </summary>
            [System.NonSerialized]
            public Color Target;
            
            /// <summary>
            /// Stores the starting colour of the actor.
            /// </summary>
            [System.NonSerialized]
            public Color Start;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructors --
            public Colour(Transformation from): base(@from: from) {}
            public Colour() {}
    // --- /Methods ---
}
}