// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.State.Scenery {

/// <summary>
/// State representing an actor that can be instanced in a scene.
/// </summary>
[Serializable, CreateAssetMenu(fileName = "DATA_ACTOR_#ActorName#", menuName = "Henshin/Actor", order = 02)]
public class Actor: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>Reference to the prefab of this actor.</summary>
            public GameObject actorPrefab;
            
        // -- Public Attributes --
            /// <summary>Reference to this <see cref="Actor"/>'s actor component in the scene.</summary>
            [NonSerialized]
            public View.Scenery.Actor ActorComponent;
    // --- /Attributes ---
}
}