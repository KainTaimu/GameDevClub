namespace Game.Core.ECS;

public readonly struct NodeProxyComponent(Node proxy)
{
    public readonly Node Proxy = proxy;
}
