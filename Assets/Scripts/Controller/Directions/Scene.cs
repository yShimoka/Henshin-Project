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
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Starts playback of the specified <see cref="Henshin.State.Directions.Scene"/>.
            /// </summary>
            /// <param name="scene">The scene instance to play.</param>
            public static void Play(State.Directions.Scene scene) {
                // Make sure that the scene is set.
                if (scene == null) throw new System.ArgumentNullException(paramName: nameof(scene), message: "Tried to play a null Scene.");
                
                // Set the current scene reference.
                Scene.Current = scene;
                
                // If the root transformation is unset.
                if (scene.RootTransformation == null) {
                    throw Application.Error(message: "The line has no transformation assigned to it !");
                }
                
                // Create the scene objects.
                View.Directions.Scene.UpdateSceneObjects(scene: scene);
                
                // Load all the actors.
                foreach (Actor actor in Scene.Current.actors) {
                    Controller.Scenery.Actor.Instantiate(actor: actor, parent: View.Application.Stage.transform);
                }
                
                // Start the scene's transformation chain.
                scene.RootTransformation.Apply();
            }
    // --- /Methods ---
}
}