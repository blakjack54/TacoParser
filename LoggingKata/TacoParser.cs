using System;

namespace LoggingKata
{
    public class TacoParser
    {
        readonly ILog logger = new TacoLogger();

        public ITrackable Parse(string line)
        {
            logger.LogInfo("Begin parsing");

            var cells = line.Split(',');

            if (cells.Length < 3)
            {
                logger.LogError("Invalid line: less than 3 elements");
                return null;
            }

            if (double.TryParse(cells[0], out double latitude) &&
                double.TryParse(cells[1], out double longitude) &&
                !string.IsNullOrEmpty(cells[2]))
            {
                var name = cells[2];

                var tacoBell = new TacoBell
                {
                    Name = name,
                    Location = new Point { Latitude = latitude, Longitude = longitude }
                };

                logger.LogInfo($"Parsed TacoBell: {name} at ({latitude}, {longitude})");

                return tacoBell;
            }
            else
            {
                logger.LogError("Invalid line: unable to parse latitude or longitude");
                return null;
            }
        }
    }
}