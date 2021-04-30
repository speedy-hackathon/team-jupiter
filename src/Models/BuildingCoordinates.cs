namespace covidSim.Models
{
    public class BuildingCoordinates
    {
        public BuildingCoordinates(Vec leftTopCorner)
        {
            LeftTopCorner = leftTopCorner;
        }
        
        public Vec LeftTopCorner;
        public const int Width = 50;
        public const int Height = 50;
    }
}