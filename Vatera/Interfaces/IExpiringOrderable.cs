namespace Vatera.Interfaces
{
    interface IExpiringOrderable: IOrderable
    {
        int DaysToExpire { get; set; }
    }
}
