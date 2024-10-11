////////////////////////////////////////
/// LSKy
/// ==================
///
/// Description:
/// =============
/// Custom inspector for skydome.
////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using Rallec.Utility;


namespace Rallec.LSky
{

	public partial class LSky_SkydomeEditor : LSky_CommonEditor
	{



		#region |Properties|

		// Renderer
		//-------------------------------------
		SerializedProperty m_RenderClouds;
		SerializedProperty m_CloudsLayerIndex;
		//=====================================

		// Scale
		//----------------------------------------
		SerializedProperty m_CloudsMeshHeightSize;
		//========================================

		// Rotation
		//----------------------------------------
		SerializedProperty m_CloudsRotationSpeed;
		//========================================

		// Type
		//----------------------------------------
		SerializedProperty m_NoiseBasedClouds;
		//========================================

		// Color
		//---------------------------------------

		SerializedProperty m_CloudsColor, m_CloudsMoonColor;
		SerializedProperty m_CloudsEdgeColor;
        SerializedProperty m_CloudsTint;
		SerializedProperty m_CloudsEdgeColorHeight;
		//=======================================

		// Shape.
		//---------------------------------------
        private SerializedProperty m_CloudsNoiseScale;
		private SerializedProperty m_CloudsNoisePower;
        private SerializedProperty m_CloudsClipping;
        private SerializedProperty m_CloudsSmoothness;
        private SerializedProperty m_CloudsDistortionScale;
        private SerializedProperty m_CloudsDistortion;
        //=====================================

        // Texture
        //---------------------------------------
        SerializedProperty m_CloudsTexture;
        SerializedProperty m_CloudsSize;
        SerializedProperty m_CloudsTexOffset;

        // Shape.
        SerializedProperty m_CloudsDensity;
        SerializedProperty m_CloudsCoverage;
        //==============================================================================


        #endregion


        #region |Foldouts|
        bool m_CloudsFoldout;

		#endregion
		

		protected void InitClouds()
		{

			
			// Renderer
			//------------------------------------------------------------------------
			m_RenderClouds = serObj.FindProperty("m_RenderClouds");
			m_CloudsLayerIndex = serObj.FindProperty("m_CloudsLayerIndex");
			//========================================================================

			// Scale
			//------------------------------------------------------------------------
			m_CloudsMeshHeightSize = serObj.FindProperty("m_CloudsMeshHeightSize");
			//========================================================================

			// Rotation
			//------------------------------------------------------------------------
			m_CloudsRotationSpeed = serObj.FindProperty("m_CloudsRotationSpeed");
			//========================================================================

			// Type
			//------------------------------------------------------------------------
			m_NoiseBasedClouds = serObj.FindProperty("m_NoiseBasedClouds");
			//========================================================================

			// Color
			//-----------------------------------------------------------------------
			m_CloudsColor     = serObj.FindProperty("m_CloudsColor");
			m_CloudsMoonColor = serObj.FindProperty("m_CloudsMoonColor");
			m_CloudsEdgeColor = serObj.FindProperty("m_CloudsEdgeColor");
			m_CloudsEdgeColorHeight = serObj.FindProperty("m_CloudsEdgeColorHeight");
            m_CloudsTint = serObj.FindProperty("m_CloudTint");
			//=======================================================================

			// Shape.
			//-----------------------------------------------------------------------
			m_CloudsNoiseScale       = serObj.FindProperty("m_CloudsNoiseScale");
            m_CloudsNoisePower       = serObj.FindProperty("m_CloudsNoisePower");
            m_CloudsClipping         = serObj.FindProperty("m_CloudsClipping");
            m_CloudsSmoothness       = serObj.FindProperty("m_CloudsSmoothness");
            m_CloudsDistortionScale  = serObj.FindProperty("m_CloudsDistortionScale");
            m_CloudsDistortion       = serObj.FindProperty("m_CloudsDistortion");
            //=======================================================================

            // Texture
            //-----------------------------------------------------------------------
            m_CloudsTexture = serObj.FindProperty("m_CloudsTexture");
            m_CloudsSize = serObj.FindProperty("m_CloudsSize");
            m_CloudsTexOffset = serObj.FindProperty("m_CloudsTexOffset");
            //=======================================================================

            // Shape.
            //-----------------------------------------------------------------------
            m_CloudsDensity = serObj.FindProperty("m_CloudsDensity");
            m_CloudsCoverage = serObj.FindProperty("m_CloudsCoverage");
        }


        protected virtual void Clouds()
		{

			R_EditorGUIUtility.ShurikenFoldoutHeader("Clouds", TextTitleStyle, ref m_CloudsFoldout);
            if(m_CloudsFoldout)
            {
				R_EditorGUIUtility.ShurikenHeader("Clouds", TextSectionStyle, 20);
                EditorGUILayout.Separator();

				EditorGUILayout.PropertyField(m_RenderClouds, new GUIContent("Render Clouds"));
				R_EditorGUIUtility.Separator(2);

 				EditorGUILayout.PropertyField(m_CloudsLayerIndex, new GUIContent("Clouds Layer Index"));
				R_EditorGUIUtility.Separator(2);

				EditorGUILayout.PropertyField(m_CloudsMeshHeightSize, new GUIContent("Clouds Mesh Height Size"));
				R_EditorGUIUtility.Separator(2);

				EditorGUILayout.PropertyField(m_CloudsRotationSpeed, new GUIContent("Clouds Rotation Speed"));
				R_EditorGUIUtility.Separator(2);

				// Color
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.PropertyField(m_CloudsColor, new GUIContent("Clouds Color"));
				EditorGUILayout.HelpBox("Evaluate Gradient time by full sun cicle", MessageType.Info);
				EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.PropertyField(m_CloudsEdgeColor, new GUIContent("Clouds Edge Color"));
                EditorGUILayout.HelpBox("Evaluate Gradient time by full sun cicle", MessageType.Info);
                EditorGUILayout.EndVertical();

                EditorGUILayout.PropertyField(m_CloudsTint, new GUIContent("Clouds Tint"));

                EditorGUILayout.PropertyField(m_CloudsEdgeColorHeight, new GUIContent("Clouds Edge Color Height"));
                R_EditorGUIUtility.Separator(2);

                if (m_NightAtmosphereMode.intValue == 1)
				{
					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.PropertyField(m_CloudsMoonColor, new GUIContent("Clouds Moon Color"));
					EditorGUILayout.HelpBox("Evaluate Gradient time by full moon cicle", MessageType.Info);
					EditorGUILayout.EndVertical();
				}

                // Type
                EditorGUILayout.PropertyField(m_NoiseBasedClouds, new GUIContent("Noise Based Clouds"));
                R_EditorGUIUtility.Separator(2);

                if (m_NoiseBasedClouds.boolValue)
                {
                    R_EditorGUIUtility.ShurikenHeader("Noise Based Clouds Settings", TextSectionStyle, 20);
                    EditorGUILayout.Separator();

                    // Shape
                    EditorGUILayout.PropertyField(m_CloudsNoiseScale, new GUIContent("Clouds Size"));
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_CloudsNoisePower, new GUIContent("Clouds Density"));
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_CloudsClipping, new GUIContent("Clouds Clipping"));
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_CloudsSmoothness, new GUIContent("Clouds Smoothness"));
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_CloudsDistortionScale, new GUIContent("Clouds Distortion Size"));
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_CloudsDistortion, new GUIContent("Clouds Distortion"));
                    R_EditorGUIUtility.Separator(2);
                }
                else
                {
                    R_EditorGUIUtility.ShurikenHeader("Texture Based Clouds Settings", TextSectionStyle, 20);
                    EditorGUILayout.Separator();

                    // Texture
                    m_CloudsTexture.objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField("Clouds Texture",
                        m_CloudsTexture.objectReferenceValue, typeof(Texture2D), true);
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_CloudsSize, new GUIContent("Clouds Size"));
                    EditorGUILayout.PropertyField(m_CloudsTexOffset, new GUIContent("Clouds Tex Offset"));
                    R_EditorGUIUtility.Separator(2);

                    // Shape
                    EditorGUILayout.PropertyField(m_CloudsDensity, new GUIContent("Clouds Density"));
                    R_EditorGUIUtility.Separator(2);

                    EditorGUILayout.PropertyField(m_CloudsCoverage, new GUIContent("Clouds Coverage"));
                    R_EditorGUIUtility.Separator(2);
                }

                EditorGUILayout.Separator();
				//=============================================================================================================================================
			}


		}

	}

}