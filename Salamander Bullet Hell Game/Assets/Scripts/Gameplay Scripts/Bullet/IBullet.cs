using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    public void SetCorrectLayer();
    public void SetDefaultVelocity();
    public Vector2 GetVelocity();
    public void SetVelocity(Vector2 velocity);
    public float GetVelocityMagnitude();
    public void SetSize(float size);
    public void SetSpeed(float speed);
    public void Shoot();
}
