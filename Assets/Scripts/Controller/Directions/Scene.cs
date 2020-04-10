// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.State;
using Henshin.State.Scenery;
using Henshin.View;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions {

/// <summary>
/// Static controller class used to manipulate <see cref="State.Directions.Scene"/> states.
/// Stores a reference to the scene currently being played back.
/// </summary>
public static class Scene {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Reference to the <see cref="Henshin.State.Directions.Scene"/> that is currently playing.</summary>
            public static State.Directions.Scene Current;
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Starts playback of the specified <see cref="Henshin.State.Directions.Scene"/>.
            /// </summary>
            /// <param name="scene">The scene instance to play.</param>
            public static void Play(State.Directions.Scene scene) {
                // Set the current scene reference.
                Scene.Current = scene;
                // Clear the scene's line index.
                scene.ClearLineIndex();
                
                // Instantiate the scene's actors.
                foreach (Actor actor in scene.actors) {
                    Scenery.Actor.Instantiate(actor: actor, parent: View.Application.Stage.transform);
                }
                
                // Update the stage's background.
                View.Application.Background = scene.background;
                
                // Starts the scene's first line.
                Line.Play(line: scene.CurrentLine);
            }
            
            /// <summary>
            /// Starts playback of the next line in the scene.
            /// </summary>
            public static void NextLine() {
                // Ensure that there is a scene playing.
                if (Scene.Current == null) {
                    throw Application.Error(message: "Tried to advance to the next line when there is no scene playing.");
                }
                
                // Increment the current scene's index.
                if (Scene.Current.IncrementLineIndex()) {
                    // Advance to the next scene.
                    Act.NextScene();
                } else {
                    // Play the next line.
                    Line.Play(line: Scene.Current.CurrentLine);
                }
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}