// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Scene {

/// <summary>
/// State class used to represent all the Scenes in the play.
/// NOTE: These are not analogous to Unity's <see cref="UnityEngine.SceneManagement.Scene"/> objects.
/// </summary>
[System.SerializableAttribute]
public class SceneState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// Sprite used as the background of the scene.
            /// </summary>
            public UnityEngine.Sprite Background;
            
            /// <summary>
            /// The list of all the actors of the scene.
            /// </summary>
            public Runtime.Actor.ActorState[] ActorList;
            
            // TODO: Add the gameplay manager class.
            // public GameplayState Gameplay;
            
            /// <summary>
            /// List of all the actions in the scene.
            /// This list is not ordered and should not be used as is.
            /// </summary>
            public Runtime.Actions.ActionState[] ActionList;
            
#if UNITY_EDITOR
            /// <summary>
            /// Flag set in debug mode if this scene should be played on its own.
            /// </summary>
            public bool IsDebugScene;
#endif
            
        // -- Public Attributes --
            // - Identifiers -
            /// <summary>
            /// Index of the scene in its parent <see cref="Act.ActState"/>.
            /// </summary>
            [System.NonSerializedAttribute]
            public int Index;
            
            /// <summary>
            /// Accessor to the identifier of the scene.
            /// </summary>
            public string Identifier => $"Scene - {this.Index + 1}";
            
            // - Runtime Parameters -
            /// <summary>
            /// Reference to the root action of this scene.
            /// This should be the first applied action.
            /// </summary>
            [System.NonSerializedAttribute]
            public Runtime.Actions.ActionState RootAction;
            
            /// <summary>
            /// Reference to the act that owns this scene.
            /// </summary>
            [System.NonSerializedAttribute]
            public Runtime.Directions.Act.ActState Owner;
            
            // - Static References -
            /// <summary>
            /// Reference to the scene that is currently playing.
            /// </summary>
            public static SceneState Current;
            
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}