// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay.Components.Answer;
using UnityEngine;
using UnityEngine.Events;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay {

/// <summary>
/// Class used to describe the state of the gameplay manager.
/// Instanced in the <see cref="ApplicationState"/> class.
/// Stores serialized info for the <see cref="GameplayView"/> class as well. 
/// </summary>
[Serializable]
public class GameplayState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Prefabs -
            /// <summary>
            /// Prefab object used for the GUI text box.
            /// Instanced when the application is started.
            /// </summary>
            public GameObject TextboxPrefab;
            
            /// <summary>
            /// Prefab object used for the GUI tool box.
            /// Instanced when the application is started.
            /// </summary>
            public GameObject ToolboxPrefab;
            
            /// <summary>
            /// Prefab object used for the GUI text answers that are created in the tool box.
            /// Instanced by some gameplays.
            /// </summary>
            public GameObject TextAnswerPrefab;
            
            /// <summary>
            /// Prefab object used for the GUI text targets that are created in the text box.
            /// Instanced by some gameplays.
            /// </summary>
            public GameObject TextTargetPrefab;
            
        // -- Public Attributes --
            // - Gameplay Info -
            /// <summary>
            /// Identifier of the current gameplay mode.
            /// Loaded from the Xml 'kind' attribute when loading a gameplay.
            /// </summary>
            [NonSerialized]
            public string CurrentMode;
            
            /// <summary>
            /// Stores the callback method to trigger once the sequence is done.
            /// </summary>
            [NonSerialized]
            public UnityAction Callback;
            
            /// <inheritdoc cref="DataState.AnswerObjectSize"/>
            public static Vector2 AnswerObjectSize => DataState.AnswerObjectSize;
            
            /// <inheritdoc cref="DataState.DuplicateAnswers"/>
            public static bool DuplicateAnswers => DataState.DuplicateAnswers;
        
            // - Reference -
            /// <summary>
            /// Helper accessor.
            /// Returns the <see cref="GameplayState"/> reference in the current <see cref="ApplicationState"/>.
            /// </summary>
            public static GameplayState Own => ApplicationState.Own.GameplayState;
            
            // - Component References -
            /// <summary>
            /// Reference to the <see cref="GameObject"/> of the textbox.
            /// </summary>
            [NonSerialized]
            public GameObject TextboxObject;
            
            /// <summary>
            /// Reference to the <see cref="GameObject"/> of the toolbox.
            /// </summary>
            [NonSerialized]
            public GameObject ToolboxObject;
            
            //public TextboxController Textbox;
            //public ToolboxController Toolbox;
    // --- /Attributes ---
}
}