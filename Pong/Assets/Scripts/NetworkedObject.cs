using UnityEngine;

public abstract class NetworkedObject
{
    protected abstract float Initialize();
    
    protected abstract float GetNetworkId();
}
