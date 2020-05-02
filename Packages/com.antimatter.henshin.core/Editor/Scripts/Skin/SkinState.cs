// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Skin {

/// <summary>
/// Class used to store all the skin parameters of the editor classes.
/// This allows the existence of a single asset that stores all the required skin objects.
/// </summary>
public class SkinState: UnityEngine.ScriptableObject {
    // ---  SubObjects ---
        // -- Public Structs --
            /// <summary>
            /// This structure holds all the textures required for the editor.
            /// It also holds simple <see cref="UnityEngine.Color"/>s for all the simple texture elements.
            /// </summary>
            [System.SerializableAttribute]
            public struct TextureStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Images -
                        /// <summary>
                        /// Texture used for the delete buttons of the application.
                        /// </summary>
                        [UnityEngine.HeaderAttribute(header: "Debug Textures", order = 10)]
                        public UnityEngine.Texture2D Delete;
                        
                        /// <summary>
                        /// Texture used for the body of all the nodes.
                        /// </summary>
                        [UnityEngine.HeaderAttribute(header: "Node Textures", order = 1)]
                        public UnityEngine.Texture2D NodeBody;
                        
                        /// <summary>
                        /// Texture used for the header of the actor nodes.
                        /// </summary>
                        public UnityEngine.Texture2D ActorNodeHeader;
                        
                        /// <summary>
                        /// Texture used for the header of the gameplay nodes.
                        /// </summary>
                        public UnityEngine.Texture2D GameplayNodeHeader;
                        
                        /// <summary>
                        /// Texture used for the header of the start nodes.
                        /// </summary>
                        public UnityEngine.Texture2D StartNodeHeader;
                        
                        /// <summary>
                        /// Texture used for the header of the end nodes.
                        /// </summary>
                        public UnityEngine.Texture2D EndNodeHeader;
                        
                        // - Simple Textures -
                        [UnityEngine.HeaderAttribute(header: "Simple Colored Textures", order = 2)]
                        public UnityEngine.Color DebugBoxBackgroundColor;
                    
                    // -- Public Attributes --
                        // - Generated Textures -
                        [System.NonSerializedAttribute]
                        public UnityEngine.Texture2D DebugBoxBackground;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        /// <summary>
                        /// Creates all the texture objects that are not loaded from disk.
                        /// </summary>
                        public void CreateTextures() {
                            // Create the generic background texture.
                            this.DebugBoxBackground = this.LoadTextureFromCache(
                                name: nameof(this.DebugBoxBackground), 
                                color: this.DebugBoxBackgroundColor
                            );
                        }
                    
                    // -- Private Methods --
                        /// <summary>
                        /// Loads the specified texture from the cache.
                        /// </summary>
                        /// <param name="name">The name of the texture to load.</param>
                        /// <param name="color">The expected color of the texture.</param>
                        /// <returns>A reference to the texture object.</returns>
                        private UnityEngine.Texture2D LoadTextureFromCache(string name, UnityEngine.Color color) {
                            // Search for the texture in the project.
                            string[] texturePath = UnityEditor.AssetDatabase.FindAssets(
                                filter: $"{name} t:{nameof(UnityEngine.Texture2D)}",
                                searchInFolders: new []{ $"{SkinState.EditorPath}/UI/Textures/Simple" }
                            );
                            
                            // Check if the texture exists in the database.
                            if (texturePath.Length > 0) {
                                // Load the texture object.
                                UnityEngine.Texture2D diskTexture = UnityEditor.AssetDatabase
                                    .LoadAssetAtPath<UnityEngine.Texture2D>(
                                        assetPath: texturePath[0]
                                );
                                
                                // Check if the texture is valid.
                                if (diskTexture != null && diskTexture.GetPixel(x: 0, y: 0) == color) {
                                    // Return the texture.
                                    return diskTexture;
                                }
                            }
                            
                            // Re-create the texture.
                            UnityEngine.Texture2D texture = this._GenerateSimpleTexture(color: color);
                            
                            // Save it to a file.
                            UnityEditor.AssetDatabase.CreateAsset(
                                asset: texture, 
                                path: $"{SkinState.EditorPath}/UI/Textures/Simple/UI_EDITOR_TEXTURE_{name}.asset"
                            );
                            
                            // Return the texture.
                            return texture;
                        }
                        
                        /// <summary>
                        /// Generates a new simple texture. 
                        /// </summary>
                        /// <param name="color">The color to set in the texture.</param>
                        /// <returns>The newly created texture object.</returns>
                        private UnityEngine.Texture2D _GenerateSimpleTexture(UnityEngine.Color color) {
                            // Create a new texture.
                            UnityEngine.Texture2D texture = new UnityEngine.Texture2D(
                                width: 1,
                                height: 1,
                                textureFormat: UnityEngine.Mathf.Approximately(a: color.a, b: 1f) ? 
                                    UnityEngine.TextureFormat.RGB24 : 
                                    UnityEngine.TextureFormat.ARGB32,
                                mipChain: false
                            );
                            
                            // Set its unique pixel value.
                            texture.SetPixel(x: 0, y: 0, color: color);
                            
                            // Set its wrap and filter modes.
                            texture.wrapMode = UnityEngine.TextureWrapMode.Repeat;
                            texture.filterMode = UnityEngine.FilterMode.Point;
                            
                            // Return the texture.
                            texture.Apply();
                            return texture;
                        }
                // --- /Methods ---
            }
            
            /// <summary>
            /// This structure holds all the <see cref="UnityEngine.GUIContent"/> objects required for the editor.
            /// </summary>
            [System.SerializableAttribute]
            public struct ContentStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Contents -
                        /// <summary>
                        /// Contents displayed in the skin editor's refresh button.
                        /// </summary>
                        [UnityEngine.HeaderAttribute(header: "Skin Editor", order = 9)]
                        public UnityEngine.GUIContent SkinEditorRefresh;
                        
                        /// <summary>
                        /// Content used for the title of the scene editor window.
                        /// </summary>
                        [UnityEngine.HeaderAttribute(header: "Miscelaneous", order = 10)]
                        public UnityEngine.GUIContent SceneEditorTitle;
                // --- /Attributes ---
            }
            
            /// <summary>
            /// This structure holds all the <see cref="UnityEngine.GUIStyle"/> objects required for the editor.
            /// </summary>
            [System.SerializableAttribute]
            public struct StyleStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Styles -
                        /// <summary>
                        /// Style used for the skin editor's refresh button.
                        /// </summary>
                        [UnityEngine.HeaderAttribute(header: "Skin Editor", order = 9)]
                        public UnityEngine.GUIStyle SkinEditorRefresh;
                        
                        /// <summary>
                        /// Style applied to the debug boxes.
                        /// This is used to test the rects of the editor.
                        /// </summary>
                        [UnityEngine.HeaderAttribute(header: "Debug", order = 10)]
                        public UnityEngine.GUIStyle DebugBox;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        /// <summary>
                        /// Applies the generated textures from the <see cref="TextureStruct"/>
                        /// to the corresponding styles.
                        /// </summary>
                        public void ApplyTextures() {
                            this.DebugBox.normal.background = SkinState.Textures.DebugBoxBackground;
                        }
                // --- /Methods ---
            }
            
            /// <summary>
            /// This structure defines all the ratios of the elements of the editor.
            /// </summary>
            [System.SerializableAttribute]
            public struct RatioStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Rects -
                        /// <summary>
                        /// Ratio of the SceneEditor's header relative to the entire window.
                        /// </summary>
                        [UnityEngine.HeaderAttribute(header: "Scene Editor Components", order = 0)]
                        public UnityEngine.Rect SceneEditorHeaderRatio;
                        
                        /// <summary>
                        /// Ratio of the SceneEditor's inspector relative to the entire window.
                        /// </summary>
                        public UnityEngine.Rect SceneEditorInspectorRatio;
                        
                        /// <summary>
                        /// Ratio of the SceneEditor's graph area relative to the entire window.
                        /// </summary>
                        public UnityEngine.Rect SceneEditorGraphAreaRatio;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        // - Helpers -
                        /// <summary>
                        /// Applies the ratio of the rect to the specified object.
                        /// </summary>
                        /// <param name="from">The base rect to compute the ration relative to.</param>
                        /// <param name="ratio">The rect representing the ration to apply.</param>
                        /// <param name="to">The rect in which the ratio will be applied.</param>
                        public static void ApplyRatio(
                            UnityEngine.Rect from, UnityEngine.Rect ratio, ref UnityEngine.Rect to
                        ) {
                            // Apply the changes to the output rect.
                            to.Set(
                                x:      from.x * ratio.x,
                                y:      from.y * ratio.y,
                                width:  from.width  * ratio.width,
                                height: from.height * ratio.height
                            );
                        }
                        
                        // - Initialization -
                        /// <summary>
                        /// Computes the ratio values that are not defined by the user.
                        /// </summary>
                        public void ComputeRatios() {
                            // Compute the width of the header.
                            this.SceneEditorHeaderRatio.x = 0;
                            this.SceneEditorHeaderRatio.y = 0;
                            this.SceneEditorHeaderRatio.width = 1;
                            
                            // Compute the height of the inspector.
                            this.SceneEditorInspectorRatio.x = 0;
                            this.SceneEditorInspectorRatio.y = this.SceneEditorHeaderRatio.height;
                            this.SceneEditorInspectorRatio.height = 1 - this.SceneEditorHeaderRatio.height;
                            
                            // Compute the size of the graph ration.
                            this.SceneEditorGraphAreaRatio.x = this.SceneEditorInspectorRatio.width;
                            this.SceneEditorGraphAreaRatio.width = 1 - this.SceneEditorInspectorRatio.width;
                            this.SceneEditorGraphAreaRatio.y = this.SceneEditorHeaderRatio.height;
                            this.SceneEditorGraphAreaRatio.height = 1 - this.SceneEditorHeaderRatio.height;
                        }
                // --- /Methods ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>
            /// Lists all the editor's texture objects.
            /// </summary>
            public TextureStruct TextureObject;
            
            /// <summary>
            /// Lists all the editor's <see cref="UnityEngine.GUIContent"/> objects.
            /// </summary>
            public ContentStruct ContentObject;
            
            /// <summary>
            /// Lists all the editor's <see cref="UnityEngine.GUIStyle"/> objects.
            /// </summary>
            public StyleStruct StyleObject;
            
            /// <summary>
            /// Lists all the editor's element ratios.
            /// </summary>
            public RatioStruct RatioObject;
        
        // -- Public Attributes --
            /// <summary>
            /// Reference to the instance of the skin found in the 
            /// </summary>
            public static SkinState Instance {
                get {
                    // If the instance is null.
                    if (SkinState._msInstance == null) {
                        // Load it.
                        SkinState._FindSkinAsset();
                    }
                    
                    // Return the instance.
                    return SkinState._msInstance;
                }
            }
            
            // - Accessors -
            /// <summary>
            /// Texture object accessor.
            /// </summary>
            public static TextureStruct Textures => SkinState.Instance.TextureObject;
            
            /// <summary>
            /// Content object accessor.
            /// </summary>
            public static ContentStruct Contents => SkinState.Instance.ContentObject;
            
            /// <summary>
            /// Style object accessor.
            /// </summary>
            public static StyleStruct Styles => SkinState.Instance.StyleObject;
            
            
            /// <summary>
            /// Ratio object accessor.
            /// </summary>
            public static RatioStruct Ratios => SkinState.Instance.RatioObject;
            
            // - Helper Properties -
            /// <summary>
            /// Path to the editor's asmdef folder.
            /// </summary>
            public static string EditorPath;
            
        // -- Private Attributes --
            /// <summary>
            /// Reference to the skin instance found in the project.
            /// </summary>
            private static SkinState _msInstance = null;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Reloads the skin instance.
            /// </summary>
            public static void Reload() {
                SkinState.Instance._Initialize();
            }
        // -- Private Methods --
            /// <summary>
            /// Seeks the skin asset in the project.
            /// Creates it if it is not found.
            /// </summary>
            private static void _FindSkinAsset() {
                // Find the location of the editor assembly asset.
                string[] editorAsset = UnityEditor.AssetDatabase.FindAssets(
                    filter: $"com.antimatter.henshin.core.editor t:{nameof(UnityEditorInternal.AssemblyDefinitionAsset)}"
                );
                
                // Check if the editor asset was found.
                if (editorAsset.Length > 0) {
                    // Get the path of the asset.
                    SkinState.EditorPath = System.IO.Path.GetDirectoryName(
                        path: UnityEditor.AssetDatabase.GUIDToAssetPath(guid: editorAsset[0])
                    );
                    
                    // Search for the skin asset in the editor path's folder.
                    string[] skinAsset = UnityEditor.AssetDatabase.FindAssets(
                        filter: "DATA_EDITOR_Skin",
                        searchInFolders: new [] { $"{SkinState.EditorPath}/UI/Data" }
                    );
                    
                    // Check if the skin object was found.
                    SkinState skin;
                    if (skinAsset.Length > 0) {
                        // Load the asset.
                        skin = UnityEditor.AssetDatabase.LoadAssetAtPath<SkinState>(
                            assetPath: UnityEditor.AssetDatabase.GUIDToAssetPath(guid: skinAsset[0])
                        );
                    } else {
                        UnityEngine.Debug.LogWarning(message: "Creating a new editor skin instance.");
                        // Instantiate a new skin.
                        skin = UnityEngine.ScriptableObject.CreateInstance<SkinState>();
                        skin.name = "DATA_EDITOR_Skin";
                        
                        // Create a new asset.
                        UnityEditor.AssetDatabase.CreateAsset(
                            asset: skin,
                            path: $"{SkinState.EditorPath}/UI/Data/{skin.name}.asset"
                        );
                        
                        // Reserialize all the assets.
                        UnityEditor.AssetDatabase.ForceReserializeAssets();
                    }
                    
                    // Store the skin instance.
                    SkinState._msInstance = skin;
                        
                    // Initialize the skin.
                    skin._Initialize();
                } else {
                    throw new System.InvalidOperationException(message: "Could not find the editor's assembly definition.");
                }
            }
            
            /// <summary>
            /// Event triggered when the object gets instanced. 
            /// </summary>
            private void _Initialize() {
                // Create all the required texture objects.
                this.TextureObject.CreateTextures();
                // Load all the gui style objects.
                this.StyleObject.ApplyTextures();
                // Reload the ratio's sizes.
                this.RatioObject.ComputeRatios();
            }
    // --- /Methods ---
}
}