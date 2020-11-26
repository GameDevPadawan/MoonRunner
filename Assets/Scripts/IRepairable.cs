public interface IRepairable
{
    void RepairFully();
    void RepairAmount(float amount);

    float GetRepairAmountNeeded();
}