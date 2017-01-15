using UnityEngine;
using System.Collections;

namespace SocketIt
{
    public class CompositionManager : MonoBehaviour
    {
        private static CompositionManager instance = null;

        public Composition compositionPrefab;

        public static CompositionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(CompositionManager)) as CompositionManager;
                }

                if (instance == null)
                {
                    GameObject obj = new GameObject("CompositionController");
                    instance = obj.AddComponent(typeof(CompositionManager)) as CompositionManager;
                }

                return instance;
            }
        }

        void OnApplicationQuit()
        {
            instance = null;
        }
    }
}
