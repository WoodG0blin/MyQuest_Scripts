namespace WG_Game
{
    public interface IDamagable
    {
        float Health { get; }
        void GetDamage(float damage);
    }
}
