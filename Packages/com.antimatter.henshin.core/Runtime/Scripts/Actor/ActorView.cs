// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Runtime.Actor {

/// <summary>
/// View class used to represent actors in the scene.
/// </summary>
public static class ActorView {
    // ---  Methods ---
        // -- Public Methods --
            // - Instantiation -
            /// <summary>
            /// Instantiates the specified actor in the scene.
            /// </summary>
            /// <param name="actor">The actor object to instantiate.</param>
            public static void InstantiateActor(ActorState actor) {
                // Copy the actor's prefab into the scene.
                actor.Instance = UnityEngine.Object.Instantiate(
                    original: actor.Prefab,
                    position: UnityEngine.Vector3.zero,
                    rotation: UnityEngine.Quaternion.identity, 
                    parent: Runtime.Application.ApplicationView.Stage
                );
                
                // Seek the actor's sprite renderer.
                if (!(
                    actor.Instance.GetComponent<UnityEngine.SpriteRenderer>() 
                    is UnityEngine.SpriteRenderer renderer
                )) {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Actor \"{actor.Identifier}\"'s prefab has no SpriteRenderer Component !"
                    );
                    // Stop the method.
                    return;
                }
                
                // Store the reference.
                actor.SpriteRenderer = renderer;
                
                // Set its world position.
                actor.Instance.transform.localPosition = UnityEngine.Vector3.zero;
                // Load its default pose.
                ActorView.SetPose(actor: actor, poseIndex: 0);
                // Set the layout of the actor.
                ActorView.SetLayer(actor: actor, layerId: Runtime.Application.ApplicationView.SortingLayers.Middleground);
                
                // Set the name of the actor's game object.
                actor.Instance.name = actor.Identifier;
            }
            
            // - Setters -
            /// <summary>
            /// Updates the pose of the actor.
            /// </summary>
            /// <param name="actor">The actor to update.</param>
            /// <param name="poseIndex">The index of the new pose in the actor's <see cref="ActorState.PoseList"/>.</param>
            public static void SetPose(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                int poseIndex
            ) {
                // Check if the value of the index is valid.
                if (poseIndex >= 0 && poseIndex < actor.PoseList.Count) {
                    // Update the pose of the actor.
                    if (actor.SpriteRenderer != null) {
                        actor.SpriteRenderer.sprite = actor.PoseList[index: poseIndex];
                    } else {
                        // Throw an error.
                        Runtime.Application.ApplicationView.Error(
                            message: $"Tried to load a pose on a non-instantiated actor \"{actor.Identifier}\""
                        );
                    }
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to load an invalid pose #{poseIndex} for actor \"{actor.Identifier}\"",
                        details: $"Actor's pose list is only {actor.PoseList.Count} items long."
                    );
                }
            }
            
            /// <summary>
            /// Updates the sorting layer of the actor.
            /// </summary>
            /// <param name="actor">The actor to update.</param>
            /// <param name="layerId">The actor's new layer ID.</param>
            public static void SetLayer(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                int layerId
            ) {
                // Check if the ID of the layer is valid.
                if (UnityEngine.SortingLayer.IsValid(id: layerId)) {
                    // Update the layer of the actor.
                    if (actor.SpriteRenderer != null) {
                        actor.SpriteRenderer.sortingLayerID = layerId;
                    } else {
                        // Throw an error.
                        Runtime.Application.ApplicationView.Error(
                            message: $"Tried to set the layer of a non-instantiated actor \"{actor.Identifier}\""
                        );
                    }
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to move the actor \"{actor.Identifier}\" to an invalid layer ID #{layerId}"
                    );
                }
            }
            
            /// <summary>
            /// Sets the position of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="position">The new position of the actor.</param>
            public static void SetPosition(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                UnityEngine.Vector2 position
            ) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Update the position of the actor.
                    actor.Instance.transform.position = 
                        Runtime.Application.ApplicationView.WorldCamera.ViewportToWorldPoint(position: position);
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the position of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the angle of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="angle">The new angle of the actor.</param>
            public static void SetAngle(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                float angle
            ) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Update the rotation of the actor.
                    actor.Instance.transform.rotation =
                        UnityEngine.Quaternion.Euler(x: 0, y: 0, z: angle); 
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the rotation of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the scale of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="scale">The new scale of the actor.</param>
            public static void SetScale(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                UnityEngine.Vector2 scale
            ) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Update the scale of the actor.
                    actor.Instance.transform.localScale = scale; 
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the rotation of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the colour of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="colour">The new colour of the actor.</param>
            public static void SetColour(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                UnityEngine.Color colour
            ) {
                // Check if the actor is set.
                if (actor.SpriteRenderer != null) {
                    // Set the colour of the actor.
                    actor.SpriteRenderer.color = colour;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the colour of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the visibility of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="visible">The visibility flag to set on the actor.</param>
            public static void SetVisible(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                bool visible
            ) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Set the visibility of the actor.
                    actor.Instance.SetActive(value: visible);
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the visibility of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the vertical flip flag of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="flipped">The flip flag to set on the actor.</param>
            public static void SetVerticalFlip(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                bool flipped
            ) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Set the flipped state of the actor.
                    actor.SpriteRenderer.flipY = flipped;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the vertical flip of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the horizontal flip flag of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="flipped">The flip flag to set on the actor.</param>
            public static void SetHorizontalFlip(
                [JetBrains.Annotations.NotNullAttribute] ActorState actor, 
                bool flipped
            ) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Set the flipped state of the actor.
                    actor.SpriteRenderer.flipX = flipped;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the horizontal flip of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            // - Getters -
            /// <summary>
            /// Gets the sorting layer of the actor.
            /// </summary>
            /// <param name="actor">The actor to update.</param>
            /// <returns>The id of the layer the actor is currently on.</returns>
            public static int GetLayer([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is valid.
                if (actor.SpriteRenderer != null) {
                    // Return the layer id.
                    return actor.SpriteRenderer.sortingLayerID;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to get the layer of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
            
            /// <summary>
            /// Gets the position of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <returns>The position of the actor in viewport coordinates.</returns>
            public static UnityEngine.Vector3 GetPosition([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the position of the actor.
                    return Runtime.Application.ApplicationView.WorldCamera.WorldToViewportPoint(
                        position: actor.Instance.transform.position
                    );
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to get the position of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
            
            /// <summary>
            /// Gets the angle of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <returns>The angle of the actor.</returns>
            public static float GetAngle([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the rotation of the actor.
                    return actor.Instance.transform.rotation.eulerAngles.z;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to get the rotation of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
            
            /// <summary>
            /// Gets the scale of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <returns>The scale of the actor.</returns>
            public static UnityEngine.Vector2 GetScale([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the scale of the actor.
                    return actor.Instance.transform.localScale;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to get the rotation of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
            
            /// <summary>
            /// Gets the colour of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <returns>The colour of the actor.</returns>
            public static UnityEngine.Color GetColour([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is set.
                if (actor.SpriteRenderer != null) {
                    // Return the colour of the actor.
                    return actor.SpriteRenderer.color;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the colour of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
            
            /// <summary>
            /// Gets the visibility of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <returns>The visibility flag of the actor.</returns>
            public static bool GetVisible([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the visibility of the actor.
                    return actor.Instance.activeInHierarchy;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the visibility of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
            
            /// <summary>
            /// Gets the vertical flip flag of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <returns>The state of the actor's vertical flip flag.</returns>
            public static bool GetVerticalFlip([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the flipped state of the actor.
                    return actor.SpriteRenderer.flipY;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the vertical flip of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
            
            /// <summary>
            /// Gets the horizontal flip flag of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <returns>The state of the actor's horizontal flip flag.</returns>
            public static bool GetHorizontalFlip([JetBrains.Annotations.NotNullAttribute] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the flipped state of the actor.
                    return actor.SpriteRenderer.flipX;
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: $"Tried to update the horizontal flip of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
    // --- /Methods ---
}
}