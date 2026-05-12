using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FindGameObjectByFileID : EditorWindow
{
    public long fileID;

    [MenuItem("Tools/Find GameObject By FileID")]
    static void Init()
    {
        GetWindow<FindGameObjectByFileID>("Find GameObject");
    }

    void OnGUI()
    {
        fileID = EditorGUILayout.LongField("FileID:", fileID);

        if (GUILayout.Button("Find GameObject"))
        {
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (Find(go.transform, fileID, out GameObject found))
                {
                    Selection.activeGameObject = found;
                    EditorGUIUtility.PingObject(found);
                    Debug.Log("Found GameObject: " + found.name);
                    return;
                }
            }
            Debug.LogWarning("GameObject not found.");
        }
    }

    bool Find(Transform parent, long targetFileID, out GameObject result)
    {
        var so = new SerializedObject(parent.gameObject);
        var prop = so.FindProperty("m_LocalIdentfierInFile");
        if (prop != null && prop.longValue == targetFileID)
        {
            result = parent.gameObject;
            return true;
        }

        foreach (Transform child in parent)
        {
            if (Find(child, targetFileID, out result)) return true;
        }

        result = null;
        return false;
    }
}