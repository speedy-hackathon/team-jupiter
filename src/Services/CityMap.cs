using System;
using covidSim.Models;

namespace covidSim.Services
{
    public class CityMap
    {
        public House[] Houses;

        private const int ComfortablePeopleAmountInHouse = 5;
        private const int HousesInGroup = 4;
        private const int GroupsInRow = 4; // |..|..|..|..|  |..|..|..|..|  |..|..|..|..|  |..|..|..|..|
        
        public const int HouseAmount = Game.PeopleCount / ComfortablePeopleAmountInHouse;

        public CityMap()
        {
            Houses = CreateMap();
        }

        private static House[] CreateMap()
        {
            const int housesInRow = HousesInGroup * GroupsInRow;
            const int groupWidth = HouseCoordinates.Width * HousesInGroup;
            const int gapInRow = (Game.FieldWidth - groupWidth * GroupsInRow) / (GroupsInRow - 1);
            var rowsAmount = (int)Math.Ceiling((double)HouseAmount / housesInRow);
            var gapBetweenRows = (Game.FieldHeight - HouseCoordinates.Height * rowsAmount) / (rowsAmount - 1); 
            
            
            var map = new House[HouseAmount];
            
            for (int i = 0; i < HouseAmount; i++)
            {
                var row = i / housesInRow;
                var y = row * HouseCoordinates.Height + row * gapBetweenRows;
                
                var col = i % housesInRow;
                var groupNum = col / HousesInGroup;
                var houseInGroup = col % HousesInGroup;
                var x = groupNum * groupWidth + groupNum * gapInRow + houseInGroup * HouseCoordinates.Width;
                
                var coordinate = new Vec(x, y);
                map[i] = new House(i, coordinate);
            }

            return map;
        }
    }
}