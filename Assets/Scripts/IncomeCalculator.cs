public static class IncomeCalculator
{
    public static float CalculateIncome(Business business, BusinessConfig config)
    {
        float multiplier = 1f;
        if (business.upgrade1Purchased) multiplier += config.upgrade1.multiplier / 100;
        if (business.upgrade2Purchased) multiplier += config.upgrade2.multiplier / 100;
        return business.level * config.baseIncome * multiplier;
    }

    public static float CalculateLevelUp(Business business, BusinessConfig config)
    {
        return (business.level + 1) * config.baseCost;
    }
}