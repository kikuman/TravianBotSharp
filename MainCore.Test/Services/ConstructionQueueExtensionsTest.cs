using MainCore.Entities;
using MainCore.Enums;
using MainCore.Services;

namespace MainCore.Test.Services;

public class ConstructionQueueExtensionsTest
{
    [Theory]
    [InlineData(0, false, false, false)]
    [InlineData(1, false, false, true)]
    [InlineData(1, true, false, false)]
    [InlineData(2, true, false, true)]
    [InlineData(2, true, true, false)]
    [InlineData(3, true, true, true)]
    public void IsConstructionQueueFull_ReturnsExpected(int queueCount, bool plusActive, bool romanLogic, bool expected)
    {
        using var context = new FakeDbContextFactory().CreateDbContext(true);

        context.Add(new AccountInfo
        {
            AccountId = 1,
            Gold = 0,
            Silver = 0,
            HasPlusAccount = plusActive,
            Tribe = TribeEnums.Any,
        });

        context.Add(new VillageSetting
        {
            VillageId = 1,
            Setting = VillageSettingEnums.ApplyRomanQueueLogicWhenBuilding,
            Value = romanLogic ? 1 : 0,
        });

        for (int i = 0; i < queueCount; i++)
        {
            context.Add(new QueueBuilding
            {
                VillageId = 1,
                Position = i + 1,
                Location = i + 1,
                Type = BuildingEnums.Woodcutter,
                Level = 1,
                CompleteTime = DateTime.Now,
            });
        }

        context.SaveChanges();

        var result = context.IsConstructionQueueFull(new AccountId(1), new VillageId(1));
        result.ShouldBe(expected);
    }
}
