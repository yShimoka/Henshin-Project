// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations {

/// <summary>
/// Transformation controller used to mark the end of a line.
/// Calls the <see cref="Scene.NextLine"/> method.
/// </summary>
[TransformationState(stateType: typeof(State.Directions.Transformations.End))]
public class End: Transformation {
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
            /// Creates a new <see cref="End"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public End(State.Directions.Transformation state) : base(state: state) {}
            
        // -- Protected Methods --
            /// <inheritdoc cref="Transformation._Apply"/>
            protected override void _Apply() {
#if UNITY_EDITOR
                // Check if the scene is the test scene.
                if (Scene.Current.testScene) {
                    // Stop the application.
                    UnityEditor.EditorApplication.ExitPlaymode();
                } else {
#endif
                    // Clear the current scene.
                    Scene.Current = null;
                    
                    
                    // Call the act's NextScene method.
                    Act.NextScene();
#if UNITY_EDITOR
                }
#endif
            }
    // --- /Methods ---
}
}