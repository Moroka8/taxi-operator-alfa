using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Threading;

namespace WpfApp1
{
    internal class MapObject
    {
        private string title;
        private DateTime creationTime;

        public MapObject(string title)
        {
            this.title = title;
            creationTime = DateTime.Now;
        }
        public string getTitle()
        {
            return title;
        }
        public DateTime getCreationDate()
        {
            return creationTime;
        }
        public virtual void getFocus(MainWindow mainWindow)
        {
        }
        public virtual double getDistance(PointLatLng point)
        {
            return new double();
        }
    }
    class Area : MapObject
    {
        List<PointLatLng> points = new List<PointLatLng>();
        public Area(string title, List<PointLatLng> points) : base(title)
        {
            for (int i = 0; i < points.Count; i++)
            {
                this.points.Add(points[i]);
            }
        }
        public PointLatLng getCenter()
        {
            double sum_lat = new double();
            double sum_lng = new double();
            for (int i = 0; i < points.Count; i++)
            {
                PointLatLng point = points[i];
                sum_lat += point.Lat;
                sum_lng += point.Lng;
            }
            sum_lat /= points.Count;
            sum_lng /= points.Count;
            return new PointLatLng(sum_lat, sum_lng);
        }
        public override double getDistance(PointLatLng point)
        {
            PointLatLng area_center = this.getCenter();
            GeoCoordinate c1 = new GeoCoordinate(area_center.Lat, area_center.Lng);
            GeoCoordinate c2 = new GeoCoordinate(point.Lat, point.Lng);
            double distance = c1.GetDistanceTo(c2);
            return distance;
        }
        public override void getFocus(MainWindow mainWindow)
        {
            mainWindow.Map.Position = this.getCenter();
        }
        public void getMarker(MainWindow mainWindow)
        {
            Trace.WriteLine("used1");
            GMapMarker marker = new GMapPolygon(points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.Black, // стиль обводки
                    Fill = Brushes.Violet, // стиль заливки
                    Opacity = 0.7, // прозрачность
                    ToolTip = this.getTitle()
                }
            };
            mainWindow.Map.Markers.Add(marker);

        }
        public void getPoints()
        {
            Trace.WriteLine(points.Count);
        }
    }
    class Car : MapObject
    {
        PointLatLng point;
        GMapMarker marker;
        public Car(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }
        public override double getDistance(PointLatLng point)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(this.point.Lat, this.point.Lng);
            double distance = c1.GetDistanceTo(c2);
            return distance;
        }
        public override void getFocus(MainWindow mainWindow)
        {
            mainWindow.Map.Position = point;
        }
        public void getMarker(MainWindow mainWindow)
        {
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера
                    ToolTip = this.getTitle(), // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/car.png")) // картинка
                }
            };
            this.marker = marker;
            mainWindow.Map.Markers.Add(marker);
        }
        async public void moveTo(Human passenger_marker, PointLatLng hum_pos, PointLatLng end, MainWindow mainWindow)
        {
            mainWindow.taxi_progress.Visibility = System.Windows.Visibility.Visible;  
            MapRoute route = GMap.NET.MapProviders.BingMapProvider.Instance.GetRoute(
             this.point, // начальная точка маршрута
             hum_pos, // конечная точка маршрута
             false, // поиск по шоссе (false - включен)
             false, // режим пешехода (false - выключен)
             15);
            // получение точек маршрута
            List<PointLatLng> routePoints = route.Points;
            Route route1 = new Route("route", routePoints);
            route1.getMarker(mainWindow);
            int current_car = mainWindow.Map.Markers.IndexOf(marker);
            int passenger = mainWindow.Map.Markers.IndexOf(passenger_marker.returnMarker());
            foreach (var point in route1.getPoints())
            {
                mainWindow.taxi_progress.Maximum = route1.getPoints().Count;
                this.point = point;
                mainWindow.Map.Markers[current_car].Position = point;
                mainWindow.taxi_progress.Value += 1;
                await Task.Delay(500);
            }
            mainWindow.taxi_progress.Value = 0;
            for (int i = 0; i < route1.returnMarkers().Count; i++)
            {
                mainWindow.Map.Markers.Remove(route1.returnMarkers()[i]);
            }
            await Task.Delay(1000);
            route = GMap.NET.MapProviders.BingMapProvider.Instance.GetRoute(
             this.point, // начальная точка маршрута
             end, // конечная точка маршрута
             false, // поиск по шоссе (false - включен)
             false, // режим пешехода (false - выключен)
             15);
            // получение точек маршрута
            routePoints = route.Points;
            Route route2 = new Route("route", routePoints);
            route2.getMarker(mainWindow);
            foreach (var point in route2.getPoints())
            {
                mainWindow.taxi_progress.Maximum = route2.getPoints().Count;
                this.point = point;
                mainWindow.Map.Markers[passenger].Position = point;
                mainWindow.Map.Markers[current_car].Position = point;
                mainWindow.taxi_progress.Value += 1;
                await Task.Delay(500);
            }
            for (int i = 0; i < route2.returnMarkers().Count; i++)
            {
                mainWindow.Map.Markers.Remove(route2.returnMarkers()[i]);
            }
            mainWindow.taxi_progress.Value = 0;
            mainWindow.taxi_progress.Visibility = System.Windows.Visibility.Hidden;

        }


    }
    class Route : MapObject
    {

        List<PointLatLng> points = new List<PointLatLng>();
        List<GMapMarker> markers = new List<GMapMarker>();
        public Route(string title, List<PointLatLng> points) : base(title)
        {
            for (int i = 0; i < points.Count; i++)
            {
                this.points.Add(points[i]);
            }
        }
        public List<GMapMarker> returnMarkers()
        {
            return markers;
        }
        public List<PointLatLng> getPoints()
        {
            return points;
        }
        public PointLatLng getCenter()
        {
            double sum_lat = new double();
            double sum_lng = new double();
            for (int i = 0; i < points.Count; i++)
            {
                PointLatLng point = points[i];
                sum_lat += point.Lat;
                sum_lng += point.Lng;
            }
            sum_lat /= points.Count;
            sum_lng /= points.Count;
            return new PointLatLng(sum_lat, sum_lng);
        }
        public override double getDistance(PointLatLng point)
        {
            PointLatLng area_center = this.getCenter();
            GeoCoordinate c1 = new GeoCoordinate(area_center.Lat, area_center.Lng);
            GeoCoordinate c2 = new GeoCoordinate(point.Lat, point.Lng);
            double distance = c1.GetDistanceTo(c2);
            return distance;
        }
        public override void getFocus(MainWindow mainWindow)
        {
            mainWindow.Map.Position = this.getCenter();
        }
        public void getMarker(MainWindow mainWindow)
        {
            List<PointLatLng> temp_list = new List<PointLatLng>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                temp_list.Add(points[i]);
                temp_list.Add(points[i + 1]);
                GMapMarker marker = new GMapPolygon(temp_list)
                {
                    Shape = new Path
                    {
                        Stroke = Brushes.DarkBlue, // стиль обводки
                        Fill = Brushes.DarkBlue, // стиль заливки
                        Opacity = 4, // прозрачность
                        ToolTip = this.getTitle()
                    }
                };
                mainWindow.Map.Markers.Add(marker);
                markers.Add(marker);
                temp_list.Clear();
            }

        }
    }
    class Human : MapObject
    {
        PointLatLng point;
        GMapMarker marker;
        public Human(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }
        public GMapMarker returnMarker()
        {
            return marker;
        }
        public PointLatLng getPoint()
        {
            return point;
        }
        public override double getDistance(PointLatLng point)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(this.point.Lat, this.point.Lng);
            double distance = c1.GetDistanceTo(c2);
            return distance;
        }
        public override void getFocus(MainWindow mainWindow)
        {
            mainWindow.Map.Position = point;
        }
        public void getMarker(MainWindow mainWindow)
        {
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера
                    ToolTip = this.getTitle(), // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/man.png")) // картинка
                }
            };
            this.marker = marker;
            mainWindow.Map.Markers.Add(marker);
        }
        /*public void moveTo(PointLatLng point) { 
            
        }
        public void carArrived(object sender, EventArgs e) { 
            
        }*/
    }
    class Location : MapObject
    {
        PointLatLng point;
        public Location(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }
        public override double getDistance(PointLatLng point)
        {
            GeoCoordinate c1 = new GeoCoordinate(this.point.Lat, this.point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(point.Lat, point.Lng);
            double distance = c1.GetDistanceTo(c2);
            return distance;
        }
        public override void getFocus(MainWindow mainWindow)
        {
            mainWindow.Map.Position = point;
        }
        public void getMarker(MainWindow mainWindow)
        {
            Trace.WriteLine("used");
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера
                    ToolTip = this.getTitle(), // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/loc.png")) // картинка
                }
            };
            mainWindow.Map.Markers.Add(marker);

        }
    }
}
