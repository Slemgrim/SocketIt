using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    public class CompositionManager : MonoBehaviour
    {
        private static CompositionManager instance = null;

        public Composition compositionPrefab;
        public bool destroyEmptyCompositions = false;

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
                    GameObject obj = new GameObject("CompositionManager");
                    instance = obj.AddComponent(typeof(CompositionManager)) as CompositionManager;
                }

                return instance;
            }
        }

        public Composition CreateComposition()
        {
            if (compositionPrefab == null)
            {
                throw new SocketItException("Composition manager has no composition prefab");
            }

            Composition composition = Instantiate(compositionPrefab).GetComponent<Composition>();

            composition.OnCompositionEmpty.AddListener(RemoveEmptyCompositions);

            return composition;
        }

        private void RemoveEmptyCompositions(Composition composition)
        {
            if (destroyEmptyCompositions)
            {
                composition.OnCompositionEmpty.RemoveListener(RemoveEmptyCompositions);
                Destroy(composition.gameObject);
            }
        }

        void OnApplicationQuit()
        {
            instance = null;
        }
    }
}
