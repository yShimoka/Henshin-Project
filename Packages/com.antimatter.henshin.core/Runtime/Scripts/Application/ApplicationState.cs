// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Runtime.Directions.Act;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Application {

/// <summary>
/// State class used to represent the application.
/// Stores all the relevant information into a single area.
/// </summary>
[CreateAssetMenu(fileName = "APP_State", menuName = "Henshin State", order = 0)]
public class ApplicationState: ScriptableObject, ISerializationCallbackReceiver {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Parameters -
            /// <summary>
            /// Serializable list of all the acts in the application.
            /// </summary>
            [SerializeField] 
            public List<ActState> ActList = new List<ActState>();
            
#if UNITY_EDITOR
            /// <summary>
            /// Flag set if this is a debug application state.
            /// Used only within the editor.
            /// </summary>
            [SerializeField]
            public bool IsDebugState;
#endif
            
            // - Visual Parameters -
            /// <summary>
            /// Color used when the camera is cleared.
            /// </summary>
            public Color ClearColor;
            
            /// <summary>
            /// Icon displayed on the error scene.
            /// </summary>
            public Sprite ErrorIcon;
            
        // -- Public Attributes --
            // - Runtime Parameters -
            /// <summary>
            /// Index of the act that is currently playing.
            /// </summary>
            [NonSerialized]
            public int CurrentActIndex;
        
            // - Static References -
            /// <summary>
            /// Reference to the current application's state.
            /// </summary>
            public static ApplicationState Own;
            
            // - Helper Accessors -
            /// <summary>
            /// Returns a reference to the act that is currently being played.
            /// </summary>
            [CanBeNull]
            public ActState CurrentAct {
                get {
                    // Check if the act index value is valid.
                    if (this.CurrentActIndex >= 0 && this.CurrentActIndex < this.ActList.Count) {
                        // Return the act reference.
                        return this.ActList[index: this.CurrentActIndex];
                    } else {
                        // Return a null.
                        return null;
                    }
                }
            }
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            // - Serialization Events -
            /// <summary>
            /// Event fired right before Unity serializes the objects of the project.
            /// Calls the <see cref="ApplicationController.Serialize"/> method on the instance.
            /// </summary>
            public void OnBeforeSerialize() { ApplicationController.Serialize(state: this); }

            /// <summary>
            /// Event fired right before Unity serializes the objects of the project.
            /// Calls the <see cref="ApplicationController.Deserialize"/> method on the instance.
            /// </summary>
            public void OnAfterDeserialize() { ApplicationController.Deserialize(state: this); }
    // --- /Methods ---
}
}