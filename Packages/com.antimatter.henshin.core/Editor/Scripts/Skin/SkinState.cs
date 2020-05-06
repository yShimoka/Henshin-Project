// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using System;
using Henshin.Editor.Misc;
using UnityEngine;

namespace Henshin.Editor.Skin {

/// <summary>
/// Class used to store all the skin parameters of the editor classes.
/// This allows the existence of a single asset that stores all the required skin objects.
/// </summary>
public class SkinState: ScriptableObject {
    // ---  SubObjects ---
        // -- Public Structs --
            /// <summary>
            /// This structure holds all the textures required for the editor.
            /// It also holds simple <see cref="UnityEngine.Color"/>s for all the simple texture elements.
            /// </summary>
            [Serializable]
            public struct TextureStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Images -
                        /// <summary>
                        /// Texture used for the delete buttons of the application.
                        /// </summary>
                        [Header(header: "Debug Textures", order = 10)]
                        public Texture2D Delete;
                        
                        /// <summary>
                        /// Texture used for the body of all the nodes.
                        /// </summary>
                        [Header(header: "Node Textures", order = 1)]
                        public Texture2D NodeBody;
                        
                        /// <summary>
                        /// Texture used for the sockets that are filled.
                        /// </summary>
                        public Texture2D FilledSocket;
                        
                        /// <summary>
                        /// Texture used for the sockets that are empty.
                        /// </summary>
                        public Texture2D EmptySocket;
                        
                        /// <summary>
                        /// Texture used for the header of the scene nodes.
                        /// </summary>
                        public Texture2D SceneNodeHeader;
                        
                        /// <summary>
                        /// Texture used for the header of the actor nodes.
                        /// </summary>
                        public Texture2D ActorNodeHeader;
                        
                        /// <summary>
                        /// Texture used for the header of the gameplay nodes.
                        /// </summary>
                        public Texture2D GameplayNodeHeader;
                        
                        /// <summary>
                        /// Texture used for the header of the start nodes.
                        /// </summary>
                        public Texture2D StartNodeHeader;
                        
                        /// <summary>
                        /// Texture used for the header of the end nodes.
                        /// </summary>
                        public Texture2D EndNodeHeader;
                        
                        /// <summary>
                        /// Color used behind the contents of the scene header.
                        /// </summary>
                        [Header(header: "Scene Header Textures", order = 0)] 
                        [Tooltip(tooltip: "Color behind the header's background.")]
                        public Color SceneHeaderColor;
                        
                        /// <summary>
                        /// Color used for the scene header's separator.
                        /// </summary>
                        [Tooltip(tooltip: "Color of the header's separator.")]
                        public Color SceneHeaderSeparatorColor;
                        
                        /// <summary>
                        /// Color used for the background of the scene header.
                        /// </summary>
                        [Tooltip(tooltip: "Color of the header's background.")]
                        public Color SceneHeaderBackgroundColor;
                        
                        /// <summary>
                        /// Color used for the border of the scene header.
                        /// </summary>
                        [Tooltip(tooltip: "Color of the header's background border.")]
                        public Color SceneHeaderBackgroundBorderColor;
                        
                        /// <summary>
                        /// Size of the scene header's border.
                        /// </summary>
                        [Tooltip(tooltip: "Size of the header's background border.")]
                        public int SceneHeaderBackgroundBorderSize;
                        
                        /// <summary>
                        /// Texture used as background for the inspector's titles.
                        /// </summary>
                        [Header(header: "Scene Inspector Textures", order = 1)]
                        [Tooltip(tooltip: "Color used for the background of the inspector header.")]
                        public Color SceneInspectorTitleBackgroundColor;
                        
                        /// <summary>
                        /// Texture used as background for the inspector's titles.
                        /// </summary>
                        [Tooltip(tooltip: "Color used for the background of the inspector's subtitle.")]
                        public Color SceneInspectorSubtitleBackgroundColor;
                        
                        /// <summary>
                        /// Size of the scene header's border radius.
                        /// </summary>
                        [Header(header: "Scene Graph Textures", order = 2)]
                        [Tooltip(tooltip: "Radius of the header's background corners.")]
                        public int SceneHeaderBackgroundBorderRadius;
                        
                        /// <summary>
                        /// The color of the background of the GraphArea.
                        /// </summary>
                        [Tooltip(tooltip: "Color of the background in the graph view.")]
                        public Color SceneGraphBackgroundColor;
                        
                        /// <summary>
                        /// The color of the separators of the GraphArea.
                        /// </summary>
                        [Tooltip(tooltip: "Color of the separators in the graph view.")]
                        public Color SceneGraphSeparatorColor;
                        
                        /// <summary>
                        /// The color of the scrollbars of the GraphArea.
                        /// </summary>
                        [Tooltip(tooltip: "Color of the scene graph view's scrollbar.")]
                        public Color SceneGraphScrollbarColor;
                        
                        /// <summary>
                        /// The color of the scrollbars background of the GraphArea.
                        /// </summary>
                        [Tooltip(tooltip: "Color of the scene graph view scrollbars background.")]
                        public Color SceneGraphScrollbarBackgroundColor;
                        
                        // - Simple Textures -
                        [Header(header: "Simple Colored Textures", order = 9)]
                        public Color DebugBoxBackgroundColor;
                    
                    // -- Public Attributes --
                        // - Generated Textures -
                        /// <summary>
                        /// Simple texture used for the background of the debug boxes.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D DebugBoxBackground;
                        
                        /// <summary>
                        /// Texture used behind the contents of the scene header.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D SceneHeaderContents;
                        
                        /// <summary>
                        /// Texture used for the background of the scene header.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D SceneHeaderBackground;
                        
                        /// <summary>
                        /// Texture used for the header separator.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D SceneHeaderSeparator;
                        
                        /// <summary>
                        /// Texture used for the inspector titles.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D SceneInspectorTitleBackground;
                        
                        /// <summary>
                        /// Texture used for the inspector sub titles.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D SceneInspectorSubtitleBackground;
                        
                        /// <summary>
                        /// Texture used for the graph view's scrollbars.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D SceneGraphScrollbar;
                        
                        /// <summary>
                        /// Texture used for the graph view scrollbar's background.
                        /// </summary>
                        [NonSerialized]
                        public Texture2D SceneGraphScrollbarBackground;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        /// <summary>
                        /// Creates all the texture objects that are not loaded from disk.
                        /// </summary>
                        public void CreateTextures() {
                            // Create the scene content background.
                            this.SceneHeaderContents = TextureGenerator.CreateSimple(
                                name: nameof(this.SceneHeaderContents),
                                colour: this.SceneHeaderColor
                            );
                            // Create the scene header background.
                            this.SceneHeaderBackground = TextureGenerator.CreateBordered(
                                name: nameof(this.SceneHeaderBackground),
                                innerColour: this.SceneHeaderBackgroundColor,
                                borderSize: this.SceneHeaderBackgroundBorderSize,
                                borderColour: this.SceneHeaderBackgroundBorderColor,
                                borderRadius: this.SceneHeaderBackgroundBorderRadius
                            );
                            // Create the scene header separator.
                            this.SceneHeaderSeparator = TextureGenerator.CreateSimple(
                                name: nameof(this.SceneHeaderSeparator),
                                colour: this.SceneHeaderSeparatorColor
                            );
                            
                            this.SceneInspectorTitleBackground = TextureGenerator.CreateSimple(
                                name: nameof(this.SceneInspectorTitleBackground),
                                colour: this.SceneInspectorTitleBackgroundColor
                            );
                            this.SceneInspectorSubtitleBackground = TextureGenerator.CreateSimple(
                                name: nameof(this.SceneInspectorSubtitleBackground),
                                colour: this.SceneInspectorSubtitleBackgroundColor
                            );
                            
                            // Create the scene graph view scrollbar
                            this.SceneGraphScrollbar = TextureGenerator.CreateBordered(
                                name: nameof(this.SceneGraphScrollbar),
                                innerColour: SkinState.Textures.SceneGraphScrollbarColor,
                                borderSize: 0,
                                borderColour: Color.black,
                                borderRadius: SkinState.Styles.SceneGraphScrollbar.border.left
                            );
                            // Create the scene graph view scrollbar background/
                            this.SceneGraphScrollbarBackground = TextureGenerator.CreateSimple(
                                name: nameof(this.SceneGraphScrollbarBackground),
                                colour: SkinState.Textures.SceneGraphScrollbarBackgroundColor
                            );
                            
                            // Create the generic background texture.
                            this.DebugBoxBackground = TextureGenerator.CreateBordered(
                                name: nameof(this.DebugBoxBackground), 
                                innerColour: this.DebugBoxBackgroundColor,
                                borderSize: 2,
                                borderColour: Color.green
                            );
                        }
                // --- /Methods ---
            }
            
            /// <summary>
            /// This structure holds all the <see cref="UnityEngine.GUIContent"/> objects required for the editor.
            /// </summary>
            [Serializable]
            public struct ContentStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Contents -
                        /// <summary>
                        /// Contents displayed in the scene editor's save button.
                        /// </summary>
                        [Header(header: "Scene Editor", order = 0)]
                        public GUIContent SceneEditorSave;
                        
                        /// <summary>
                        /// Contents displayed in the scene editor's play button.
                        /// </summary>
                        public GUIContent SceneEditorPlay;
                        
                        /// <summary>
                        /// Contents displayed in the scene editor's center button.
                        /// </summary>
                        public GUIContent SceneEditorCenter;
                        
                        /// <summary>
                        /// Contents displayed in the scene inspector's scene section title.
                        /// </summary>
                        public GUIContent SceneInspectorSceneSectionTitle;
                        
                        /// <summary>
                        /// Contents displayed in the scene inspector's action section title.
                        /// </summary>
                        public GUIContent SceneInspectorActionSectionTitle;
                        
                        /// <summary>
                        /// Contents displayed in the skin editor's refresh button.
                        /// </summary>
                        [Header(header: "Skin Editor", order = 9)]
                        public GUIContent SkinEditorRefresh;
                        
                        /// <summary>
                        /// Content used for the title of the scene editor window.
                        /// </summary>
                        [Header(header: "Miscelaneous", order = 10)]
                        public GUIContent SceneEditorTitle;
                // --- /Attributes ---
            }
            
            /// <summary>
            /// This structure holds all the <see cref="UnityEngine.GUIStyle"/> objects required for the editor.
            /// </summary>
            [Serializable]
            public struct StyleStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Styles -
                        /// <summary>
                        /// Style applied to the header of the scene editor.
                        /// </summary>
                        [Header(header: "Scene Editor", order = 0)]
                        public GUIStyle SceneHeaderContents;
                        
                        /// <summary>
                        /// Style applied to the scene editor header's background
                        /// </summary>
                        public GUIStyle SceneHeaderBackground;
                        
                        /// <summary>
                        /// Style applied to the scene header's separator.
                        /// </summary>
                        public GUIStyle SceneHeaderSeparator;
                        
                        /// <summary>
                        /// Style applied to the scene editor header's buttons.
                        /// </summary>
                        public GUIStyle SceneHeaderButton;
                        
                        /// <summary>
                        /// Style applied to the titles of the scene inspector.
                        /// </summary>
                        [Header(header: "Scene Inspector", order = 1)]
                        public GUIStyle SceneInspectorTitle;
                        
                        /// <summary>
                        /// Style applied to the subtitles of the scene inspector.
                        /// </summary>
                        public GUIStyle SceneInspectorSubtitle;
                        
                        /// <summary>
                        /// Style applied to the scrollbars of the graph area.
                        /// </summary>
                        [Header(header: "Scene Graph", order = 2)]
                        public GUIStyle SceneGraphScrollbar;
                        
                        /// <summary>
                        /// Style applied to the background of the scrollbars of the graph area.
                        /// </summary>
                        public GUIStyle SceneGraphScrollbarBackground;
                        
                        /// <summary>
                        /// Style applied to the node's header.
                        /// </summary>
                        [Header(header: "Graph Nodes", order = 1)]
                        public GUIStyle NodeHeader;
                        
                        /// <summary>
                        /// Style applied to the node's header text.
                        /// </summary>
                        public GUIStyle NodeHeaderText;
                        
                        /// <summary>
                        /// Style applied to the node's body.
                        /// </summary>
                        public GUIStyle NodeBody;
                        
                        /// <summary>
                        /// Style applied to the node's filled sockets.
                        /// </summary>
                        public GUIStyle FilledSocket;
                        
                        /// <summary>
                        /// Style applied to the node's empty sockets.
                        /// </summary>
                        public GUIStyle EmptySocket;
                        
                        /// <summary>
                        /// Style applied to the node's actor header.
                        /// </summary>
                        [NonSerialized]
                        public GUIStyle ActorNodeHeader;
                        
                        /// <summary>
                        /// Style applied to the node's gameplay header.
                        /// </summary>
                        [NonSerialized]
                        public GUIStyle GameplayNodeHeader;
                        
                        /// <summary>
                        /// Style applied to the node's scene header.
                        /// </summary>
                        [NonSerialized]
                        public GUIStyle SceneNodeHeader;
                        
                        /// <summary>
                        /// Style applied to the node's start header.
                        /// </summary>
                        [NonSerialized]
                        public GUIStyle StartNodeHeader;
                        
                        /// <summary>
                        /// Style applied to the node's end header.
                        /// </summary>
                        [NonSerialized]
                        public GUIStyle EndNodeHeader;
                        
                        /// <summary>
                        /// Style used for the skin editor's refresh button.
                        /// </summary>
                        [Header(header: "Skin Editor", order = 9)]
                        public GUIStyle SkinEditorRefresh;
                        
                        /// <summary>
                        /// Style applied to the debug boxes.
                        /// This is used to test the rects of the editor.
                        /// </summary>
                        [Header(header: "Debug", order = 10)]
                        public GUIStyle DebugBox;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Public Methods --
                        /// <summary>
                        /// Applies the generated textures from the <see cref="TextureStruct"/>
                        /// to the corresponding styles.
                        /// </summary>
                        public void ApplyTextures() {
                            // Apply the scene header's texture and radius.
                            {
                                int b = SkinState.Textures.SceneHeaderBackgroundBorderRadius + SkinState.Textures.SceneHeaderBackgroundBorderSize;
                                this.SceneHeaderBackground.border = new RectOffset{ left = b, top = b, right = b, bottom = b};
                                this.SceneHeaderBackground.normal.background = SkinState.Textures.SceneHeaderBackground;
                                this.SceneHeaderContents.normal.background = SkinState.Textures.SceneHeaderContents;
                                this.SceneHeaderSeparator.normal.background = SkinState.Textures.SceneHeaderSeparator;
                                this.SceneGraphScrollbar.normal.background = SkinState.Textures.SceneGraphScrollbar;
                                this.SceneGraphScrollbarBackground.normal.background = SkinState.Textures.SceneGraphScrollbarBackground;
                            }

                            // Apply the styling of the inspector.
                            {
                                this.SceneInspectorTitle.normal.background = SkinState.Textures.SceneInspectorTitleBackground;
                                this.SceneInspectorSubtitle.normal.background = SkinState.Textures.SceneInspectorSubtitleBackground;
                            }
                            
                            // Apply the styling to the nodes.
                            {
                                this.GameplayNodeHeader = new GUIStyle(other: this.NodeHeader) {
                                    normal = { background = SkinState.Textures.GameplayNodeHeader }
                                };
                                this.SceneNodeHeader = new GUIStyle(other: this.NodeHeader) {
                                    normal = { background = SkinState.Textures.SceneNodeHeader }
                                };
                                this.ActorNodeHeader = new GUIStyle(other: this.NodeHeader) {
                                    normal = { background = SkinState.Textures.ActorNodeHeader }
                                };
                                this.StartNodeHeader = new GUIStyle(other: this.NodeHeader) {
                                    normal = { background = SkinState.Textures.StartNodeHeader }
                                };
                                this.EndNodeHeader = new GUIStyle(other: this.NodeHeader) {
                                    normal = { background = SkinState.Textures.EndNodeHeader }
                                };
                                this.NodeBody.normal.background = SkinState.Textures.NodeBody;
                                this.FilledSocket.normal.background = SkinState.Textures.FilledSocket;
                                this.EmptySocket.normal.background = SkinState.Textures.EmptySocket;
                            }
                            
                            // Apply the debug box's background image.
                            this.DebugBox.normal.background = SkinState.Textures.DebugBoxBackground;
                        }
                // --- /Methods ---
            }
            
            /// <summary>
            /// This structure defines all the ratios of the elements of the editor.
            /// </summary>
            [Serializable]
            public struct RatioStruct {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        // - Rects -
                        /// <summary>
                        /// Ratio of the SceneEditor's header relative to the entire window.
                        /// </summary>
                        [Header(header: "Scene Editor Components", order = 0)]
                        public Rect SceneEditorHeaderRatio;
                        
                        /// <summary>
                        /// Ratio of the SceneEditor's inspector relative to the entire window.
                        /// </summary>
                        public Rect SceneEditorInspectorRatio;
                        
                        /// <summary>
                        /// Ratio of the SceneEditor's graph area relative to the entire window.
                        /// </summary>
                        public Rect SceneEditorGraphAreaRatio;
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
                            Rect from, Rect ratio, ref Rect to
                        ) {
                            // Apply the changes to the output rect.
                            to.Set(
                                x:      from.width * ratio.x,
                                y:      from.height * ratio.y,
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
                        Debug.LogWarning(message: "Creating a new editor skin instance.");
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
                    try {
                        skin._Initialize();
                    } catch (NullReferenceException) {
                        // Reserialize all the assets.
                        UnityEditor.AssetDatabase.ForceReserializeAssets();
                        
                        // Reinitialize the skin.
                        skin._Initialize();
                    }
                } else {
                    throw new InvalidOperationException(message: "Could not find the editor's assembly definition.");
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