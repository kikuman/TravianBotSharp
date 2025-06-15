using MainCore.Entities;
using MainCore.Enums;
using MainCore.Infrasturecture.Persistence;

namespace MainCore.Services
{
    public static class ConstructionQueueExtensions
    {
        public static bool IsConstructionQueueFull(this AppDbContext context, AccountId accountId, VillageId villageId)
        {
            var queueCount = context.QueueBuildings
                .Where(x => x.VillageId == villageId.Value)
                .Where(x => x.Level != -1)
                .Where(x => x.Type != BuildingEnums.Site)
                .Count();

            var plusActive = context.AccountsInfo
                .Where(x => x.AccountId == accountId.Value)
                .Select(x => x.HasPlusAccount)
                .FirstOrDefault();

            var applyRomanQueueLogic = context.BooleanByName(villageId, VillageSettingEnums.ApplyRomanQueueLogicWhenBuilding);

            int limit = 1;
            if (plusActive) limit++;
            if (applyRomanQueueLogic) limit++;

            return queueCount >= limit;
        }
    }
}
