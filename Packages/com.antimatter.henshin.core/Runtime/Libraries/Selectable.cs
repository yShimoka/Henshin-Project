// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Libraries {

/// <summary>
/// Implementation of the <see cref="EmptyGraphic"/> class that can receive click and overlapp events.
/// </summary>
[RequireComponent(requiredComponent: typeof(RectTransform), requiredComponent2: typeof(CanvasRenderer))]
public class Selectable: EmptyGraphic, IPointerClickHandler {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Event invoked when the user clicks on the element.
            /// </summary>
            [NonSerialized, CanBeNull]
            public UnityAction OnClick;
             
            /// <summary>
            /// Flag set if the selectable is selected.
            /// </summary>
            [NonSerialized]
            public bool Selected;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <inheritdoc cref="Graphic.SetMaterialDirty"/>
            public override void SetMaterialDirty() {}
            
            /// <inheritdoc cref="Graphic.SetVerticesDirty"/>
            public override void SetVerticesDirty() {}
            
            /// <inheritdoc cref="IPointerClickHandler.OnPointerClick"/>
            public void OnPointerClick(PointerEventData eventData) => this.OnClick?.Invoke();
            
        // -- Protected Methods --
            /// <inheritdoc cref="Graphic.OnPopulateMesh(VertexHelper)"/>
            protected override void OnPopulateMesh(VertexHelper vh) => vh.Clear();
    // --- /Methods ---
}
}