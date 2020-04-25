// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions {

/// <summary>
/// State class used to represent an <see cref="Act"/>'s scene.
/// <see cref="Scene"/>s are filled with <see cref="Line"/>s that are played in sequence.
/// </summary>
[CreateAssetMenu(menuName = "Henshin/Scene", fileName = "DATA_SCENE_Scene")]
public class Scene: ScriptableObject, ISerializationCallbackReceiver {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>Identifier of the current scene.</summary>
            public string identifier;
            
            /// <summary>List of all the transformation states.</summary>
            //[HideInInspector]
            public List<Transformation> stateList;
            
            /// <summary>List of all the actors that take part in the scene.</summary>
            //[HideInInspector]
            public List<Scenery.Actor> actors;
            
            /// <summary>Reference to the sprite used as this scene's background.</summary>
            public Sprite background;
            
#if UNITY_EDITOR
            /// <summary>Flag set if the scene should be played for testing.</summary>
            public bool testScene;
#endif
            
        // -- Public Attributes --
            /// <summary>List of all the deserialized transformation controllers.</summary>
            [NonSerialized]
            public List<Controller.Directions.Transformation> Transformations = new List<Controller.Directions.Transformation>();
            
            /// <summary>First transformation of the list.</summary>
            [NonSerialized]
            public Controller.Directions.Transformation RootTransformation;
            
        // -- Private Attributes --
            /// <summary>Index of the line that is currently playing.</summary>
            private int _mCurrentLineIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            // - Serialization Events -
            /// <summary>
            /// Called right before the <see cref="Line"/> object is serialized.
            /// 
            /// </summary>
            public void OnBeforeSerialize() {
                // Serialize the entire transformation list.
                this.stateList = this.Transformations.Select(selector: transformation => transformation.Serialize()).Select(selector: serialized => serialized.State).ToList();
                if (this.stateList.Count == 0) Debug.LogWarning(message: "The state list was empty !");
            }

            /// <summary>
            /// Callback called right after the deserialization of the <see cref="Line"/> state.
            /// Rebuilds the transformation list and tree. 
            /// </summary>
            public void OnAfterDeserialize() {
                // Deserialize the transformation list.
                this.Transformations = this.stateList.Select(selector: Controller.Directions.Transformation.Deserialize).ToList();
                
                // Unfold the tree.
                if (this.Transformations != null) {
                    this.RootTransformation = Controller.Directions.Transformation.RebuildTree(from: this.Transformations);
                }
            }
            
    // --- /Methods ---
}
}