using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    [RequireComponent(typeof(Module))]
    [AddComponentMenu("SocketIt/Snap Validators/Composition Validator")]
    public class CompositionValidator : MonoBehaviour, ISnapValidator
    {
        public bool Validate(Snap snap)
        {
            return snap.SocketB.Module.Composition != null;
        }
    }
}