using UnityEngine;

public class RocketPart : MonoBehaviour {

    public bool isActivated = false;
    public RocketControll controll = null;

    public float maxFuel;
    public float fuel;

    public float GetFuel(float amount)
    {
        if(fuel == 0)
        {
            return 0;
        }


        if(fuel <= amount)
        {
            fuel = 0;
            return fuel;
        }

        fuel -= amount;
        if(fuel < 0)
        {
            fuel = 0;
        }
        
        return amount;
    }

    public virtual void Activate()
    {
        isActivated = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
    }
}
