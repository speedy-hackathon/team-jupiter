using covidSim.Services;

namespace covidSim.Models
{
    public class GameDto
    {
        public GameDto(Person[] people)
        {
            People = people;
        }

        public Person[] People;
    }
}