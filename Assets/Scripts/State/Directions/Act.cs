// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions {

/// <summary>
/// State class used to represent an <see cref="Application"/>'s act.
/// <see cref="Act"/>s are filled with <see cref="Scene"/>s that are played in sequence.
/// </summary>
[Serializable]
public class Act {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>Unique identifier of this act.</summary>
            public string identifier;
            
            /// <summary>List of all the scenes played in this act.</summary>
            public List<Scene> scenes = new List<Scene>();
            
        // -- Public Attributes --
            /// <summary>Helper method used to access the scene currently playing.</summary>
            public Scene CurrentScene => this.scenes[index: this._mCurrentSceneIndex]; 
        
        // -- Private Attributes --
            /// <summary>Index of the scene currently played.</summary>
            private int _mCurrentSceneIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Resets the scene index of this scene to 0.</summary>
            public void ClearSceneIndex() { this._mCurrentSceneIndex = 0; }
            
            /// <summary>Increments the scene index counter.</summary>
            /// <returns>True if the line index is greater than the total line count.</returns>
            public bool IncrementSceneIndex() { this._mCurrentSceneIndex++; return this._mCurrentSceneIndex >= this.scenes.Count; }
    // --- /Methods ---
}
}