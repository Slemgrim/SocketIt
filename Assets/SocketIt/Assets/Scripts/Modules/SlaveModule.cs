using UnityEngine;
using System.Collections;
using System;

namespace SocketIt
{
    public class SlaveModule : MonoBehaviour, ISnapValidator
    {
        public MasterModule Master;

        public bool Validate(Snap snap)
        {
            MasterModule master = snap.SocketB.Module.GetComponent<MasterModule>();
            SlaveModule slave = snap.SocketB.Module.GetComponent<SlaveModule>();

            return master != null || (slave != null && slave.Master != null);  
        }
    }
}
