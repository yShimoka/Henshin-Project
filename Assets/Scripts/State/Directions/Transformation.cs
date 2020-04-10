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
public class Transformation {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Tree Behaviour -
            /// <summary>List of all the children nodes of this transformation.</summary>
            [HideInInspector]
            public List<int> nodeIndices;
            
            /// <summary>Type of the controller that will handle this state.</summary>
            public Type controller;
            
            // - Transformation Data -
            /// <summary>Actor that is being transformed.</summary>
            public Scenery.Actor actor;
            
        // -- Public Attributes --
    // --- /Attributes ---
}
}