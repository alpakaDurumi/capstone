using UnityEngine;
using UnityEngine.Assertions;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if(_instance == null)
                {
                    var newGameObj = new GameObject(typeof(T).ToString());
                    newGameObj.AddComponent<T>();
                    DontDestroyOnLoad(newGameObj);
                    _instance = newGameObj.GetComponent<T>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(this != _instance)
        {
            //Destroy(this);
        }

    }
}