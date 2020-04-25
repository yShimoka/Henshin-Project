// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Directions {

/// <summary>
/// Simple serializable class that represents the state of transformation objects.
/// </summary>
[Serializable]
public class Transformation: ISerializationCallbackReceiver {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Tree Behaviour -
            /// <summary>List of all the children nodes of this transformation.</summary>
            //[HideInInspector]
            public List<int> nodeIndices = new List<int>();
            
            // - Transformation Data -
            /// <summary>Actor that is being transformed.</summary>
            public Scenery.Actor actor;
            
            /// <summary>Stringified version of the data.</summary>
            [SerializeField]
            public List<string> encodedData = new List<string>();
            
            // - Private Fields -
            /// <summary>Stores the name of the controller type.</summary>
            [SerializeField]
            private string controllerTypeName;
            
        // -- Public Attributes --
            /// <summary>Type of the controller that will handle this state.</summary>
            [NonSerialized]
            public Type controller;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            public void OnBeforeSerialize() {
                // Store the controller type name.
                this.controllerTypeName = this.controller.FullName;
            }

            public void OnAfterDeserialize() {
                // Load the controller type.
                this.controller = System.Reflection.Assembly.GetAssembly(type: typeof(Transformation)).GetType(name: this.controllerTypeName);
            }
            
        // -- Public Methods --
            public Transformation() {}
            public Transformation(Transformation from) {
                this.actor = from.actor;
                this.controller = from.controller;
                this.encodedData = new List<string>(collection: from.encodedData);
                this.nodeIndices = from.nodeIndices;
                this.controllerTypeName = from.controllerTypeName;
            }
    // --- /Methods ---
}
}