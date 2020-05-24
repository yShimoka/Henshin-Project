// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay.Components.Textbox;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
            [Header(header: "Prefabs")]
            public GameObject TextboxPrefab;
            
            /// <summary>
            /// Prefab instance used for all the textbox child text components.
            /// </summary>
            public GameObject TextboxTextPrefab;
            
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
            
            // - Text Settings -
            /// <summary>
            /// Color applied to the texts.
            /// </summary>
            public Color TextColor;
            
            public Sprite Arrow;
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
            
            /// <summary>
            /// Stores the current index in the selected Gameplay node.
            /// </summary>
            [NonSerialized]
            public int CurrentIndex;
            
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
            
            public TextboxController Textbox => TextboxState.ControllerInstance;
            //public ToolboxController Toolbox;
    // --- /Attributes ---
}
}