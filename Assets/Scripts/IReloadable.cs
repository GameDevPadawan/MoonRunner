public interface IReloadable
{
    void ReloadFully();
    void ReloadAmount(int amount);
    bool NeedsReload();
}
