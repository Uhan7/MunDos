using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class Category : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public CategoryType category;
    [SerializeField] private bool lastActiveState;

    [Header("Properties")]
    [SerializeField] private bool changeRoot;

    [Header("Conditionals Interactions")]
    [ShowIf("changeRoot")][SerializeField] public GameObject rootObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        InitializeCache();
    }

    public void SetActive(bool state)
    {
        if (lastActiveState == state) return;

        gameObject.SetActive(state);
        lastActiveState = state;
    }
    // Helper Functions --------------------------------------------------------

    void InitializeCache()
    {
        lastActiveState = true;
        if (!rootObject)
        {
            if(!changeRoot) Debug.Log(this.gameObject.name + "root category not set");
            FindRoot(CategoryType.Room);
        }
        
    }

    void FindRoot(CategoryType value)
    {
        Transform current = this.transform;
        while (current != null)
        {
            Category category = current.GetComponent<Category>();
            if ((category.category == value))
            {
                rootObject = category.gameObject;
                break;
            }
            current = current.parent;
        }
    }
}
