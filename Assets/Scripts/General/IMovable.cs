using UnityEngine;

internal interface IMovable : IPoolable
{
    public Vector3 Direction { get; }
    public float Speed { get; }
    public void SetDirection(Vector3 direction);
    public void SetSpeed(float speed);
}