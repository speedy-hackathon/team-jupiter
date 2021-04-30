namespace covidSim.Models
{
    public class House
    {
        public House(int id, Vec cornerCoordinates)
        {
            Id = id;
            Coordinates = new HouseCoordinates(cornerCoordinates);
        }

        public int Id;
        public HouseCoordinates Coordinates;
        public int ResidentCount = 0;
    }
}