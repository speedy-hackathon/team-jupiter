using System;
using System.Linq;
using covidSim.Models;
using covidSim.Models.Enums;

namespace covidSim.Services
{
    public class Person
    {
        public Vec HomeCoords;
        public int HomeId;
        public int Id;
        public Vec Position;

        private const int MaxDistancePerTurn = 30;
        private const int MaxSickTurns = 45;
        private int deathTurns;
        private const int MaxDeathTurns = 10;
        private const double DeathProbability = 0.00003;
        private static readonly Random random = new Random();
        private int sickTurns;
        private int atShopTurns;
        private PersonState state = PersonState.AtHome;
        private int stepsInHome;

        public Person(int id, int homeId, CityMap map)
        {
            Id = id;
            HomeId = homeId;

            HomeCoords = map.Buildings[homeId].Coordinates.LeftTopCorner;
            var x = HomeCoords.X + random.Next(BuildingCoordinates.Width);
            var y = HomeCoords.Y + random.Next(BuildingCoordinates.Height);
            Position = new Vec(x, y);
            if (random.NextDouble() < 0.05) HealthState = PersonHealthState.Sick;
        }

        public PersonHealthState HealthState = PersonHealthState.Healthy;

        public bool ShouldBeRemoved() => deathTurns > MaxDeathTurns;
        public bool IsBored
        {
            get => stepsInHome >= 5;
            set => stepsInHome = value ? 5 : 0;
        }

        public void CalcNextStep()
        {
            if (HealthState == PersonHealthState.Sick)
            {
                sickTurns++;
                if (sickTurns > MaxSickTurns)
                {
                    sickTurns = 0;
                    HealthState = PersonHealthState.Healthy;
                }
                else if (random.NextDouble() < DeathProbability) HealthState = PersonHealthState.Dead;
            }
            if (HealthState == PersonHealthState.Dead) deathTurns++;
            else
            {
                switch (state)
                {
                    case PersonState.AtHome:
                        CalcNextStepForPersonAtHome();
                        CalcSickStateForPersonAtHome();
                        break;
                    case PersonState.Walking:
                        CalcNextPositionForWalkingPerson();
                        CalcSickStateForWalkingPerson();
                        break;
                    case PersonState.GoingHome:
                        CalcNextPositionForGoingHomePerson();
                        break;
                }
            }
        }

        private void CalcNextStepForPersonAtHome()
        {
            var goingWalk = random.NextDouble() < 0.005;
            if (!goingWalk) CalcNextPositionForPersonWalkingAtHome();

            state = PersonState.Walking;
            CalcNextPositionForWalkingPerson();
        }

        private void CalcSickStateForPersonAtHome()
        {
            if (HealthState == PersonHealthState.Sick)
                return;
            var thereAreSickPersonsInHome = Game.Instance.People.Any(x => x.HealthState == PersonHealthState.Sick
                                                                          && x.state == PersonState.AtHome
                                                                          && x.HomeId == HomeId);
            if (thereAreSickPersonsInHome)
            {
                var startsSick = random.NextDouble() < 0.5;
                if (startsSick)
                    HealthState = PersonHealthState.Sick;
            }
        }

        private void CalcNextPositionForPersonWalkingAtHome()
        {
            var newX = HomeCoords.X + random.Next(HouseCoordinates.Width);
            var newY = HomeCoords.Y + random.Next(HouseCoordinates.Height);
            Position = new Vec(newX, newY);
        }

        private void CalcNextPositionForWalkingPerson()
        {
            Vec nextPosition = null;

            var xLength = random.Next(MaxDistancePerTurn);
            var yLength = MaxDistancePerTurn - xLength;
            foreach (var direction in ChooseDirection())
            {
                var delta = new Vec(xLength * direction.X, yLength * direction.Y);
                nextPosition = new Vec(Position.X + delta.X, Position.Y + delta.Y);
                if (IsCorrectPosition(nextPosition))
                    break;
            }


            if (isCoordInField(nextPosition))
                Position = nextPosition;
            else
                CalcNextPositionForWalkingPerson();
        }

        private void CalcSickStateForWalkingPerson()
        {
            if (IsSick)
                return;
            var closePersons = Game.Instance.People.Where(other => GetDistanceTo(other) < 7);
            var sickClosePersons = closePersons.Count(other => other.IsSick);
            for (var i = 0; i < sickClosePersons; i++)
            {
                if (random.Next() % 2 != 0) continue;
                IsSick = true;
                return;
            }
        }

        private double GetDistanceTo(Person other)
        {
            return Math.Sqrt((Position.X - other.Position.X) * (Position.X - other.Position.X) +
                             (Position.Y - other.Position.Y) * (Position.Y - other.Position.Y));
        }

        private bool IsCorrectPosition(Vec pos)
        {
            return !Game.Instance.Map.Houses.Where((x, i) => i != HomeId)
                .Any(x => x.Coordinates.LeftTopCorner.X < pos.X
                          && x.Coordinates.LeftTopCorner.X + HouseCoordinates.Width > pos.X
                          && x.Coordinates.LeftTopCorner.Y < pos.Y
                          && x.Coordinates.LeftTopCorner.Y + HouseCoordinates.Height > pos.Y);
        }

        private void CalcNextPositionForGoingHomePerson()
        {
            var game = Game.Instance;
            var homeCoord = game.Map.Houses[HomeId].Coordinates.LeftTopCorner;
            var homeCenter = new Vec(homeCoord.X + HouseCoordinates.Width / 2,
                homeCoord.Y + HouseCoordinates.Height / 2);

            var xDiff = homeCenter.X - Position.X;
            var yDiff = homeCenter.Y - Position.Y;
            var xDistance = Math.Abs(xDiff);
            var yDistance = Math.Abs(yDiff);

            var distance = xDistance + yDistance;
            if (distance <= MaxDistancePerTurn)
            {
                Position = homeCenter;
                state = PersonState.AtHome;
                return;
            }

            var direction = new Vec(Math.Sign(xDiff), Math.Sign(yDiff));

            var xLength = Math.Min(xDistance, MaxDistancePerTurn);
            var newX = Position.X + xLength * direction.X;
            var yLength = MaxDistancePerTurn - xLength;
            var newY = Position.Y + yLength * direction.Y;
            Position = new Vec(newX, newY);
        }

        public void GoHome()
        {
            if (state != PersonState.Walking) return;

            state = PersonState.GoingHome;
            CalcNextPositionForGoingHomePerson();
        }

        private Vec[] ChooseDirection()
        {
            var directions = new[]
            {
                new Vec(-1, -1),
                new Vec(-1, 1),
                new Vec(1, -1),
                new Vec(1, 1)
            };
            return directions.OrderBy(x => random.Next()).ToArray();
        }

        private bool isCoordInField(Vec vec)
        {
            var belowZero = vec.X < 0 || vec.Y < 0;
            var beyondField = vec.X > Game.FieldWidth || vec.Y > Game.FieldHeight;

            return !(belowZero || beyondField);
        }
    }
}