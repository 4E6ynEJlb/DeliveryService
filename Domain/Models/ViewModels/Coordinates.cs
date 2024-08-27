using Domain.Models.Entities.SQLEntities;

namespace Domain.Models.VievModels
{
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Coordinates() { }
        public Coordinates(Restaurant restaurant)
        {
            Latitude = restaurant.Latitude; ;
            Longitude = restaurant.Longitude;
        }
        static public double CalcDistance(Coordinates coords1, Coordinates coords2)
        {
            double deg2rad(double deg)
            {
                return deg * (Math.PI / 180);
            }

            int r = 6371;
            var dLat = deg2rad(coords2.Latitude - coords1.Latitude);
            var dLon = deg2rad(coords2.Longitude - coords1.Longitude);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(coords1.Latitude)) * Math.Cos(deg2rad(coords2.Latitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = r * c;
            return d;
        }
        public double CalcDistance(Coordinates coords)
        {
            return CalcDistance(this, coords);
        }
    }
}
