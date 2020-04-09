// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Misc {

/// <summary>
/// Helper class used for GUIs.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class EditorGUIHelper {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders an asset field.
            /// Stores the altered asset object in the specified <see cref="property"/>.
            /// </summary>
            /// <param name="property">The base property where the asset is stored.</param>
            /// <param name="baseFolders">(Optional) The folder in which the asset will be searched.</param>
            /// <typeparam name="T">The type of the asset.</typeparam>
            public static void AssetField<T>(SerializedProperty property, params string[] baseFolders) where T : Object {
                // Get the value from the property.
                string assetName;
                switch (property.propertyType) {
                    case SerializedPropertyType.String: assetName = property.stringValue; break;
                    case SerializedPropertyType.ObjectReference: assetName = property.objectReferenceValue.name; break;
                    default: throw new ArgumentOutOfRangeException(paramName: nameof(property), message: $"Property was of unsupported type: {property.propertyType.GetType().Name}");
                }
                
                // Render the field.
                Object renderedAsset = EditorGUIHelper.AssetField<T>(assetName: assetName, baseFolders: baseFolders);
                
                // Update the value of the property.
                switch (property.propertyType) {
                    case SerializedPropertyType.String: property.stringValue = renderedAsset.name; break;
                    case SerializedPropertyType.ObjectReference: property.objectReferenceValue = renderedAsset; break;
                    default: break;
                }
            }
            
            /// <summary>
            /// Renders an asset field.
            /// </summary>
            /// <param name="assetName">The name of the asset to draw.</param>
            /// <param name="baseFolders">(Optional) The folder in which the asset will be searched.</param>
            /// <typeparam name="T">The type of the asset.</typeparam>
            /// <returns>The modified asset object.</returns>
            public static T AssetField<T>(string assetName, params string[] baseFolders) where T : Object {
                Object renderedAsset = null;
                
                // Try to load the asset.
                if (!string.IsNullOrEmpty(value: assetName)) {
                    // Load all the assets.
                    T[] assets = Helper.LoadAssets<T>(name: assetName, filter: "", baseFolders: baseFolders);
                    
                    // Check if there is a single asset.
                    if (assets.Length == 1) {
                        // Store the asset.
                        renderedAsset = assets[0];
                    } else if (assets.Length > 1) {
                        // Store the asset.
                        renderedAsset = assets[0];
                        
                        // Log a warning.
                        Debug.LogWarning(message: $"Found multiple assets named {assetName} with type {typeof(T).Name}");
                    } else if (assets.Length == 0) {
                        // Log an error.
                        Debug.LogError(message: $"Found no asset named {assetName} with type {typeof(T).Name}");
                    }
                }
                
                // Render the field.
                return EditorGUILayout.ObjectField(label: Helper.PrettifyCamel(camel: assetName), obj: renderedAsset, objType: typeof(T), allowSceneObjects: false) as T;
            }
            
            /// <summary>
            /// Renders a drop target asset field.
            /// Returns the altered asset object.
            /// </summary>
            /// <param name="fieldName">The name of the field to display.</param>
            /// <typeparam name="T">The type of the asset.</typeparam>
            public static T AssetDropArea<T>(string fieldName) where T : Object {
                // Render the field.
                return EditorGUILayout.ObjectField(label: fieldName, obj: null, objType: typeof(T), allowSceneObjects: false) as T;
            }

            /// <summary>
            /// Draws a simple horizontal line.
            /// </summary>
            /// <param name="width">The width of the line.</param>
            /// <param name="y">The y position of the line.</param>
            /// <param name="height">The height of the line.</param>
            /// <param name="x">The x position of the line.</param>
            public static void HorizontalLine(float width, float y = 0, float height = 1, float x = 0) {
                // Draw the line object.
                GUI.Box(position: new Rect{ x = x, y = y, width = width, height = height }, text: "", style: StyleStore.BlackLine);
            }

            /// <summary>
            /// Draws a simple vertical line.
            /// </summary>
            /// <param name="height">The height of the line.</param>
            /// <param name="x">The x position of the line.</param>
            /// <param name="width">The width position of the line.</param>
            /// <param name="y">The y position of the line.</param>
            public static void VerticalLine(float height, float x = 0, float width = 1, float y = 0) {
                // Draw the line object.
                GUI.Box(position: new Rect{ x = x, y = y, width = width, height = height }, text: "", style: StyleStore.BlackLine);
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}