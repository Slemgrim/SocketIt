using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEngine : RocketPart {

    public float consumption = 100;
    public float force = 10;
    public ParticleSystem nozzle;
    private Rigidbody rb;

    override public void Activate()
    {
        base.Activate();
        nozzle.Play();
        rb = GetComponent<Rigidbody>();
    }

	void FixedUpdate () {
        if (isActivated)
        {
            float fuel = controll.GetFuel(consumption * Time.deltaTime);
            if(fuel == 0)
            {
                nozzle.Stop();
                return;
            }

            Vector3 direction = nozzle.transform.forward * -1;

            rb.AddForceAtPosition(direction * force * fuel, nozzle.transform.position);
        }
	}
}
