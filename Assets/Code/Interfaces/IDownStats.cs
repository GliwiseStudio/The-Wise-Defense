public interface IDownStats
{
    public void ReceiveSlowdown(float slowdown);

    public void ReleaseSlowdown(float slowdown);

    public void ReceiveDamageReduction(float damageReduction);

    public void ReleaseDamageReduction(float damageReduction);

    public void ReceiveTimedDownStats(float slowdown, float damageReduction, float duration);

    public void ReceiveTimedParalysis(float duration);
}
