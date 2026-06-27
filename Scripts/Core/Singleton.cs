using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Global access to the singleton instance
    public static T Instance { get; private set; }

    [Header("Singleton Settings")]
    [SerializeField] private bool dontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        RegisterSingleton();
    }

    protected bool RegisterSingleton()
    {
        // Prevent duplicate instances
        if (Instance != null)
        {
            Destroy(gameObject);
            return false;
        }

        Instance = this as T;

        // Keep this object between scene loads if needed
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        return true;
    }

    protected virtual void OnDestroy()
    {
        // Clear the reference when the singleton is destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }
}