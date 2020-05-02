// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Act {

/// <summary>
/// State class used to describe the state of an act.
/// These instances are stored and serialized in the <see cref="Application.ApplicationState"/> object.
/// </summary>
[System.SerializableAttribute]
public class ActState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// List of all the scenes in the current act.
            /// </summary>
            public Runtime.Directions.Scene.SceneState[] SceneList;
            
        // -- Public Attributes --
            // - Identifiers -
            /// <summary>
            /// Index of the act in the state's list.
            /// </summary>
            [System.NonSerializedAttribute]
            public int Index;
            
            /// <summary>
            /// Accessor to the identifier of this act.
            /// </summary>
            public string Identifier => $"Act - {this.Index + 1}";
            
            // - Runtime Parameters -
            /// <summary>
            /// Reference to the owner of this act.
            /// </summary>
            [System.NonSerializedAttribute]
            public Runtime.Application.ApplicationState Owner;
            
            /// <summary>
            /// Index of the scene that is currently playing.
            /// </summary>
            [System.NonSerializedAttribute]
            public int CurrentSceneIndex;
            
            // - Static References -
            /// <summary>
            /// Reference to the act that is currently being played.
            /// </summary>
            [System.NonSerializedAttribute]
            public static ActState Current;
            
            // - Helper Properties -
            /// <summary>
            /// Helper accessor to the current scene object.
            /// </summary>
            [JetBrains.Annotations.CanBeNullAttribute]
            public Runtime.Directions.Scene.SceneState CurrentScene {
                get {
                    // Check if the scene index is valid.
                    if (this.CurrentSceneIndex >= 0 && this.CurrentSceneIndex < this.SceneList.Length) {
                        // Return the scene instance.
                        return this.SceneList[this.CurrentSceneIndex];
                    } else {
                        // Return a null.
                        return null;
                    }
                }
            }
            
    // --- /Attributes ---
}
}