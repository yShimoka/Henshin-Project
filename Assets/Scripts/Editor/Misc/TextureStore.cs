// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Misc {

/// <summary>
/// List of all the indices for the <see cref="TextureStore"/>'s dictionary.
/// </summary>
public enum ETexture {
    // Simple textures.
    Black, GraphBackground, Header,
    // Button icons.
    Pointer, Refresh, Delete, Plus, 
    // Window icons.
    ManagerEditor, TransformationEditor
}

/// <summary>
/// 
/// </summary>
public class TextureStore {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Singleton -
            /// <summary>Stores the singleton instance of the class.</summary>
            public static readonly TextureStore Instance = new TextureStore();
            
            // - References -
            /// <summary>Static reference to the simple black texture.</summary>
            public static Texture2D Black => TextureStore.GetTexture(texture: ETexture.Black);
            
            /// <summary>Static reference to the graph background texture.</summary>
            public static Texture2D GraphBackground => TextureStore.GetTexture(texture: ETexture.GraphBackground);
            
            /// <summary>Static reference to the simple header texture.</summary>
            public static Texture2D Header => TextureStore.GetTexture(texture: ETexture.Header);
            
            /// <summary>Static reference to the pointer texture.</summary>
            public static Texture2D Pointer => TextureStore.GetTexture(texture: ETexture.Pointer);
            
            /// <summary>Static reference to the refresh texture.</summary>
            public static Texture2D Refresh => TextureStore.GetTexture(texture: ETexture.Refresh);
            
            /// <summary>Static reference to the delete texture.</summary>
            public static Texture2D Delete => TextureStore.GetTexture(texture: ETexture.Delete);
            
            /// <summary>Static reference to the plus texture.</summary>
            public static Texture2D Plus => TextureStore.GetTexture(texture: ETexture.Plus);
            
            /// <summary>Static reference to the manager editor icon texture.</summary>
            public static Texture2D ManagerEditor => TextureStore.GetTexture(texture: ETexture.ManagerEditor);
            
            /// <summary>Static reference to the transformation editor icon texture.</summary>
            public static Texture2D TransformationEditor => TextureStore.GetTexture(texture: ETexture.TransformationEditor);
        // -- Private Attributes --
            /// <summary>Dictionary of all the textures.</summary>
            private readonly Dictionary<ETexture, Texture2D> _mTextures = new Dictionary<ETexture, Texture2D>();
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Loads all the textures.
            /// </summary>
            private TextureStore() { this._LoadTextures(); }
        // -- Public Methods --
            /// <summary>Indexer access to the underlying objects. </summary>
            /// <param name="texture">The index of the requested object.</param>
            public Texture2D this[ETexture texture] => this._mTextures[key: texture];
            
            /// <returns>The requested texture object.</returns>
            public static Texture2D GetTexture(ETexture texture) { return TextureStore.Instance[texture: texture]; }
        // -- Private Methods --
            /// <summary>Load all the textures.</summary>
            private void _LoadTextures() {
                // Load the black texture.
                {
                    Texture2D black = new Texture2D(width: 1, height: 1, textureFormat: TextureFormat.ARGB32, mipChain: false);
                    black.SetPixel(x: 0, y: 0, color: new Color(r: .5f, g: .5f, b: .5f, a: 1f));
                    black.wrapMode = TextureWrapMode.Repeat;
                    black.filterMode = FilterMode.Point;
                    black.Apply();
                    this._mTextures[key: ETexture.Black] = black;
                }
                // Load the header texture.
                {
                    Texture2D texture = new Texture2D(width: 1, height: 1, textureFormat: TextureFormat.ARGB32, mipChain: false);
                    texture.SetPixel(x: 0, y: 0, color: new Color(r: .8f, g: .8f, b: .8f, a: 1f));
                    texture.wrapMode = TextureWrapMode.Repeat;
                    texture.filterMode = FilterMode.Point;
                    texture.Apply();
                    this._mTextures[key: ETexture.Header] = texture;
                }
                // Load the header texture.
                {
                    Texture2D texture = new Texture2D(width: 100, height: 100, textureFormat: TextureFormat.ARGB32, mipChain: false);
                    Color bg = new Color(r: 0.25f, g: 0.25f, b: 0.25f, a: 1f);
                    Color line1 = new Color(r: 0.18f, g: 0.18f, b: 0.18f, a: 1f);
                    Color line2 = new Color(r: 0.10f, g: 0.10f, b: 0.10f, a: 1f);
                    for (int i = 0; i < 100; i++) {
                        for (int j = 0; j < 100; j++) {
                            texture.SetPixel(
                                x: i, y: j,
                                color: i == 0 || j == 0 || i == 1 || j == 1 ?
                                    line2 : 
                                    i % 25 == 0 || j % 25 == 0 ? line1 : bg
                            );
                        }
                    }
                    texture.filterMode = FilterMode.Point;
                    texture.wrapMode = TextureWrapMode.Repeat;
                    texture.wrapModeU = TextureWrapMode.Repeat;
                    texture.wrapModeV = TextureWrapMode.Repeat;
                    texture.Apply();
                    this._mTextures[key: ETexture.GraphBackground] = texture;
                }
                
                // Load the icons.
                const string RESOURCE_BASE = "Editor/Icons/";
                this._mTextures[key: ETexture.Pointer] = Resources.Load<Texture2D>(path: RESOURCE_BASE + "Pointer");
                this._mTextures[key: ETexture.Refresh] = Resources.Load<Texture2D>(path: RESOURCE_BASE + "Refresh");
                this._mTextures[key: ETexture.Delete] = Resources.Load<Texture2D>(path: RESOURCE_BASE + "Delete");
                this._mTextures[key: ETexture.Plus] = Resources.Load<Texture2D>(path: RESOURCE_BASE + "Plus");
                this._mTextures[key: ETexture.ManagerEditor] = Resources.Load<Texture2D>(path: RESOURCE_BASE + "ManagerEditor");
                this._mTextures[key: ETexture.TransformationEditor] = Resources.Load<Texture2D>(path: RESOURCE_BASE + "TransformationEditor");
            }
    // --- /Methods ---
}
}