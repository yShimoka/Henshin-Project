// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Runtime.Directions.Act;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea {

/// <summary>
/// Assetable object that is used to store all the GraphAreas of an <see cref="ActState"/>.
/// Every <see cref="GraphAreaState"/> refers to its owner scene. 
/// </summary>
public class GraphAreaStore: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - References -
            /// <summary>
            /// Hash of the <see cref="ActState"/> that owns this store object.
            /// </summary>
            public int OwnerHash;
            
            /// <summary>
            /// List of all the <see cref="GraphAreaState"/> that belong to this store.
            /// </summary>
            public List<GraphAreaState> GraphList;
            
        // -- Public Attributes --
            /// <summary>
            /// List of all the stores found in the project.
            /// Each store refers to an <see cref="ActState"/>.
            /// </summary>
            public static GraphAreaStore[] StoreList;
            
            /// <summary>
            /// Reference to the <see cref="ActState"/> that owns this store.
            /// </summary>
            [NonSerialized]
            public ActState Owner;
    // --- /Attributes ---
}
}