using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    [RequireComponent(typeof(Module))]
    public class RocketSnapValidator : MonoBehaviour, ISnapValidator
    {
        public RocketControll Controll; 

        public bool Validate(Snap snap)
        {
            return snap.SocketB.Module.Composition != null && snap.SocketB.Module.Composition.Origin.GetComponent<RocketControll>() == Controll;
        }
    }
}