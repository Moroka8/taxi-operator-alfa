using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using System.Diagnostics;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PointLatLng> points = new List<PointLatLng>();
        PointLatLng last_point = new PointLatLng();
        List<MapObject> all_objects = new List<MapObject>();
        List<Human> all_humans = new List<Human>();
        List<Car> all_cars = new List<Car>();
        public MainWindow()
        {
            InitializeComponent();
        }

        int points_counter = 0;
       /* public void getSoloMarker(PointLatLng point)
        {
            Trace.WriteLine("used");
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 32, // ширина маркера
                    Height = 32, // высота маркера
                                 //ToolTip = this.getTitle(), // всплывающая подсказка
                    Source = new BitmapImage(new Uri("pack://application:,,,/loc.png")) // картинка
                }
            };
            Map.Markers.Add(marker);
            Trace.WriteLine(Map.Markers[0]);

        }
        public void getManyMarker(List<PointLatLng> points)
        {
            GMapMarker marker = new GMapPolygon(points)
            {
                Shape = new Path
                {
                    Stroke = Brushes.Black, // стиль обводки
                    Fill = Brushes.Violet, // стиль заливки
                    Opacity = 0.7, // прозрачность
                    //ToolTip = this.getTitle()
                }
            };
            Map.Markers.Add(marker);
        }*/

            private void MapLoaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            // установка фокуса карты
            Map.Position = new PointLatLng(55.012823, 82.950359);
            BingMapProvider.Instance.ClientKey = "Arrj4jXJapQ78MYKH-8_1PVs50ArzkNkQkQ_PRDvqN3WqZh2iHOY1UykbD_5u08Q";

            Map.MapProvider = BingMapProvider.Instance;

           MapRoute route = GMap.NET.MapProviders.BingMapProvider.Instance.GetRoute(
             new PointLatLng(55.016215, 82.948772), // начальная точка маршрута
             new PointLatLng(55.016667, 82.949546), // конечная точка маршрута
             false, // поиск по шоссе (false - включен)
             false, // режим пешехода (false - выключен)
             15);
            // получение точек маршрута
            List<PointLatLng> routePoints = route.Points;
            Route route1 = new Route("route", routePoints);
            route1.getMarker(App.Current.MainWindow as MainWindow);

            // настройка взаимодействия с картой
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;
        }
        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (create.IsChecked == true)
            {
                PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                last_point = point;
                points.Add(point);
                points_counter++;
                Location dot = new Location(String.Format("place {0}", points_counter + 1), point);
                dot.getMarker(App.Current.MainWindow as MainWindow);
            }
            else if (near.IsChecked == true) {
                try
                {
                    PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
                    List<double> temp_range = new List<double>();
                    for (int i = 0; i < all_objects.Count; i++)
                    {
                        temp_range.Add(all_objects[i].getDistance(point));
                    }
                    int index_min = temp_range.IndexOf(temp_range.Min());
                    all_objects[index_min].getFocus(App.Current.MainWindow as MainWindow);
                }
                catch(System.InvalidOperationException){
                
                }


            }
            //this.getSoloMarker(point);
            /*if (points_counter == 3) {
                for (int i=0; i < points_counter; i++)
                {
                    Map.Markers.RemoveAt(Map.Markers.Count-1);
                }
                points_counter = 0;
                Area area = new Area("place", points);
                area.getMarker(App.Current.MainWindow as MainWindow);
                //this.getManyMarker(points);
            }*/

        }

        private void Add_but_Click(object sender, RoutedEventArgs e)
        {
            if (obj_list.SelectedItem == obj_area)
            {
                for (int i = 0; i < points_counter; i++)
                {
                    Map.Markers.RemoveAt(Map.Markers.Count - 1);
                }
                points_counter = 0;
                Area area = new Area(name_box.Text, points);
                all_objects.Add(area);
                area.getPoints();
                area.getMarker(App.Current.MainWindow as MainWindow);
                area.getPoints();
                points.Clear();
                area.getPoints();
            }
            else if (obj_list.SelectedItem == obj_human)
            {
                for (int i = 0; i < points_counter; i++)
                {
                    Map.Markers.RemoveAt(Map.Markers.Count - 1);
                }
                points_counter = 0;
                Human human = new Human(name_box.Text, last_point);
                all_objects.Add(human);
                all_humans.Add(human);
                hum_list.Items.Add(human.getTitle());
                human.getMarker(App.Current.MainWindow as MainWindow);
                points.Clear();

            }
            else if (obj_list.SelectedItem == obj_car) {
                for (int i = 0; i < points_counter; i++)
                {
                    Map.Markers.RemoveAt(Map.Markers.Count - 1);
                }
                points_counter = 0;
                Car car = new Car(name_box.Text, last_point);
                all_objects.Add(car);
                all_cars.Add(car);
                car.getMarker(App.Current.MainWindow as MainWindow);
                points.Clear();
            }
            if (obj_list.SelectedItem == obj_route)
            {
                for (int i = 0; i < points_counter; i++)
                {
                    Map.Markers.RemoveAt(Map.Markers.Count - 1);
                }
                points_counter = 0;
                Route route = new Route(name_box.Text, points);
                all_objects.Add(route);
                route.getMarker(App.Current.MainWindow as MainWindow);
                points.Clear();
            }
        }

        private void Clear_but_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < points_counter; i++)
            {
                Map.Markers.RemoveAt(Map.Markers.Count - 1);
            }
            points_counter = 0;
            points.Clear();
        }

        private void find_but_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (find_box.Text != "")
                {
                    List<string> temp_titles = new List<string>();
                    for (int i = 0; i < all_objects.Count; i++)
                    {
                        temp_titles.Add(all_objects[i].getTitle());
                    }
                    all_objects[temp_titles.IndexOf(find_box.Text)].getFocus(App.Current.MainWindow as MainWindow);
                }
            }
            catch (System.ArgumentOutOfRangeException) {
            
            }
        }

        private void order_but_Click(object sender, RoutedEventArgs e)
        {
            PointLatLng point = last_point;
            int index = hum_list.SelectedIndex;
            PointLatLng hum_pos = all_humans[index].getPoint();
            List<double> temp_range = new List<double>();
            for (int i = 0; i < all_cars.Count; i++)
            {
                temp_range.Add(all_cars[i].getDistance(hum_pos));
            }
            int index_min = temp_range.IndexOf(temp_range.Min());
            Car selected_car = all_cars[index_min];
            selected_car.getFocus(App.Current.MainWindow as MainWindow);
            selected_car.moveTo(all_humans[index],hum_pos, point, App.Current.MainWindow as MainWindow);
        }
    }
}
