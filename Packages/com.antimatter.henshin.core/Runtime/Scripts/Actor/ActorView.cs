// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Application;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actor {

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
                actor.Instance = Object.Instantiate(
                    original: actor.Prefab,
                    position: Vector3.zero,
                    rotation: Quaternion.identity, 
                    parent: ApplicationView.Stage
                );
                
                // Seek the actor's sprite renderer.
                if (!(
                    actor.Instance.GetComponent<Image>() 
                    is Image renderer
                )) {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Actor \"{actor.Identifier}\"'s prefab has no Image Component !"
                    );
                    // Stop the method.
                    return;
                }
                
                // Seek the actor's canvas.
                if (!(
                    actor.Instance.GetComponent<Canvas>() 
                    is Canvas canvas
                )) {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Actor \"{actor.Identifier}\"'s prefab has no Canvas Component !"
                    );
                    // Stop the method.
                    return;
                }
                
                // Store the reference.
                actor.Image = renderer;
                actor.Canvas = canvas;
                
                // Enable sorting layer overide.
                canvas.overrideSorting = true;
                
                // Set its world position.
                actor.Instance.transform.localPosition = Vector3.zero;
                // Load its default pose.
                ActorView.SetPose(actor: actor, poseIndex: 0);
                // Set the layout of the actor.
                ActorView.SetLayer(actor: actor, layerId: ApplicationView.SortingLayers.Middleground);
                // Set the actor as transparent.
                ActorView.SetColour(actor: actor, colour: new Color(r: 1, g: 1, b: 1, a: 0));
                
                // Set the name of the actor's game object.
                actor.Instance.name = actor.Identifier;
            }
            
            /// <summary>
            /// Clears all the actors from the stage.
            /// </summary>
            public static void ClearActors() {
                // Loop through all the stage children.
                for (int child = 0; child < ApplicationView.Stage.childCount; child++) {
                    // Remove the object at the specified location.
                    Object.Destroy(obj: ApplicationView.Stage.GetChild(index: child).gameObject);
                }
            }
            
            // - Setters -
            /// <summary>
            /// Updates the pose of the actor.
            /// </summary>
            /// <param name="actor">The actor to update.</param>
            /// <param name="poseIndex">The index of the new pose in the actor's <see cref="PoseStore"/>.</param>
            public static void SetPose([NotNull] ActorState actor, int poseIndex) {
                // Check if the value of the index is valid.
                if (poseIndex >= 0 && poseIndex < actor.PoseStore.PoseList.Count) {
                    // Update the pose of the actor.
                    if (actor.Image != null) {
                        // Load the sprite.
                        actor.Image.sprite = actor.PoseStore.PoseList[index: poseIndex];
                        // Set the size of the image.
                        actor.Image.GetComponent<RectTransform>().sizeDelta = actor.Image.sprite.textureRect.size; 
                    } else {
                        // Throw an error.
                        ApplicationView.Error(
                            message: $"Tried to load a pose on a non-instantiated actor \"{actor.Identifier}\""
                        );
                    }
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to load an invalid pose #{poseIndex} for actor \"{actor.Identifier}\"",
                        details: $"Actor's pose list is only {actor.PoseStore.PoseList.Count} items long."
                    );
                }
            }
            
            /// <summary>
            /// Updates the sorting layer of the actor.
            /// </summary>
            /// <param name="actor">The actor to update.</param>
            /// <param name="layerId">The actor's new layer ID.</param>
            public static void SetLayer([NotNull] ActorState actor, int layerId) {
                // Check if the ID of the layer is valid.
                if (SortingLayer.IsValid(id: layerId)) {
                    // Update the layer of the actor.
                    if (actor.Canvas != null) {
                        actor.Canvas.sortingLayerID = layerId;
                    } else {
                        // Throw an error.
                        ApplicationView.Error(
                            message: $"Tried to set the layer of a non-instantiated actor \"{actor.Identifier}\""
                        );
                    }
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to move the actor \"{actor.Identifier}\" to an invalid layer ID #{layerId}"
                    );
                }
            }
            
            /// <summary>
            /// Sets the position of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="position">The new position of the actor.</param>
            public static void SetPosition([NotNull] ActorState actor, Vector2 position) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Update the position of the actor.
                    actor.Instance.transform.position =
                        ((Vector2) ApplicationView.WorldCamera.ViewportToWorldPoint(position: (position + Vector2.one) / 2));
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to update the position of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the angle of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="angle">The new angle of the actor.</param>
            public static void SetAngle([NotNull] ActorState actor, float angle) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Update the rotation of the actor.
                    actor.Instance.transform.rotation =
                        Quaternion.Euler(x: 0, y: 0, z: angle); 
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to update the rotation of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the scale of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="scale">The new scale of the actor.</param>
            public static void SetScale([NotNull] ActorState actor, Vector2 scale) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Check if the actor is inverted.
                    if (ActorView.GetVerticalFlip(actor: actor)) {
                        scale.y *= -1;
                    }
                    if (ActorView.GetHorizontalFlip(actor: actor)) {
                        scale.x *= -1;
                    }
                    // Update the scale of the actor.
                    actor.Instance.transform.localScale = scale; 
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to update the rotation of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the colour of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="colour">The new colour of the actor.</param>
            public static void SetColour([NotNull] ActorState actor, Color colour) {
                // Check if the actor is set.
                if (actor.Image != null) {
                    // Set the colour of the actor.
                    actor.Image.color = colour;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to update the colour of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the visibility of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="visible">The visibility flag to set on the actor.</param>
            public static void SetVisible([NotNull] ActorState actor, bool visible) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Get the color of the actor.
                    Color color = actor.Image.color;
                    
                    // Set the alpha value of the color.
                    color = new Color(
                        r: color.r,
                        g: color.g,
                        b: color.b,
                        a: visible ? 1 : 0
                    );
                    
                    // Set the actor's color.
                    actor.Image.color = color;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to update the visibility of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the vertical flip flag of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="flipped">The flip flag to set on the actor.</param>
            public static void SetVerticalFlip([NotNull] ActorState actor, bool flipped) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Get the local scale of the object.
                    Vector3 localScale = actor.Instance.transform.localScale;
                    
                    // Set the flipped state of the actor.
                    localScale.y = Mathf.Abs(f: localScale.y) * (flipped ? -1 : 1);
                    
                    // Store the scale back.
                    actor.Instance.transform.localScale = localScale;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to update the vertical flip of a non-instantiated actor \"{actor.Identifier}\""
                    );
                }
            }
            
            /// <summary>
            /// Sets the horizontal flip flag of the actor in the scene.
            /// </summary>
            /// <param name="actor">The actor to update the position of.</param>
            /// <param name="flipped">The flip flag to set on the actor.</param>
            public static void SetHorizontalFlip([NotNull] ActorState actor, bool flipped) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Get the local scale of the object.
                    Vector3 localScale = actor.Instance.transform.localScale;
                    
                    // Set the flipped state of the actor.
                    localScale.x = Mathf.Abs(f: localScale.x) * (flipped ? -1 : 1);
                    
                    // Store the scale back.
                    actor.Instance.transform.localScale = localScale;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static int GetLayer([NotNull] ActorState actor) {
                // Check if the actor is valid.
                if (actor.Image != null) {
                    // Return the layer id.
                    return actor.Canvas.sortingLayerID;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static Vector3 GetPosition([NotNull] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the position of the actor.
                    return (Vector2)ApplicationView.WorldCamera.WorldToViewportPoint(
                        position: actor.Instance.transform.position
                    ) * 2 - Vector2.one;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static float GetAngle([NotNull] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the rotation of the actor.
                    return actor.Instance.transform.rotation.eulerAngles.z;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static Vector2 GetScale([NotNull] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Get the actor's scale.
                    Vector3 scale = actor.Instance.transform.localScale;
                    
                    // Check if the actor is inverted.
                    if (ActorView.GetVerticalFlip(actor: actor)) {
                        scale.y *= -1;
                    }
                    if (ActorView.GetHorizontalFlip(actor: actor)) {
                        scale.x *= -1;
                    }
                    
                    // Return the scale.
                    return scale;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static Color GetColour([NotNull] ActorState actor) {
                // Check if the actor is set.
                if (actor.Image != null) {
                    // Return the colour of the actor.
                    return actor.Image.color;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static bool GetVisible([NotNull] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the visibility of the actor.
                    return actor.Image.color.a > 0.01f;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static bool GetVerticalFlip([NotNull] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the flipped state of the actor.
                    return actor.Instance.transform.localScale.y < 0;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
            public static bool GetHorizontalFlip([NotNull] ActorState actor) {
                // Check if the actor is set.
                if (actor.Instance != null) {
                    // Return the flipped state of the actor.
                    return actor.Instance.transform.localScale.x < 0;
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: $"Tried to update the horizontal flip of a non-instantiated actor \"{actor.Identifier}\""
                    );
                    return default;
                }
            }
    // --- /Methods ---
}
}