public interface IHeal
{
    public void Heal(int health);

    public bool FullHeal();

    public void BecomeInvulnerable(float duration);
}
