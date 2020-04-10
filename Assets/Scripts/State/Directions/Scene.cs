// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions {

/// <summary>
/// State class used to represent an <see cref="Act"/>'s scene.
/// <see cref="Scene"/>s are filled with <see cref="Line"/>s that are played in sequence.
/// </summary>
[Serializable]
public class Scene {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>List of all the lines of this scene.</summary>
            public List<Line> lines;
            
            /// <summary>List of all the actors that take part in the scene.</summary>
            public List<Scenery.Actor> actors;
            
            /// <summary>Reference to the sprite used as this scene's background.</summary>
            public Sprite background;
            
        // -- Public Attributes --
            /// <summary>Returns a reference to the <see cref="Line"/> at the current index.</summary>
            public Line CurrentLine => this.lines[index: this._mCurrentLineIndex];
        
        // -- Private Attributes --
            /// <summary>Index of the line that is currently playing.</summary>
            private int _mCurrentLineIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Resets the line index of this scene to 0.</summary>
            public void ClearLineIndex() { this._mCurrentLineIndex = 0; }
            
            /// <summary>
            /// Increments the line index counter.
            /// </summary>
            /// <returns>True if the line index is greater than the total line count.</returns>
            public bool IncrementLineIndex() { this._mCurrentLineIndex++; return this._mCurrentLineIndex >= this.lines.Count; }
            
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}