// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions.Transformations.Scene {

/// <summary>
/// Simple serializable class for the Delay transformation controller.
/// Stores the duration of the delay in seconds.
/// </summary>
[Serializable]
public class Delay: Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Time of the delay, in seconds.</summary>
            [NonSerialized]
            public float Time;
            
            /// <summary>Timer used to check the duration of the delay.</summary>
            [NonSerialized]
            public float Timer;
    // --- /Attributes ---
    
    
    public Delay(Transformation from): base(@from: from) {}
    public Delay() {}
}
}