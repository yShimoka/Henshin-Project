// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Visible: Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Flag set if the game object should be activated.
            /// </summary>
            [System.NonSerialized]
            public bool Activate;
            
            /// <summary>
            /// Flag set if all the actors should be activated.
            /// </summary>
            [System.NonSerialized]
            public bool AllActors;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructors --
            public Visible(Transformation from): base(@from: from) {}
            public Visible() {}
    // --- /Methods ---
}
}