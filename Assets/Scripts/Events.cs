public struct LevelUpEvent : IBusinessEvent 
{
    public int BusinessEntity { get; set; }
}

public struct Upgrade1Event : IBusinessEvent 
{
    public int BusinessEntity { get; set; }
}

public struct Upgrade2Event : IBusinessEvent 
{
    public int BusinessEntity { get; set; }
}
