using covidSim.Models.Enums;

namespace covidSim.Models
{
    public abstract class Building
    {
        public int Id;
        public BuildingCoordinates Coordinates;
        public BuildingType BuildingType;
    }
}