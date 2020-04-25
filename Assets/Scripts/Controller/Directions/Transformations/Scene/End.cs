// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations.Scene {

/// <summary>
/// Transformation controller used to mark the end of a line.
/// Calls the <see cref="Scene.NextLine"/> method.
/// </summary>
[TransformationState(stateType: typeof(Henshin.State.Directions.Transformations.Scene.End))]
public class End: Transformation {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new Henshin.State.Directions.Transformations.Scene.Start State => base.State as Henshin.State.Directions.Transformations.Scene.Start;
            
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
                if (Henshin.Controller.Directions.Scene.Current.testScene) {
                    // Unset the debug flag.
                    Henshin.Controller.Directions.Scene.Current.testScene = false;
                    // Stop the application.
                    UnityEditor.EditorApplication.ExitPlaymode();
                } else {
#endif
                    // Clear the current scene.
                    Henshin.Controller.Directions.Scene.Current = null;
                    
                    
                    // Call the act's NextScene method.
                    Act.NextScene();
#if UNITY_EDITOR
                }
#endif
            }
    // --- /Methods ---
}
}