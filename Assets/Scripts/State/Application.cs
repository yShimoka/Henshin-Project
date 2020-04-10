// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State {

/// <summary>
/// State class used for the entire application.
/// Stored in <see cref="ScriptableObject"/> assets in the resources folder.
/// </summary>
/// <seealso cref="Controller.Application"/>
[CreateAssetMenu(fileName = "DATA_APP_State", menuName = "Henshin/Application State", order = 01)]
public class Application: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - State Info -
            /// <summary>Flag set if this is the state to use for the entire application.</summary>
            //[HideInInspector]
            public bool isAppState;
            
            // - Theatre Info -
            /// <summary>Prefab of the theatre's spectator object.</summary>
            public GameObject spectatorPrefab;
            
            // - Play Info -
            /// <summary>
            /// List of all the acts found in the application.
            /// </summary>
            public List<Directions.Act> acts; 
            
            // - Debugging Attributes -
            /// <summary>Identifier of this act. Used solely for debugging purposes.</summary>
            public string identifier;
            
        // -- Public Attributes --
            // - Constants -
            /// <summary>Path of the <see cref="Application"/> in the resources folder.</summary>
            public const string RESOURCE_PATH = "Serialized/Application";
            
            // - Helper Properties -
            /// <summary>Stores a reference to the currently used <see cref="Application"/> object.</summary>
            /// <seealso cref="Controller.Application.GetCurrent"/>
            public static Application Current;
            
            /// <summary>Stores a list of all the <see cref="Application"/> assets found in the project.</summary>
            /// <seealso cref="Controller.Application.LoadAssets"/>
            public static Application[] Assets;
            
            /// <summary>Returns a reference to the act currently playing.</summary>
            public Directions.Act CurrentAct => this.acts[index: this._mCurrentActIndex];
            
        // -- Private Attributes --
            /// <summary>Counter for the list of acts in the application.</summary>
            private int _mCurrentActIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Increments the act counter.
            /// </summary>
            /// <returns>True if the last act has finished.</returns>
            public bool IncrementActIndex() { this._mCurrentActIndex++; return this._mCurrentActIndex >= this.acts.Count; }
    // --- /Methods ---
}
}