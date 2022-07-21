using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

interface IHittable
{
    void OnHit(Vector3 shotForceVector, Player shooter);
}
