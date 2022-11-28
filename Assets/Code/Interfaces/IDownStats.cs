using UnityEngine;

public interface IDownStats
{
    public void ReceiveSlowdown(float slowdown);

    public void ReleaseSlowdown(float slowdown);

    public void SetSlowdownObject(GameObject gameObject);

    public void RemoveSlowdownObject();

    public void ReceiveDamageReduction(float damageReduction);

    public void ReleaseDamageReduction(float damageReduction);

    public void ReceiveDamageOnLoop(int damage, float time);

    public void StopDamageLoop();

    public void ReceiveTimedDownStats(float slowdown, float damageReduction, float duration);

    public void ReceiveTimedParalysis(float duration);
}
