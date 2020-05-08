// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actor {

/// <summary>
/// State class used to describe a <see cref="Directions.Scene.SceneState"/>'s actor.
/// Actors are shared across multiple scenes. 
/// </summary>
[Serializable]
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
            public GameObject Prefab;
            
            /// <summary>
            /// List of all the poses that the actor can take.
            /// </summary>
            public PoseStore PoseStore;
            
        // -- Public Attributes --
            /// <summary>
            /// Instance of the actor's <see cref="UnityEngine.GameObject"/> in the scene.
            /// </summary>
            [NonSerialized]
            public GameObject Instance;
            
            /// <summary>
            /// Instance of the actor's <see cref="UnityEngine.SpriteRenderer"/> component.
            /// </summary>
            [NonSerialized]
            public Image Image;
            
            /// <summary>
            /// Instance of the actor's Canvas component.
            /// </summary>
            [NonSerialized]
            public Canvas Canvas;
    // --- /Attributes ---
}
}