// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using Henshin.Runtime.Application;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components {

/// <summary>
/// Component used for all the actors that can be dragged by the user.
/// Uses Physics2D to compute collisions.
/// </summary>
[RequireComponent(
    requiredComponent: typeof(Image),
    requiredComponent2: typeof(GraphicRaycaster)
),AddComponentMenu(menuName: "Henshin/Gameplay/Draggable", order: 01)]
public class DraggableComponent: MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Identifiers -
            /// <summary>
            /// Identifier used to define what this object is.
            /// </summary>
            [NonSerialized]
            public string Identifier;
            
            // - Flags -
            /// <summary>
            /// Flag set if the draggable object can be placed on top of the wrong <see cref="DropTargetComponent"/>.
            /// </summary>
            public bool CanBeWrong { get; private set; }
            
            /// <summary>
            /// Flag set if the draggable object should duplicate itself on drag.
            /// </summary>
            public bool CreateDuplicate { get; private set; }
            
            // - Components -
            /// <summary>
            /// Reference to the <see cref="UnityEngine.RectTransform"/> on this game object.
            /// </summary>
            [NonSerialized] 
            public RectTransform RectTransform;
            
        // -- Private Attributes --
            // - Components -
            /// <summary>
            /// Reference to the <see cref="UnityEngine.UI.Image"/> on this game object.
            /// </summary>
            [NonSerialized] 
            private Image _mImage;
            
            // - References -
            /// <summary>
            /// List of all the drop targets that were found in the scene.
            /// Updated on <see cref="OnBeginDrag"/>.
            /// </summary>
            private DropTargetComponent[] _mTargets;
            
            /// <summary>
            /// Reference to the currently overlapped area.
            /// </summary>
            private DropTargetComponent _mOverlappedTarget;
            
            /// <summary>
            /// Reference to the owner of this object.
            /// Used to reattach the object to its parent after dropping.
            /// </summary>
            private Transform _mOwner;
            
            /// <summary>
            /// Callback method called upon validation of the draggable object.
            /// </summary>
            private UnityAction _mCallback;
            
            // - Flags -
            /// <summary>
            /// Flag set if the area the object's drop area is valid.
            /// </summary>
            private bool _mDropAreaIsCorrect;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called right after the object is created.
            /// </summary>
            private void Awake() {
                // Get the components of the game object.
                this.RectTransform = this.GetComponent<RectTransform>();
                this._mImage = this.GetComponent<Image>();
            }

        // -- Public Methods --
            // - UI Events -
            /// <summary>
            /// Called when the user begins to drag the game object on the screen.
            /// </summary>
            /// <param name="eventData">The info about the current drag event.</param>
            public void OnBeginDrag(PointerEventData eventData) {
                // Check if the object should duplicate itself.
                if (this.CreateDuplicate) {
                    // Create a new instance of the object.
                    DraggableComponent clone = Object.Instantiate(
                        original: this.gameObject, 
                        parent: this.transform.parent, 
                        worldPositionStays: false
                    ).GetComponent<DraggableComponent>();
                    
                    // Prepare the instance.
                    clone.Prepare(
                        identifier: this.Identifier, 
                        callback: this._mCallback, 
                        canBeWrong: this.CanBeWrong, 
                        createDuplicate: this.CreateDuplicate
                    );
                }
                // Get the list of all the targets in the scene.
                this._mTargets = Object.FindObjectsOfType<DropTargetComponent>();
                
                // Get a reference to the current canvas.
                this._mOwner = this.transform.parent;
                // Attach to the root of the current canvas.
                this.transform.SetParent(parent: this._mImage.canvas.rootCanvas.transform, worldPositionStays: true);
                
                // Move the image to the top of the render list.
                this._mImage.canvas.sortingOrder = 1000;
            }
            
            /// <summary>
            /// Called when the user moves the character on the screen.
            /// </summary>
            /// <param name="eventData">The info about the current drag event.</param>
            public void OnDrag(PointerEventData eventData) {
                // Move the actor to that location on the canvas.
                this.transform.localPosition = ApplicationView.ScreenToCanvas(point: eventData.position);
                
                // Clear the flags.
                this._mOverlappedTarget = null;
                this._mDropAreaIsCorrect = false;
                
                // Get the rect of the object.
                Rect ownRect = new Rect(
                    position: this.RectTransform.position, 
                    size: ApplicationView.CanvasToWorld(point: this.RectTransform.sizeDelta)
                );
                
                // Loop through all the enabled targets.
                foreach (DropTargetComponent target in this._mTargets.Where(predicate: target => target.enabled)) {
                    // Get the rect of the object.
                    Rect targetRect = new Rect(
                        position: target.RectTransform.position, 
                        size: ApplicationView.CanvasToWorld(point: target.RectTransform.sizeDelta)
                    );
                    
                    // If both their rects overlap.
                    if (ownRect.Overlaps(other: targetRect)) {
                        // Check if the drop area is the correct one.
                        if (target.ExpectedIdentifier == this.Identifier) {
                            // Store the valid reference.
                            this._mOverlappedTarget = target;
                            // Set the flag.
                            this._mDropAreaIsCorrect = true;
                            
                            // Stop the method.
                            return; 
                        } else {
                            // Store the reference.
                            this._mOverlappedTarget = target;
                        }
                    }
                }
            }
            
            /// <summary>
            /// Called when the user drops the game object on the screen.
            /// </summary>
            /// <param name="eventData">The info about the current drag event.</param>
            public void OnEndDrag(PointerEventData eventData) {
                // Move the image back to its original position.
                this._mImage.canvas.sortingOrder = 0;
                
                // Check if the target was valid.
                if (this._mOverlappedTarget != null) {
                    // Check if the target is valid.
                    if (this._mDropAreaIsCorrect || this.CanBeWrong) {
                        // Attach the actor to the target.
                        this.transform.SetParent(p: this._mOverlappedTarget.transform);
                        // Place the actor at the target's position.
                        this.RectTransform.anchoredPosition = 
                            this.RectTransform.sizeDelta / 2 * new Vector2(x: 1, y: -1);
                        
                        // Set the image's color.
                        this._mImage.color = this._mDropAreaIsCorrect ? Color.green : Color.red;
                        
                        // Disable the components.
                        this._mOverlappedTarget.enabled = false;
                        this.enabled = false;
                        
                        // Call the callback.
                        this._mCallback?.Invoke();
                        
                        // Stop the method.
                        return;
                    }
                }
                
                // If the instances are duplicated.
                if (this.CreateDuplicate) {
                    // Destroy the instance.
                    Object.Destroy(obj: this.gameObject);
                } else {
                    // Re-attach to the owner.
                    this.transform.SetParent(parent: this._mOwner, worldPositionStays: true);
                }
            }
            
            // - Initialization -
            /// <summary>
            /// Prepares the current draggable object.
            /// Set its <see cref="Identifier"/> and <see cref="_mCallback"/> references.
            /// </summary>
            /// <param name="identifier">The identifier of the component.</param>
            /// <param name="callback">The method to call once the object has finished.</param>
            /// <param name="canBeWrong">If true, allows for bad answers.</param>
            /// <param name="createDuplicate">If true, create duplicates of the word instead of using the one instance.</param>
            public void Prepare(string identifier, UnityAction callback, bool canBeWrong, bool createDuplicate) {
                // Store the references.
                this.Identifier = identifier;
                this._mCallback = callback;
                this.CanBeWrong = canBeWrong;
                this.CreateDuplicate = createDuplicate;
            }
            
            /// <summary>
            /// Create a new draggable component in the scene.
            /// Attaches the object to the specified toolbox.
            /// </summary>
            /// <param name="identifier">The identifier of the component.</param>
            /// <param name="callback">The callback that must be called after the component is done.</param>
            /// <param name="canBeWrong">If true, allows for bad answers.</param>
            /// <param name="createDuplicate">If true, create a duplicate of the object on each drag.</param>
            /// <param name="size">The size to apply to the object.</param>
            /// <param name="toolbox">The toolbox that the component will be attached to.</param>
            public static DraggableComponent Instantiate(
                string identifier, 
                UnityAction callback, 
                bool canBeWrong, 
                bool createDuplicate,
                Vector2 size,
                ToolBoxComponent toolbox
            ) {
                // Create a new instance.
                GameObject draggableObj = new GameObject();
                // Set its name.
                draggableObj.name = $"{identifier} - Source";
                
                // Get the component on the object.
                if (!(draggableObj.GetComponent<DraggableComponent>() is DraggableComponent component)) {
                    // Throw an exception.
                    ApplicationView.Error(
                        message: "The current ApplicationState has no DraggableComponent on its Draggable prefab."
                    );
                    return null;
                }
                
                // Set the anchor of the object.
                component.RectTransform.anchorMin = Vector2.up;
                component.RectTransform.anchorMax = Vector2.up;
                component.RectTransform.pivot = Vector2.one / 2;
                component.RectTransform.sizeDelta = size;
                
                // Attach the instance to the toolbox.
                toolbox.AttachChild(child: component.RectTransform);
                
                // Prepare the instance.
                component.Prepare(
                    identifier: identifier, 
                    callback: callback, 
                    canBeWrong: canBeWrong, 
                    createDuplicate: createDuplicate
                );
                
                // Get the text component in the children objects.
                if (!(draggableObj.GetComponentInChildren<Text>() is Text text)) {
                    // Throw an exception.
                    ApplicationView.Error(
                        message: "The current ApplicationState has no Text child component on its Draggable prefab."
                    );
                    return null;
                }
                
                // Set the text and size of the component.
                text.text = identifier;
                
                // Return the instance.
                return component;
            }
    // --- /Methods ---
}
}