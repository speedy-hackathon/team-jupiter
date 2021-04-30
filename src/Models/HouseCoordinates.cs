namespace covidSim.Models
{
    public class HouseCoordinates
    {
        public HouseCoordinates(Vec leftTopCorner)
        {
            LeftTopCorner = leftTopCorner;
        }
        
        public Vec LeftTopCorner;
        public const int Width = 50;
        public const int Height = 50;
    }
}