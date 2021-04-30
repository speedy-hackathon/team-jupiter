using covidSim.Models.Enums;

namespace covidSim.Models
{
    public class Shop : Building
    {
        public Shop(int id, Vec cornerCoordinates)
        {
            Id = id;
            Coordinates = new BuildingCoordinates(cornerCoordinates);
            BuildingType = BuildingType.Shop;
        }
    }
}