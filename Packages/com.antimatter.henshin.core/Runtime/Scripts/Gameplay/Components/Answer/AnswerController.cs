// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using Henshin.Runtime.Application;
using Henshin.Runtime.Gameplay.Components.Target;
using Henshin.Runtime.Libraries;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Answer {

/// <summary>
/// Controller class for one of the answers of the game.
/// Answer objects are draggable on the screen and are used
/// to fill the gaps in most gameplay modes.
/// Inherits from <see cref="Graphic"/>, but does not render anything.
/// This is done to receive UI Raycast callbacks through <see cref="IDragHandler"/> for example.
/// </summary>
[AddComponentMenu(menuName: "Henshin/Gameplay/Answer")]
public class AnswerController: EmptyGraphic, IDragHandler, IBeginDragHandler, IEndDragHandler {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Instance of the controller's state class.
            /// </summary>
            [NonSerialized]
            public AnswerState State;
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called upon instantiation of the component.
            /// Loads all required elements.
            /// </summary>
            protected override void Awake() {
                // Store the instance of the controller in the state.
                this.State = new AnswerState(owner: this);
                
                // Seek a text component somewhere in the game object.
                if (this.GetComponent<Text>() is Text ownText) {
                    // Store the text in the state.
                    this.State.Text = ownText;
                } else if (this.GetComponentInChildren<Text>() is Text childText) {
                    // Store the text in the state.
                    this.State.Text = childText;
                }
                
                // Seek an image component somewhere in the game object.
                if (this.GetComponent<Image>() is Image ownImage) {
                    // Store the image in the state.
                    this.State.Image = ownImage;
                } else if (this.GetComponentInChildren<Image>() is Image childImage) {
                    // Store the image in the state.
                    this.State.Image = childImage;
                }
            }
            
        // -- Public Methods --
            // - Drag Triggers -
            /// <summary>
            /// Event triggered when the object is moved on the screen.
            /// Updates the render position of the object.
            /// </summary>
            /// <param name="eventData">The info about the drag event.</param>
            public void OnDrag(PointerEventData eventData) {
                // Get the canvas position of the mouse.
                Vector2 canvasMousePos = ApplicationView.ScreenToCanvas(point: eventData.position);
                
                // Update the position of the answer.
                this.State.Transform.localPosition = canvasMousePos + new Vector2(
                    x: -this.State.Transform.sizeDelta.x / 2,
                    y: this.State.Transform.sizeDelta.y / 2
                );
                
                // Update the world rect of the object.
                this.State.WorldRect.position = ApplicationView.CanvasToWorld(point: canvasMousePos);
            }

            /// <summary>
            /// Event triggered when the object has just been picked up.
            /// Creates a copy of the item if the <see cref="GameplayState.DuplicateAnswers"/> flag is set.
            /// Places the moved object under the <see cref="ApplicationView.GUI"/>.
            /// </summary>
            /// <param name="eventData">The info about the pick up event.</param>
            public void OnBeginDrag(PointerEventData eventData) {
                // Check if the duplicate flag is set.
                if (GameplayState.DuplicateAnswers) {
                    // Create a duplicate instance.
                    GameObject copy = Object.Instantiate(
                        original: this.gameObject, 
                        parent: this.State.Parent,
                        position: this.transform.position,
                        rotation: this.transform.rotation
                    );
                    
                    // Set the copy's value and callback.
                    copy.GetComponent<AnswerController>().State.Value = this.State.Value;
                    copy.GetComponent<AnswerController>().State.Callback = this.State.Callback;
                    // Set its name.
                    copy.name = this.gameObject.name;
                }
                
                // Place at the end of the owner's list.
                this.transform.SetAsLastSibling();
                // Attach to the GUI layer.
                this.transform.SetParent(parent: ApplicationView.GUI, worldPositionStays: true);
                
                // Find all the drop targets in the scene.
                this.State.PossibleTargets = ApplicationView.GUI
                    .GetComponentsInChildren<TargetController>(includeInactive: true)
                    .Where(predicate: target => target.enabled)
                    .ToArray();
                    
                // Get the size of the object.
                this.State.WorldRect.size = ApplicationView.CanvasToWorld(point: this.State.Transform.sizeDelta);
            }

            /// <summary>
            /// Event triggered when the object has just been dropped.
            /// If the <see cref="GameplayState.DuplicateAnswers"/> is set, deletes this copy.
            /// </summary>
            /// <param name="eventData">The info about the drop event.</param>
            public void OnEndDrag(PointerEventData eventData) {
                // Check if the object is overlapping a possible target.
                TargetController overlapped = null;
                foreach (TargetController target in this.State.PossibleTargets) {
                    // Check if both of their world rects overlapp.
                    if (target.State.WorldRect.Overlaps(other: this.State.WorldRect)) {
                        // Store the overlapped reference.
                        overlapped = target;
                        
                        // If the target has the same value as us.
                        if (target.State.Value == this.State.Value) {
                            // Stop searching.
                            break;
                        }
                    }
                }
                
                // If the object is overlapping a target.
                if (overlapped != null) {
                    // Attach to the target.
                    this.transform.SetParent(parent: overlapped.transform, worldPositionStays: false);
                    // Place the object on top of the target.
                    this.transform.position = overlapped.transform.position;
                    
                    // If the answer has an image.
                    if (this.State.Image != null) {
                        // Dray the image to signal that the answer is placed.
                        this.State.Image.color = Color.gray;
                    }
                    
                    // Store the answer in the target.
                    overlapped.State.PlacedAnswer = this.State;
                    
                    // Disable the component.
                    this.enabled = false;
                    
                    // Disable the target.
                    overlapped.enabled = false;
                    
                    // If it is set, call the callback.
                    this.State.Callback?.Invoke(arg0: this, arg1: overlapped);
                } else {
                    // If the duplicate flag is set.
                    if (GameplayState.DuplicateAnswers) {
                        // Delete this instance.
                        Object.Destroy(obj: this.gameObject);
                    } else {
                        // Reattach to the original parent.
                        this.transform.SetParent(parent: this.State.Parent, worldPositionStays: true);
                    }
                }
                
            }
            
            // - Instance Management -
            /// <summary>
            /// Instantiates a new <see cref="AnswerController"/> in the scene.
            /// Uses the prefab found in <see cref="GameplayState.TextAnswerPrefab"/>.
            /// </summary>
            /// <param name="parent">The <see cref="Transform"/> to which the new instance will be attached.</param>
            /// <returns>The created <see cref="AnswerState"/> instance.</returns>
            public static AnswerState Instantiate(Transform parent) {
                // Seek the prefab in the current gameplay state.
                if (GameplayState.Own.TextAnswerPrefab is GameObject prefab) {
                    // Instantiate the prefab.
                    GameObject owner = Object.Instantiate(
                        original: prefab,
                        parent: parent,
                        position: Vector3.zero,
                        rotation: Quaternion.identity
                    );
                    // Reset its position to ensure that it is at (0, 0, 0).
                    owner.transform.localPosition = Vector3.zero;
                    // Set its name.
                    owner.name = "Answer Instance";
                    
                    // Seek the answer controller.
                    if (owner.GetComponent<AnswerController>() is AnswerController controller) {
                        // Disable the component.
                        controller.enabled = false;
                        
                        // Return the instance.
                        return controller.State;
                    } else {
                        ApplicationView.Error(
                            message: "The specified TextAnswerPrefab does not have an AnswerController component."
                        );
                        return null;
                    }
                } else {
                    ApplicationView.Error(
                        message: "Tried to instance a new AnswerController but no " +
                                 "TextAnswerPrefab was specified in the GameplayState instance."
                    );
                    return null;
                }
            }

        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}