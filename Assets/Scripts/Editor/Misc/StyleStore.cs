// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Collections.Generic;
using Henshin.Editor.Scene.Transformation;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Misc {

/// <summary>
/// List of all the indices for the <see cref="StyleStore"/>'s dictionary.
/// </summary>
public enum EStyle {
    IconButton, BlackLine, SmallNumberField,
    
    TabBackground, TabCurrent, TabSelectable,
    
    GraphCanvas,
    
    PartHeader,
    
    GraphNode
}

/// <summary>
/// Class used to store all the <see cref="GUIStyle"/> used in the editor.
/// </summary>
public class StyleStore {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Singleton -
            /// <summary>Stores the singleton instance of the class.</summary>
            public static readonly StyleStore Instance = new StyleStore();
            
            // - References -
            public static GUIStyle IconButton => StyleStore.GetStyle(style: EStyle.IconButton);
            public static GUIStyle BlackLine => StyleStore.GetStyle(style: EStyle.BlackLine);
            public static GUIStyle SmallNumberField => StyleStore.GetStyle(style: EStyle.SmallNumberField);
            public static GUIStyle TabBackground => StyleStore.GetStyle(style: EStyle.TabBackground);
            
            public static GUIStyle TabCurrent => StyleStore.GetStyle(style: EStyle.TabCurrent);
            
            public static GUIStyle TabSelectable => StyleStore.GetStyle(style: EStyle.TabSelectable);
            public static GUIStyle PartHeader => StyleStore.GetStyle(style: EStyle.PartHeader);
            public static GUIStyle GraphCanvas => StyleStore.GetStyle(style: EStyle.GraphCanvas);
            public static GUIStyle GraphNode => StyleStore.GetStyle(style: EStyle.GraphNode);
        // -- Private Attributes --
            /// <summary>Dictionary of all the textures.</summary>
            private readonly Dictionary<EStyle, GUIStyle> _mStyles = new Dictionary<EStyle, GUIStyle>();
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Loads all the styles.
            /// </summary>
            private StyleStore() { this._LoadStyles(); }
            
        // -- Public Methods --
            /// <summary>Indexer access to the underlying objects. </summary>
            /// <param name="style">The index of the requested object.</param>
            public GUIStyle this[EStyle style] => this._mStyles[key: style];
            
            /// <returns>The requested style object.</returns>
            public static GUIStyle GetStyle(EStyle style) { return StyleStore.Instance[style: style]; }
            
        // -- Private Methods --
            /// <summary>Load all the textures.</summary>
            private void _LoadStyles() {
                // Load the tabs background header style.
                {
                    GUIStyle tabBackground = new GUIStyle { normal = { background = Texture2D.blackTexture }};
                    this._mStyles[key: EStyle.TabBackground] = tabBackground;
                }
                
                // Load the tab style.
                {
                    GUIStyle tab = new GUIStyle(other: EditorStyles.toolbarButton);
                    this._mStyles[key: EStyle.TabSelectable] = tab;
                }
                
                // Load the tab style.
                {
                    GUIStyle tab = new GUIStyle(other: EditorStyles.toolbarButton) {
                        fontStyle = FontStyle.Bold
                    };
                    this._mStyles[key: EStyle.TabCurrent] = tab;
                }
                
                
                // Load the icon button style.
                {
                    GUIStyle iconButton = new GUIStyle(other: EditorStyles.miniButton);
                    iconButton.fixedWidth = iconButton.fixedHeight;
                    iconButton.padding = new RectOffset { top = 1, bottom = 1, left = 1, right = 1 };
                    this._mStyles[key: EStyle.IconButton] = iconButton;
                }
                
                // Load the line style.
                {
                    GUIStyle blackLine = new GUIStyle {stretchWidth = true, normal = {background = TextureStore.Black}};
                    this._mStyles[key: EStyle.BlackLine] = blackLine;
                }
                
                // Load the small number field style.
                {
                    GUIStyle smallNumField = new GUIStyle(other: EditorStyles.numberField) {fixedWidth = 32, alignment = TextAnchor.MiddleRight};
                    this._mStyles[key: EStyle.SmallNumberField] = smallNumField;
                }
                
                // Load the part header style.
                {
                    GUIStyle style = new GUIStyle(other: EditorStyles.label) {
                        fontSize = 13,
                        fontStyle = FontStyle.Bold,
                        padding = { left = 4 },
                        normal = { background = TextureStore.Header }
                    };
                    this._mStyles[key: EStyle.PartHeader] = style;
                }
                
                {
                    GUIStyle style = new GUIStyle {
                        normal = { background = TextureStore.GraphBackground },
                        fixedWidth =  TransformationEditorCanvas.CanvasSize.x,
                        fixedHeight = TransformationEditorCanvas.CanvasSize.y,
                    };
                    style.normal.background.wrapMode = TextureWrapMode.Repeat;
                    this._mStyles[key: EStyle.GraphCanvas] = style;
                }
                {
                    GUIStyle style = new GUIStyle(other: EditorStyles.toolbarButton) {
                        fixedHeight = 0, fixedWidth = 0,
                    };
                    this._mStyles[key: EStyle.GraphNode] = style;
                }
            }
    // --- /Methods ---
}
}