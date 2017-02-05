using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIt;

public class RocketControll : MonoBehaviour {

	public List<RocketPart> parts = new List<RocketPart>();

    private Composition composition;

    public void Start()
    {
        composition = GetComponent<Composition>();

        Module part = GetComponent<Module>();
        if(part != null)
        {
            AddPart(part);
        }
    }

    public void AddPart(Module module)
    {
        RocketPart part = module.GetComponent<RocketPart>();
        if(part == null)
        {
            return;
        }

        if (!parts.Contains(part))
        {
            parts.Add(part);
            part.controll = this;
        }
    }

    public void RemovePart(Module module)
    {
        RocketPart part = module.GetComponent<RocketPart>();
        if (part == null)
        {
            return;
        }

        if (parts.Contains(part))
        {
            part.controll = null;
            parts.Remove(part);
        }
    }

    public void Launch()
    {
        foreach(Module module in composition.Modules)
        {
            module.transform.SetParent(null);
        }

        foreach(Connection connection in composition.Connections)
        {
            FixedJoint joint = connection.Connector.Module.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = connection.Connectee.Module.GetComponent<Rigidbody>();
        }

        foreach (RocketPart part in parts)
        {
            part.Activate();
        }
    }

    public float GetFuel(float amount)
    {
        float fuel = 0;

        foreach (RocketPart part in parts)
        {
            if(fuel < amount)
            {
                float diff =  amount - fuel;
                fuel += part.GetFuel(diff);
            }
        }

        return fuel;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Launch");
            Launch();
        }     
    }

}
