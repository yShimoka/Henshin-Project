// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions {

/// <summary>
/// Static controller class used to manipulate <see cref="State.Directions.Act"/> states.
/// Stores a reference to the act currently being played back.
/// </summary>
public class Act {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Reference to the <see cref="Henshin.State.Directions.Act"/> currently playing.</summary>
            public static State.Directions.Act Current;
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            /// <summary>
            /// Starts playback of the specified <see cref="State.Directions.Act"/>.
            /// Stores the reference in the <see cref="Current"/> property.
            /// </summary>
            /// <param name="act"></param>
            public static void Play(State.Directions.Act act) {
                // Store the act's reference.
                Act.Current = act;
                // Return to the first scene of the act.
                act.ClearSceneIndex();
                
                // Start playback of the act's first scene.
                Scene.Play(scene: act.CurrentScene);
            }
            
            /// <summary>
            /// Starts playback of the next scene in the act.
            /// </summary>
            public static void NextScene() {
                // Ensure that there is an act playing.
                if (Act.Current == null) {
                    throw Application.Error(message: "Tried to advance to the next scene when there is no act playing.");
                }
                
                // Increment the current act's scene index.
                if (Act.Current.IncrementSceneIndex()) {
                    // Advance to the next act.
                    Application.NextAct();
                } else {
                    // Play the next scene.
                    Scene.Play(scene: Act.Current.CurrentScene);
                }
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}