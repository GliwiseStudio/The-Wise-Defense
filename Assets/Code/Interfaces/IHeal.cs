public interface IHeal
{
    public void Heal(float healPercentage);

    public bool FullHeal();

    public void BecomeInvulnerable(float duration);
}
