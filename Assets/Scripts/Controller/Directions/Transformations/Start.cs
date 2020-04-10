// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations {

/// <summary>
/// Transformation controller used to mark the start of a line.
/// Immediately starts its children nodes' application.
/// </summary>
[TransformationState(stateType: typeof(State.Directions.Transformations.Start))]
public class Start: Transformation {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new State.Directions.Transformations.Start State => base.State as State.Directions.Transformations.Start;
            
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Start"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public Start(State.Directions.Transformation state) : base(state: state) {}
            
        // -- Protected Methods --
            /// <inheritdoc cref="Transformation._Apply"/>
            protected override void _Apply() {
                // Immediately finish the action.
                this._Finish();
            }
    // --- /Methods ---
}
}