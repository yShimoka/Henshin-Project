// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Runtime.Actor {

/// <summary>
/// State class used to describe a <see cref="Directions.Scene.SceneState"/>'s actor.
/// Actors are shared across multiple scenes. 
/// </summary>
[System.SerializableAttribute]
public class ActorState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Identifiers -
            /// <summary>
            /// Name of this actor.
            /// </summary>
            public string Identifier;
            
            /// <summary>
            /// Prefab instance used by the actor.
            /// </summary>
            public UnityEngine.GameObject Prefab;
            
            /// <summary>
            /// List of all the poses that the actor can take.
            /// </summary>
            public System.Collections.Generic.List<UnityEngine.Sprite> PoseList = 
                new System.Collections.Generic.List<UnityEngine.Sprite>();
            
        // -- Public Attributes --
            /// <summary>
            /// Instance of the actor's <see cref="UnityEngine.GameObject"/> in the scene.
            /// </summary>
            [System.NonSerializedAttribute]
            public UnityEngine.GameObject Instance;
            
            /// <summary>
            /// Instance of the actor's <see cref="UnityEngine.SpriteRenderer"/> component.
            /// </summary>
            [System.NonSerializedAttribute]
            public UnityEngine.SpriteRenderer SpriteRenderer;
    // --- /Attributes ---
}
}