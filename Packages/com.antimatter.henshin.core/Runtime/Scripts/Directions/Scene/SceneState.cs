// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Directions.Act;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Scene {

/// <summary>
/// State class used to represent all the Scenes in the play.
/// These are not analogous to Unity's <see cref="UnityEngine.SceneManagement.Scene"/> objects.
/// </summary>
[Serializable]
public class SceneState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// Sprite used as the background of the scene.
            /// </summary>
            public Sprite Background;
            
            /// <summary>
            /// The list of all the actors of the scene.
            /// </summary>
            public List<ActorState> ActorList = new List<ActorState>();
            
            /// <summary>
            /// List of all the actions in the scene.
            /// This list is not ordered and should not be used as is.
            /// </summary>
            public List<ActionState> ActionList = new List<ActionState>();
            
            /// <summary>
            /// Flag set in debug mode if this scene should be played on its own.
            /// </summary>
            public bool IsDebugScene;
            
        // -- Public Attributes --
            // - Identifiers -
            /// <summary>
            /// Index of the scene in its parent <see cref="Act.ActState"/>.
            /// </summary>
            [NonSerialized]
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
            [NonSerialized]
            public ActionState RootAction;
            
            /// <summary>
            /// Reference to the act that owns this scene.
            /// </summary>
            [NonSerialized]
            public ActState Owner;
            
            // - Static References -
            /// <summary>
            /// Reference to the scene that is currently playing.
            /// </summary>
            [CanBeNull]
            public static SceneState Current;
    // --- /Attributes ---
}
}