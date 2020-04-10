// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.State;
using Henshin.Controller;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions {

/// <summary>
/// Static controller class used to manipulate <see cref="State.Directions.Line"/> states.
/// Stores a reference to the line currently being played back.
/// </summary>
public static class Line {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Stores a reference to the line object that is currently being played.</summary>
            public static State.Directions.Line Current;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Starts the playback of the specified line.
            /// Stores said line in the <see cref="Current"/> property.
            /// </summary>
            /// <param name="line">The line to start playback for.</param>
            public static void Play(State.Directions.Line line) {
                // Set the current line reference.
                Line.Current = line;
                
                // If the root transformation is unset.
                if (line.RootTransformation == null) {
                    throw Application.Error(message: "The line has no transformation assigned to it !");
                }
                
                // Start the line's transformation chain.
                line.RootTransformation.Apply();
            }
    // --- /Methods ---
}
}