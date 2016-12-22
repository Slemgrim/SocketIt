using UnityEngine;
using System.Collections;

public class CannonControll : MonoBehaviour {

    public TriggerController controller;
    public LaserPointer laserPointer;
    public Chamber chamber;
    public Turret turret;

    void Start()
    {
        laserPointer.OnTargetFound += OnLaserPointerTarget;
        controller.OnTriggerPull += OnControllerFire;
    }

    private void OnLaserPointerTarget(RaycastHit hit)
    {
        turret.Aim(hit.point);
    }

    private void OnControllerFire()
    {
        chamber.Load();
        chamber.Fire();
    }
}
