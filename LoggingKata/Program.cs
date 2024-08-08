using System;
using System.IO;
using System.Linq;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");

            var path = "TacoBell-US-AL.csv";

            if (!File.Exists(path))
            {
                logger.LogFatal($"File not found: {path}");
                return;
            }

            var lines = File.ReadAllLines(path);

            if (lines.Length == 0)
            {
                logger.LogError("No lines found in file");
                return;
            }
            if (lines.Length == 1)
            {
                logger.LogWarning("Only one line found in file");
            }

            var parser = new TacoParser();
            var locations = lines.Select(parser.Parse).Where(loc => loc != null).ToList();

            ITrackable locA = null;
            ITrackable locB = null;
            double maxDistance = 0;

            foreach (var origin in locations)
            {
                var corA = new GeoCoordinate(origin.Location.Latitude, origin.Location.Longitude);

                foreach (var destination in locations)
                {
                    var corB = new GeoCoordinate(destination.Location.Latitude, destination.Location.Longitude);

                    var distance = corA.GetDistanceTo(corB);

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        locA = origin;
                        locB = destination;
                    }
                }
            }

            logger.LogInfo($"The two Taco Bells that are furthest from each other are:");
            logger.LogInfo($"{locA.Name} and {locB.Name}");
            logger.LogInfo($"They are {maxDistance / 1000} kilometers apart.");
        }
    }
}