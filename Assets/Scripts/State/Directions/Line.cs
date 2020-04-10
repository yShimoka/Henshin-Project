// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions {

/// <summary>
/// State class used to represent a single line of the play.
/// Uses <see cref="Transformation"/> to evolve the scene.
/// TODO: Ask the Director if the line is allowed to advance OR use Transformations as gameplay.
/// </summary>
[Serializable]
public class Line: ISerializationCallbackReceiver {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>List of all the transformation states.</summary>
            public List<Transformation> stateList;
            
        // -- Public Attributes --
            /// <summary>List of all the deserialized transformation controllers.</summary>
            [NonSerialized]
            public List<Controller.Directions.Transformation> Transformations;
            
            /// <summary>First transformation of the list.</summary>
            [NonSerialized]
            public Controller.Directions.Transformation RootTransformation;
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
                this.stateList = this.Transformations.Select(selector: transformation => transformation.Serialize()).ToList();
            }

            /// <summary>
            /// Callback called right after the deserialization of the <see cref="Line"/> state.
            /// Rebuilds the transformation list and tree. 
            /// </summary>
            public void OnAfterDeserialize() {
                // Deserialize the transformation list.
                this.Transformations = this.stateList.Select(selector: Controller.Directions.Transformation.Deserialize).ToList();
                
                // Unfold the tree.
                this.RootTransformation = Controller.Directions.Transformation.RebuildTree(from: this.Transformations);
            }
            
        // -- Public Methods --
    // --- /Methods ---
}
}