using System;
using System.Linq;
using covidSim.Models;

namespace covidSim.Services
{
    public class CityMap
    {
        public Building[] Buildings;

        private const int ComfortablePeopleAmountInHouse = 5;
        private const int HousesInGroup = 8;
        private const int GroupsInRow = 2; // |..|..|..|..|..|..|..|..|  |..|..|..|..|..|..|..|..|


        public const int BuildingAmount = Game.PeopleCount / ComfortablePeopleAmountInHouse;
        public static readonly int[] ShopsId = {1, BuildingAmount - 1};

        public CityMap()
        {
            Buildings = CreateMap();
        }

        private static Building[] CreateMap()
        {
            const int housesInRow = HousesInGroup * GroupsInRow;
            const int groupWidth = BuildingCoordinates.Width * HousesInGroup;
            const int gapInRow = (Game.FieldWidth - groupWidth * GroupsInRow) / (GroupsInRow - 1);
            var rowsAmount = (int) Math.Ceiling((double) BuildingAmount / housesInRow);
            var gapBetweenRows = (Game.FieldHeight - BuildingCoordinates.Height * rowsAmount) / (rowsAmount - 1);


            var map = Enumerable.Range(0, BuildingAmount)
                .Select(i => GetBuildingCoordinate(i, housesInRow, gapBetweenRows, groupWidth, gapInRow))
                .Select((x, i) => ShopsId.Contains(i) ? (Building) new Shop(i, x) : new House(i, x)).ToArray();
            return map;
        }

        private static Vec GetBuildingCoordinate(int i, int housesInRow, int gapBetweenRows, int groupWidth,
            int gapInRow)
        {
            var row = i / housesInRow;
            var y = row * BuildingCoordinates.Height + row * gapBetweenRows;

            var col = i % housesInRow;
            var groupNum = col / HousesInGroup;
            var houseInGroup = col % HousesInGroup;
            var x = groupNum * groupWidth + groupNum * gapInRow + houseInGroup * BuildingCoordinates.Width;

            var coordinate = new Vec(x, y);
            return coordinate;
        }
    }
}