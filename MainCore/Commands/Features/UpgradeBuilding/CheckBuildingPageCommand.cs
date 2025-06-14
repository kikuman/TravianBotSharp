using MainCore.Constraints;
using MainCore.Parsers;

namespace MainCore.Commands.Features.UpgradeBuilding
{
    [Handler]
    public static partial class CheckBuildingPageCommand
    {
        public sealed record Command(AccountId AccountId, VillageId VillageId, NormalBuildPlan Plan, JobId JobId) : IAccountVillageCommand;

        private static async ValueTask<Result> HandleAsync(
            Command command,
            IChromeBrowser browser,
            DeleteJobByIdCommand.Handler deleteJobByIdCommand,
            JobUpdated.Handler jobUpdated,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var (accountId, villageId, plan, jobId) = command;

            var html = browser.Html;
            var upgradingLevel = UpgradeParser.GetUpgradingLevel(html);
            var maxLevelRunning = UpgradeParser.IsMaxLevelUnderConstruction(html);

            if (maxLevelRunning || (upgradingLevel.HasValue && upgradingLevel.Value >= plan.Level))
            {
                await deleteJobByIdCommand.HandleAsync(new(villageId, jobId), cancellationToken);
                await jobUpdated.HandleAsync(new(accountId, villageId), cancellationToken);
                return Continue.Error;
            }

            return Result.Ok();
        }
    }
}
