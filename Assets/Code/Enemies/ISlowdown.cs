using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlowdown
{
    public void ReceiveSlowdown(float slowdown);

    public void ReleaseSlowdown(float slowdown);
}
