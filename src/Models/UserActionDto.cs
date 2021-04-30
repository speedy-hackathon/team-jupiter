namespace covidSim.Models
{
    public class UserActionDto
    {
        public UserActionDto(int personClicked)
        {
            PersonClicked = personClicked;
        }

        public int PersonClicked;
    }
}