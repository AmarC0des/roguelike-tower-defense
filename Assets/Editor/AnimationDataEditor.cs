using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(AnimationData))]
[CanEditMultipleObjects]

public class AnimationDataEditor : Editor
{
    string subFrameSetAll = "0";
    int lengthAll;
    AnimationData animData;

    SerializedObject frames;
    SerializedProperty sideFrameProperty;
    SerializedProperty upFrameProperty;
    SerializedProperty downFrameProperty;

    private void OnEnable()
    {
        animData = target as AnimationData;
        sideFrameProperty = serializedObject.FindProperty("SideAnimationFrames");
        upFrameProperty = serializedObject.FindProperty("UpAnimationFrames");
        downFrameProperty = serializedObject.FindProperty("DownAnimationFrames");
        lengthAll = sideFrameProperty.arraySize;
    }

    /* No longer works because of different directions. */

    /*    public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            spriteIndex += 1;
            spriteIndex %= animData.SideAnimationFrames.Count*2;
            //Crop sprite into Texture.
            Sprite _spriteFrame = animData.SideAnimationFrames[spriteIndex/2].Sprite;
            Texture2D _croppedTexture = new Texture2D((int)_spriteFrame.rect.width, (int)_spriteFrame.rect.height);
            var _pixels = _spriteFrame.texture.GetPixels(
                (int)_spriteFrame.rect.x,
                (int)_spriteFrame.rect.y,
                (int)_spriteFrame.rect.width,
                (int)_spriteFrame.rect.height
            );
            _croppedTexture.SetPixels(_pixels);
            _croppedTexture.Apply();
            _croppedTexture.filterMode = FilterMode.Point;

            GUI.DrawTexture(r, _croppedTexture,ScaleMode.ScaleToFit);
        }*/

    public override void OnInspectorGUI()
    {
        //pre-gui shit
        serializedObject.Update();
        base.OnInspectorGUI();
        //Frame tick length control
        GUILayout.BeginHorizontal();

        GUILayout.Label("Frame Tick Length:");
        EditorGUILayout.Space(10);
        subFrameSetAll = GUILayout.TextField(subFrameSetAll);

        if (GUILayout.Button("Set All Tick Lengths"))
        {
            if (int.TryParse(subFrameSetAll, out int result))
            {
                for (int index = 0; index < sideFrameProperty.arraySize; index++)
                {
                    sideFrameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("HoldForTicks").uintValue = (uint)result;
                }
                for (int index = 0; index < upFrameProperty.arraySize; index++)
                {
                    upFrameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("HoldForTicks").uintValue = (uint)result;
                }
                for (int index = 0; index < downFrameProperty.arraySize; index++)
                {
                    downFrameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("HoldForTicks").uintValue = (uint)result;
                }
            }
        }
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();

        //Frame count control
        GUILayout.BeginHorizontal();

        GUILayout.Label("Frame Length:");
        EditorGUILayout.Space();
        lengthAll = EditorGUILayout.IntField(lengthAll);

        GUILayout.EndHorizontal();

        if (lengthAll == 0) lengthAll = 1; //clamp so atleast 1 frame.
        sideFrameProperty.arraySize = lengthAll;
        upFrameProperty.arraySize = lengthAll;
        downFrameProperty.arraySize = lengthAll;
        /*GUILayout.Label("Sprite Previews:");
        for (int index = 0; index < sideFrameProperty.arraySize; index ++)
        {
            //Fuck unity.
            sideFrameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("SpriteShown").SetUnderlyingValue(EditorGUILayout.ObjectField(sideFrameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("SpriteShown").objectReferenceValue, typeof(Sprite), true, GUILayout.Height(64), GUILayout.Width(64)) as Sprite);
        }*/
        sideFrameProperty.serializedObject.ApplyModifiedProperties();
        upFrameProperty.serializedObject.ApplyModifiedProperties();
        downFrameProperty.serializedObject.ApplyModifiedProperties();

    }

}
