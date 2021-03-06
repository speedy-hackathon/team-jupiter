using covidSim.Models.Enums;

namespace covidSim.Models
{
    public class House : Building
    {
        public House(int id, Vec cornerCoordinates)
        {
            Id = id;
            Coordinates = new BuildingCoordinates(cornerCoordinates);
            BuildingType = BuildingType.House;
        }

        public int ResidentCount = 0;
    }
}