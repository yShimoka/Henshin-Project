// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Runtime.Actions.Scene {

/// <summary>
/// Controller class used to manipulate <see cref="StartAction.State"/> objects.
/// This action marks the beginning of the specified action.
/// </summary>
[ActionControllerType(stateType: typeof(StartState))]
public class StartAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State class used to represent a <see cref="StartAction"/>.
            /// </summary>
            [System.SerializableAttribute]
            public class StartState: ActionState { public StartState() {} }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
        // -- Protected Methods --
            // - Serialization Events -
            /// <inheritdoc cref="ActionController._SaveParameters"/>
            protected override void _SaveParameters() { this.State.Parameters.Add(item: "Test"); }

            /// <inheritdoc cref="ActionController._LoadParameters"/>
            protected override void _LoadParameters() { }
        // -- Private Methods --
    // --- /Methods ---
}
}