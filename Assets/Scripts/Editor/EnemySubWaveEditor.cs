using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySubWave))]
public class EnemySubWaveEditor : Editor
{
    EnemySubWave waveWeAreEditing;

    private void OnEnable()
    {
        waveWeAreEditing = target as EnemySubWave;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (waveWeAreEditing.EnemyPrefab != null)
        {
            EditorGUILayout.IntField(nameof(waveWeAreEditing.TotalWaveHealth), waveWeAreEditing.TotalWaveHealth); 
        }
    }
}
