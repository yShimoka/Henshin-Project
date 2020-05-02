// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Scene {

/// <summary>
/// Static class used to manipulate the view of the scene.
/// Creates the actors and updates the background.
/// </summary>
public static class SceneView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Prepares the specified scene's view.
            /// Creates the actors and updates the background.
            /// </summary>
            /// <param name="scene">The scene object that must be prepared.</param>
            public static void Prepare([JetBrains.Annotations.NotNullAttribute] SceneState scene) {
                // Create all the scene's actors.
                foreach (Runtime.Actor.ActorState actorState in scene.ActorList) {
                    Runtime.Actor.ActorView.InstantiateActor(actor: actorState);
                }
                
                // Update the view's background.
                Runtime.Application.ApplicationView.Background.sprite = scene.Background;
            }
        // -- Private Methods --
    // --- /Methods ---
}
}