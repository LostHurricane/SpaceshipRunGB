using Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShipCollider : NetworkBehaviour
{
    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlanetOrbit> (out var planet))
        {
            RpcRespawn();
        }
    }
    [ClientRpc]
    private void RpcRespawn()
    {
        gameObject.SetActive(false);
        transform.position = NetworkManager.singleton.GetStartPosition().position;
        gameObject.SetActive(true);
    }
}
