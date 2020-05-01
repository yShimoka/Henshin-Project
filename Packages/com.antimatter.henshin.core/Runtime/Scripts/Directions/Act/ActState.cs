// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Runtime.Directions.Act {

/// <summary>
/// State class used to describe the state of an act.
/// These instances are stored and serialized in the <see cref="Application.ApplicationState"/> object.
/// </summary>
[System.SerializableAttribute]
public class ActState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// List of all the scenes in the current act.
            /// </summary>
            public Runtime.Directions.Scene.SceneState[] SceneList;
            
        // -- Public Attributes --
            // - Identifiers -
            /// <summary>
            /// Index of the act in the state's list.
            /// </summary>
            [System.NonSerializedAttribute]
            public int Index;
            
            /// <summary>
            /// Accessor to the identifier of this act.
            /// </summary>
            public string Identifier => $"Act - {this.Index}";
            
            // - Runtime Parameters -
            /// <summary>
            /// Reference to the owner of this act.
            /// </summary>
            [System.NonSerializedAttribute]
            public Runtime.Application.ApplicationState Owner;
            
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
}
}