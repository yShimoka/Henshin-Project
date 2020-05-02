// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Runtime.Actions {

/// <summary>
/// Base class used for all the action states.
/// This class is used by the <see cref="Directions.Scene.SceneState"/> to list all the actions of the scene.
/// </summary>
[System.SerializableAttribute]
public class ActionState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// List of all of this <see cref="ActionState"/>'s children indices.
            /// Used for serialization only. 
            /// </summary>
            public int[] ChildrenIndexList;
            
            /// <summary>
            /// List of all the parameters for this action state.
            /// This list is used by the <see cref="ActionController"/> overrides to handle action-specific parameters.
            /// </summary>
            public System.Collections.Generic.List<string> Parameters;
            
            /// <summary>
            /// Name of the <see cref="ActionController"/>'s class.
            /// </summary>
            public string ActionControllerName;
            
        // -- Public Attributes --
            // - Runtime Parameters -
            /// <summary>
            /// List of all the children of this <see cref="ActionState"/>.
            /// </summary>
            [System.NonSerializedAttribute]
            public System.Collections.Generic.List<ActionState> ChildrenList 
                = new System.Collections.Generic.List<ActionState>();
            
            /// <summary>
            /// Number of parents of this <see cref="ActionState"/>.
            /// </summary>
            [System.NonSerializedAttribute]
            public int ParentCount = 0;
            
            /// <summary>
            /// Counts the number of parents that are finished.
            /// While this value is below <see cref="ParentCount"/>,
            /// the action should not be applied.
            /// </summary>
            [System.NonSerializedAttribute]
            public int ParentFinishedCounter;
            
    // --- /Attributes ---
}
}