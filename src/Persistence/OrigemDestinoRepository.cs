using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DbfDataReader;
using OrigemDestino.Core;

namespace OrigemDestino.Persistence
{
    public class OrigemDestinoRepository : IOrigemDestinoRepository
    {
        public ICollection<Location> GetAllFromDbf()
        {
            var locals = new List<Location>();
            var coordinateRecords = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(2, 3),
                new Tuple<int, int>(57, 58),
                new Tuple<int, int>(62, 63),
                new Tuple<int, int>(71, 72),
                new Tuple<int, int>(84, 85),
                new Tuple<int, int>(89, 90),
                new Tuple<int, int>(92, 93),
                new Tuple<int, int>(96, 97),
                new Tuple<int, int>(100, 101)
            };

            using var table = new DbfTable("Data/OD_2017.dbf", EncodingProvider.UTF8);

            var record = new DbfRecord(table);

            var personId = 1;

            while (table.Read(record))
            {
                foreach (var coordinate in coordinateRecords)
                {
                    try
                    {
                        if (record.Values[coordinate.Item1].GetValue() != null
                            && record.Values[coordinate.Item2].GetValue() != null)
                        {
                            var x = (int)record.Values[coordinate.Item1].GetValue();
                            var y = (int)record.Values[coordinate.Item2].GetValue();

                            AddCoordinate(personId, x, y, locals);
                        }
                    }
                    catch
                    {
                    }
                }
                personId++;
            }

            return locals;
        }

        private void AddCoordinate(int frequenterId, int x, int y, ICollection<Location> locations)
        {
            var location = locations.SingleOrDefault(l => l.X == x && l.Y == y);
            var frequenter = locations.SelectMany(l => l.LocationFrequenters).Select(lf => lf.Frequenter).FirstOrDefault(f => f.Id == frequenterId);

            if (location == null)
            {
                location = new Location
                {
                    X = x,
                    Y = y
                };

                locations.Add(location);
            }

            if (frequenter == null)
            {
                frequenter = new Frequenter { Id = frequenterId };
            }

            location.AddFrequenter(frequenter);
        }
    }
}