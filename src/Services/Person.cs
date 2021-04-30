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
            IsSick = random.NextDouble() < 0.05;
            stepsInHome = 0;
        }

        public bool IsBored
        {
            get => stepsInHome >= 5;
            set => stepsInHome = value ? 5 : 0;
        }


        public bool IsSick
        {
            get => sickTurns >= 0;
            set => sickTurns = value ? 0 : -1;
        }

        public void CalcNextStep()
        {
            if (IsSick) sickTurns++;
            if (sickTurns > MaxSickTurns) IsSick = false;
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
                case PersonState.GoingShop:
                    CalcNextPositionForGoingShopPerson();
                    break;
                case PersonState.AtShop:
                    if (atShopTurns < 10)
                        atShopTurns++;
                    else
                    {
                        state = PersonState.GoingHome;
                        CalcNextPositionForGoingHomePerson();
                    }

                    break;
            }
        }

        private void CalcNextStepForPersonAtHome()
        {
            var goingWalk = random.NextDouble() < 0.005;
            if (!goingWalk)
            {
                stepsInHome++;
                CalcNextPositionForPersonWalkingAtHome();
                return;
            }

            IsBored = false;
            if (random.Next(0, 10) > 3)
            {
                state = PersonState.Walking;
                CalcNextPositionForWalkingPerson();
            }
            else
            {
                state = PersonState.GoingShop;
                CalcNextPositionForGoingShopPerson();
            }
        }


        private void CalcSickStateForPersonAtHome()
        {
            if (IsSick)
                return;
            var thereAreSickPersonsInHome = Game.Instance.People.Any(x => x.IsSick
                                                                          && x.state == PersonState.AtHome
                                                                          && x.HomeId == HomeId);
            if (thereAreSickPersonsInHome)
            {
                var startsSick = random.NextDouble() < 0.5;
                if (startsSick)
                    IsSick = true;
            }
        }

        private void CalcNextPositionForPersonWalkingAtHome()
        {
            var newX = HomeCoords.X + random.Next(BuildingCoordinates.Width);
            var newY = HomeCoords.Y + random.Next(BuildingCoordinates.Height);
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
            var closePersons = Game.Instance.People.Where(other => GetDistanceTo(other.Position) < 7);
            var sickClosePersons = closePersons.Count(other => other.IsSick);
            for (var i = 0; i < sickClosePersons; i++)
            {
                if (random.Next() % 2 != 0) continue;
                IsSick = true;
                return;
            }
        }

        private double GetDistanceTo(Vec other)
        {
            return Math.Sqrt((Position.X - other.X) * (Position.X - other.X) +
                             (Position.Y - other.Y) * (Position.Y - other.Y));
        }

        private bool IsCorrectPosition(Vec pos)
        {
            return !Game.Instance.Map.Buildings.Where((x, i) => i != HomeId)
                .Any(x => x.Coordinates.LeftTopCorner.X < pos.X
                          && x.Coordinates.LeftTopCorner.X + BuildingCoordinates.Width > pos.X
                          && x.Coordinates.LeftTopCorner.Y < pos.Y
                          && x.Coordinates.LeftTopCorner.Y + BuildingCoordinates.Height > pos.Y);
        }

        private void CalcNextPositionForGoingShopPerson()
        {
            var game = Game.Instance;
            var shopCoord = game.Map.Buildings.Where(x => x.BuildingType == BuildingType.Shop)
                .OrderBy(x => GetDistanceTo(x.Coordinates.LeftTopCorner)).First();
            var shopCenter = new Vec(shopCoord.Coordinates.LeftTopCorner.X + BuildingCoordinates.Width / 2,
                shopCoord.Coordinates.LeftTopCorner.Y + BuildingCoordinates.Height / 2);

            var xDiff = shopCenter.X - Position.X;
            var yDiff = shopCenter.Y - Position.Y;
            var xDistance = Math.Abs(xDiff);
            var yDistance = Math.Abs(yDiff);

            var distance = xDistance + yDistance;
            if (distance <= MaxDistancePerTurn)
            {
                Position = shopCenter;
                state = PersonState.AtShop;
                atShopTurns = 0;
                return;
            }

            var direction = new Vec(Math.Sign(xDiff), Math.Sign(yDiff));

            var xLength = Math.Min(xDistance, MaxDistancePerTurn);
            var newX = Position.X + xLength * direction.X;
            var yLength = MaxDistancePerTurn - xLength;
            var newY = Position.Y + yLength * direction.Y;
            Position = new Vec(newX, newY);
        }

        private void CalcNextPositionForGoingHomePerson()
        {
            var game = Game.Instance;
            var homeCoord = game.Map.Buildings[HomeId].Coordinates.LeftTopCorner;
            var homeCenter = new Vec(homeCoord.X + BuildingCoordinates.Width / 2,
                homeCoord.Y + BuildingCoordinates.Height / 2);

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