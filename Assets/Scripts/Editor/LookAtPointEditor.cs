using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace SpaceshipRunGB.Lesson10
{
    [CustomEditor(typeof(LookAt))]
    [CanEditMultipleObjects]
    public class LookAtPointEditor : Editor
	{
        SerializedProperty lookAtPoint;
        void OnEnable()
        {
            lookAtPoint = serializedObject.FindProperty("LookAtPoint");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(lookAtPoint);
            if (lookAtPoint.vector3Value.y > (target as
            LookAt).transform.position.y)
            {
                EditorGUILayout.LabelField("(Above this object)");
            }
            if (lookAtPoint.vector3Value.y < (target as
            LookAt).transform.position.y)
            {
                EditorGUILayout.LabelField("(Below this object)");
            }
            serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI()
        {
            var t = (target as LookAt);
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Handles.PositionHandle(t.LookAtPoint,
            Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move point");
                t.LookAtPoint = pos;
                t.Update();
            }
        }

    }
}