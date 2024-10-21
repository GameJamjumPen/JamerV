using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(EnemyType))]
public class EnemyTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyType enemyTypeScript = (EnemyType)target;

        // Iterate through the outer list
        for (int i = 0; i < enemyTypeScript.enemyGrid.Count; i++)
        {
            EditorGUILayout.LabelField("Row " + i);

            // Make sure the inner list exists
            if (enemyTypeScript.enemyGrid[i] == null)
            {
                enemyTypeScript.enemyGrid[i] = new List<EnemyType.Enemytype>();
            }

            // Iterate through the inner list
            for (int j = 0; j < enemyTypeScript.enemyGrid[i].Count; j++)
            {
                enemyTypeScript.enemyGrid[i][j] = (EnemyType.Enemytype)EditorGUILayout.EnumPopup("Enemy " + j, enemyTypeScript.enemyGrid[i][j]);
            }

            // Buttons to add or remove enemies from a row
            if (GUILayout.Button("Add Enemy to Row " + i))
            {
                enemyTypeScript.enemyGrid[i].Add(EnemyType.Enemytype.Fish); // Default to Fish
            }
            if (GUILayout.Button("Remove Last Enemy from Row " + i))
            {
                if (enemyTypeScript.enemyGrid[i].Count > 0)
                    enemyTypeScript.enemyGrid[i].RemoveAt(enemyTypeScript.enemyGrid[i].Count - 1);
            }
        }

        // Buttons to add or remove rows
        if (GUILayout.Button("Add Row"))
        {
            enemyTypeScript.enemyGrid.Add(new List<EnemyType.Enemytype>()); // Add a new row
        }
        if (GUILayout.Button("Remove Last Row"))
        {
            if (enemyTypeScript.enemyGrid.Count > 0)
                enemyTypeScript.enemyGrid.RemoveAt(enemyTypeScript.enemyGrid.Count - 1); // Remove the last row
        }

        // Apply changes to the target object if anything was modified
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
