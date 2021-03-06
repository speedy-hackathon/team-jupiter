using System;
using System.Collections.Generic;
using System.Linq;
using covidSim.Models;
using covidSim.Models.Enums;

namespace covidSim.Services
{
    public class Game
    {
        public List<Person> People;
        public CityMap Map;
        private DateTime _lastUpdate;

        private static Game _gameInstance;
        private static Random _random = new Random();

        public const int PeopleCount = 320;
        public const int FieldWidth = 1000;
        public const int FieldHeight = 500;
        public const int MaxPeopleInHouse = 10;

        private Game()
        {
            Map = new CityMap();
            People = CreatePopulation();
            _lastUpdate = DateTime.Now;
        }

        public static Game Instance => _gameInstance ?? (_gameInstance = new Game());

        private List<Person> CreatePopulation()
        {
            return Enumerable
                .Repeat(0, PeopleCount)
                .Select((_, index) => new Person(index, FindHome(), Map)
                { Profession = index % 10 == 0 ? Profession.Doctor : Profession.Unemployed })
                .ToList();
        }

        public void Reset()
        {
            _gameInstance = new Game();
        }

        private int FindHome()
        {
            while (true)
            {
                var homeId = _random.Next(CityMap.BuildingAmount);

                if (Map.Buildings[homeId].BuildingType == BuildingType.House &&
                    ((House) Map.Buildings[homeId]).ResidentCount < MaxPeopleInHouse)
                {
                    ((House) Map.Buildings[homeId]).ResidentCount++;
                    return homeId;
                }
            }
        }

        public Game GetNextState()
        {
            var diff = (DateTime.Now - _lastUpdate).TotalMilliseconds;
            if (diff >= 1000)
            {
                CalcNextStep();
            }

            return this;
        }

        private void CalcNextStep()
        {
            _lastUpdate = DateTime.Now;
            foreach (var person in People)
            {
                person.CalcNextStep();
            }

            People.RemoveAll(person => person.ShouldBeRemoved());
        }
    }
}