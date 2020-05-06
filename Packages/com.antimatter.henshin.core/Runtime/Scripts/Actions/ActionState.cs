// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.



/* Wrap the class within the local namespace. */

using System;
using System.Collections.Generic;

namespace Henshin.Runtime.Actions {

/// <summary>
/// Base class used for all the action states.
/// This class is used by the <see cref="Directions.Scene.SceneState"/> to list all the actions of the scene.
/// </summary>
[Serializable]
public class ActionState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// List of all of this <see cref="ActionState"/>'s children indices.
            /// Used for serialization only. 
            /// </summary>
            public List<int> ChildrenIndexList = new List<int>();
            
            /// <summary>
            /// List of all the parameters for this action state.
            /// This list is used by the <see cref="Runtime.Actions.ActionController"/> overrides to handle action-specific parameters.
            /// </summary>
            public List<string> Parameters = new List<string>();
            
            /// <summary>
            /// Name of the <see cref="Runtime.Actions.ActionController"/>'s class.
            /// </summary>
            public string ActionControllerName;
            
        // -- Public Attributes --
            // - Runtime Parameters -
            /// <summary>
            /// List of all the children of this <see cref="ActionState"/>.
            /// </summary>
            [NonSerialized]
            public List<ActionState> ChildrenList = new List<ActionState>();
            
            /// <summary>
            /// Number of parents of this <see cref="ActionState"/>.
            /// </summary>
            [NonSerialized]
            public int ParentCount = 0;
            
            /// <summary>
            /// Counts the number of parents that are finished.
            /// While this value is below <see cref="ParentCount"/>,
            /// the action should not be applied.
            /// </summary>
            [NonSerialized]
            public int ParentFinishedCounter;
            
            /// <summary>
            /// The type of the controller.
            /// </summary>
            [NonSerialized]
            public Type ControllerType;
    // --- /Attributes ---
}
}