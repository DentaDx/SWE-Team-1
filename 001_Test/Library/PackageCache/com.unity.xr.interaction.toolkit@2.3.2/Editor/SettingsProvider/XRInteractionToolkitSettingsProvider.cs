﻿using System.Linq;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Editor;
using UnityEditor.XR.Interaction.Toolkit.ProjectValidation;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

namespace UnityEditor.XR.Interaction.Toolkit
{
    /// <summary>
    /// Class configuration to display XR Interaction Toolkit settings in the Project Settings window.
    /// </summary>
    class XRInteractionToolkitSettingsProvider : SettingsProvider
    {
        /// <summary>
        /// Scope that adds a top and a left margin.
        /// </summary>
        class SettingsMarginScope : GUI.Scope
        {
            internal SettingsMarginScope()
            {
                const float topMargin = 10f;
                const float leftMargin = 10f;
                
                GUILayout.BeginHorizontal();
                GUILayout.Space(leftMargin);
                GUILayout.BeginVertical();
                GUILayout.Space(topMargin);
            }

            protected override void CloseScope()
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Contents of GUI elements used by this settings provider.
        /// </summary>
        protected static class Contents
        {
            public static readonly GUIContent projectValidation = EditorGUIUtility.TrTextContentWithIcon("XR Interaction Toolkit project validation issues", ProjectValidationContents.validationWarningIcon.image);
            public static readonly GUIContent layerMaskUpdater = EditorGUIUtility.TrTextContent("Run Interaction Layer Mask Updater", "Open the dialog box to update the Interaction Layer Mask for projects upgrading from older versions of the XR Interaction Toolkit.");

            public static readonly GUIContent deviceSimulatorSettingsTitle = new GUIContent("XR Device Simulator Settings");
            public static readonly GUIContent interactionLayerSettingsTitle = new GUIContent("Interaction Layer Settings");

            public static readonly GUIStyle sectionTitleStyle = new GUIStyle("Label")
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
            };
        }

        /// <summary>
        /// The path to display this settings provider in the Project Settings window.
        /// </summary>
        const string k_Path = "Project/XR Interaction Toolkit";
        
        Editor m_InteractionLayerSettingsEditor;
        Editor m_XRInteractionEditorSettingsEditor;
        Editor m_XRDeviceSimulatorSettingsEditor;

        static readonly List<BuildValidationRule> s_ValidationRules = new List<BuildValidationRule>();
        
        /// <summary>
        /// Create and returns this settings provider.
        /// </summary>
        /// <returns>Returns a new instance of this settings provider.</returns>
        [SettingsProvider]
#pragma warning disable IDE0051 // Remove unused private members -- Invoked by Unity due to attribute
        static SettingsProvider CreateInteractionLayerProvider()
#pragma warning restore IDE0051
        {
            var keywordsList = GetSearchKeywordsFromPath(AssetDatabase.GetAssetPath(InteractionLayerSettings.instance)).ToList();
            return new XRInteractionToolkitSettingsProvider { keywords = keywordsList };
        }

        XRInteractionToolkitSettingsProvider(string path = k_Path, SettingsScope scopes = SettingsScope.Project,
            IEnumerable<string> keywords = null)
            : base(path, scopes, keywords)
        {
        }
        
        /// <summary>
        /// Draw the interaction layer updater button.
        /// </summary>
        static void DrawInteractionLayerUpdateButton()
        {
            var oldGuiEnabled = GUI.enabled;
            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;
            
            if (GUILayout.Button(Contents.layerMaskUpdater, GUILayout.ExpandWidth(false)))
            {
                InteractionLayerUpdater.RunIfUserWantsTo();
                GUIUtility.ExitGUI();
            }

            GUI.enabled = oldGuiEnabled;
        }
        
        /// <summary>
        /// Draw the the project validation warning if there are any validation issues.
        /// </summary>
        static void DrawXRInteractionValidation()
        {
            XRInteractionProjectValidation.GetCurrentValidationIssues(s_ValidationRules);
            if (s_ValidationRules.Count == 0)
                return;

            var anyErrors = s_ValidationRules.Any(rule => rule.Error);
            var issueContent = anyErrors ? ProjectValidationContents.validationErrorIcon: ProjectValidationContents.validationWarningIcon;
            Contents.projectValidation.image = issueContent.image;
            Contents.projectValidation.tooltip = issueContent.tooltip;
            
            if (GUILayout.Button(Contents.projectValidation, EditorStyles.label, GUILayout.Height(EditorGUIUtility.singleLineHeight))) 
            {
                XRInteractionProjectValidationRulesSetup.ShowWindow();
            }
        }

        /// <summary>
        /// Draws the XR Interaction Settings editor.
        /// </summary>
        void DrawXRInteractionEditorSettingsEditor()
        {
            if (m_XRInteractionEditorSettingsEditor != null)
            {
                GUILayout.Label(Contents.interactionLayerSettingsTitle, Contents.sectionTitleStyle);
                m_XRInteractionEditorSettingsEditor.OnInspectorGUI();
            }
        }
        
        /// <summary>
        /// Draws the Interaction Layer Settings editor.
        /// </summary>
        void DrawInteractionLayerSettingsEditor()
        {
            if (m_InteractionLayerSettingsEditor != null)
                m_InteractionLayerSettingsEditor.OnInspectorGUI();
        }

        /// <summary>
        /// Draws the Interaction Layer Settings editor.
        /// </summary>
        void DrawXRDeviceSimulatorSettingsEditor()
        {
            if (m_InteractionLayerSettingsEditor != null)
            {
                GUILayout.Label(Contents.deviceSimulatorSettingsTitle, Contents.sectionTitleStyle);
                m_XRDeviceSimulatorSettingsEditor.OnInspectorGUI();
            }
        }

        /// <inheritdoc />
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            
            m_InteractionLayerSettingsEditor = Editor.CreateEditor(InteractionLayerSettings.instance);
            m_XRInteractionEditorSettingsEditor = Editor.CreateEditor(XRInteractionEditorSettings.instance);
            m_XRDeviceSimulatorSettingsEditor = Editor.CreateEditor(XRDeviceSimulatorSettings.instance);
        }

        /// <inheritdoc />
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            
            if (m_InteractionLayerSettingsEditor != null)
                Object.DestroyImmediate(m_InteractionLayerSettingsEditor);
            
            if (m_XRInteractionEditorSettingsEditor != null)
                Object.DestroyImmediate(m_XRInteractionEditorSettingsEditor);

            if (m_XRDeviceSimulatorSettingsEditor != null)
                Object.DestroyImmediate(m_XRDeviceSimulatorSettingsEditor);
        }

        /// <inheritdoc />
        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            using (new SettingsMarginScope())
            {
                DrawXRInteractionValidation();
                DrawXRDeviceSimulatorSettingsEditor();
                EditorGUILayout.Space();
                DrawXRInteractionEditorSettingsEditor();
                DrawInteractionLayerUpdateButton();
                EditorGUILayout.Space();
                DrawInteractionLayerSettingsEditor();
            }
        }
    }
}
