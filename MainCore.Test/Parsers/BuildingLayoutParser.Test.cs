using MainCore.Enums;
using Shouldly;
using System.Collections.Generic;

namespace MainCore.Test.Parsers
{
    public class BuildingLayoutParser : BaseParser
    {
        private const string Buildings = "Parsers/BuildingLayout/Buildings.html";
        private const string Resources = "Parsers/BuildingLayout/Resources.html";
        private const string BuildingsWithWall = "Parsers/BuildingLayout/BuildingsWithWall.html";
        private const string BuildingsWithQueue = "Parsers/BuildingLayout/BuildingsWithQueue.html";
        private const string UnderConstructionTitle = "Parsers/BuildingLayout/UnderConstructionTitle.html";

        [Fact]
        public void GetFields()
        {
            _html.Load(Resources);
            var actual = MainCore.Parsers.BuildingLayoutParser.GetFields(_html);
            actual.Count().ShouldBe(18);
            actual.ShouldNotContain(x => x.Location == -1 || x.Type == BuildingEnums.Unknown);
        }

        [Fact]
        public void GetInfrastructures()
        {
            _html.Load(Buildings);
            var actual = MainCore.Parsers.BuildingLayoutParser.GetInfrastructures(_html);
            actual.Count().ShouldBe(22);
            actual.ShouldNotContain(x => x.Location == -1 || x.Type == BuildingEnums.Unknown);
        }

        [Fact]
        public void GetInfrastructures_WithWall()
        {
            _html.Load(BuildingsWithWall);
            var actual = MainCore.Parsers.BuildingLayoutParser.GetInfrastructures(_html);
            actual.Count().ShouldBe(22);
            actual.ShouldNotContain(x => x.Location == -1 || x.Type == BuildingEnums.Unknown);
        }

        [Theory]
        [InlineData(Buildings, 0)]
        [InlineData(Resources, 0)]
        [InlineData(BuildingsWithQueue, 2)]
        public void GetQueueBuilding(string path, int expected)
        {
            _html.Load(path);
            var actual = MainCore.Parsers.BuildingLayoutParser.GetQueueBuilding(_html);
            actual.Count(x => x.Level != -1).ShouldBe(expected);
        }

        [Fact]
        public void GetTitleUpgradeInfo()
        {
            _html.Load(UnderConstructionTitle);
            var (levels, isMax) = MainCore.Parsers.BuildingLayoutParser.GetTitleUpgradeInfo(_html, 7);
            levels.ShouldBe(new List<int> { 9, 10 });
            isMax.ShouldBeTrue();
        }
    }
}