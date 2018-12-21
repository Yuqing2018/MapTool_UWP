using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MapTool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private const string token = "AjFrDDOWc6zU67mJ9vcnh7Qbai9Htic0JQuGJR8K8jGreFFqCmi_Cj2IA-5camLo";
        public static readonly Geopoint ChinaGeopoint = new Geopoint(new BasicGeoposition() { Latitude = 35.3653324, Longitude = 102.5418386 });

        public List<MenuItem> MenuItems { get; set; }
        public MainPage()
        {
           // MapService.ServiceToken = token;
            this.InitializeComponent();
            MenuItems = Configurations._MenuItems.ToList();
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            myMap.MapElements.Clear();

            var context = (sender as Control).DataContext as MenuItem;
            if (MapType.ClearAll == context.Type)
                return;
            if (MapType.LoadAll == context.Type)
            {
                MenuItems.ForEach(async item =>
                    {
                        if (item.IsActive)
                            await AddMapElements(item, false);
                        else
                            item.SubMenuItems.ForEach(async subitem =>
                            {
                                await AddMapElements(subitem, false);
                            });
                    });
                myMap.ZoomLevel = 4;
            }
            else
                await AddMapElements(context, true);
        }

        private async Task AddMapElements(MenuItem menuItem, bool resetCenter = true)
        {
            switch (menuItem.Type)
            {
                case MapType.界河:
                    await LoadPolyline(menuItem.MapGeoPoints, Colors.Blue, resetCenter);
                    myMap.ZoomLevel = 9;
                    break;
                case MapType.湖泊:
                    await LoadPolyline(menuItem.MapGeoPoints, Colors.Red, resetCenter);
                    myMap.ZoomLevel = 10;
                    break;
                case MapType.岛屿:
                    await LoadPolygon(menuItem.MapGeoPoints, Colors.Green, resetCenter);
                    myMap.ZoomLevel = 10;
                    break;
                case MapType.南海九段线:
                    var boundaries = menuItem.SubMenuItems.Select(x => x.MapGeoPoints).ToList();

                    boundaries.ForEach(async item =>
                    {
                        await LoadPolyline(item, Colors.Red, false);
                    });

                    if (resetCenter)
                    {
                        double centerLatitude = boundaries.Average(x => x.Select(l => l.Latitude).FirstOrDefault());
                        double centerLongitude = boundaries.Average(x => x.Select(l => l.Longitude).FirstOrDefault());
                        var centerPoint = new Geopoint(new BasicGeoposition() { Latitude = centerLatitude, Longitude = centerLongitude });
                        var flag = await myMap.TrySetViewAsync(centerPoint);
                        myMap.ZoomLevel = 6;
                    }
                    break;
                case MapType.边界线:
                    var boundaries2 = menuItem.SubMenuItems.Select(x => x.MapGeoPoints).ToList();
                    boundaries2.ForEach(async item =>
                    {
                        await LoadPolyline(item, Colors.Red, false);
                    });

                    if (resetCenter)
                    {
                        double centerLatitude = boundaries2.Average(x => x.Select(l => l.Latitude).FirstOrDefault());
                        double centerLongitude = boundaries2.Average(x => x.Select(l => l.Longitude).FirstOrDefault());
                        var centerPoint = new Geopoint(new BasicGeoposition() { Latitude = centerLatitude, Longitude = centerLongitude });
                        var flag = await myMap.TrySetViewAsync(centerPoint);
                        myMap.ZoomLevel = 4;
                    }
                    break;
                case MapType.中国边界线:
                    var boundaries1 = menuItem.SubMenuItems.Select(x => x.MapGeoPoints).ToList();

                    boundaries1.ForEach(async item =>
                    {
                        await LoadPolyline(item, Colors.Red, false);
                    });
                    myMap.ZoomLevel = 4;
                    break;

            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MyMap_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            myMap.Center = MainPage.ChinaGeopoint;
            myMap.ZoomLevel = 4;
            //myMap.DesiredPitch = 45;
        }

        public async Task LoadPolyline(List<BasicGeoposition> positions, Color color, bool ResetSetCenter = true)
        {
            MapPolyline polyline = new MapPolyline();
            polyline.Path = new Geopath(positions);
            polyline.StrokeColor = color;
            polyline.StrokeThickness = 2;
            polyline.StrokeDashed = false;
            myMap.MapElements.Add(polyline);

            if (ResetSetCenter)
            {
                double centerLatitude = positions.Average(x => x.Latitude);
                double centerLongitude = positions.Average(x => x.Longitude);

                var centerPoint = new Geopoint(new BasicGeoposition() { Latitude = centerLatitude, Longitude = centerLongitude });
                var flag = await myMap.TrySetViewAsync(centerPoint);//.GetAwaiter().GetResult();
            }

        }
        public async Task LoadPolygon(List<BasicGeoposition> positions, Color color, bool ResetSetCenter = true)
        {
            MapPolygon mapPolygon = new MapPolygon();
            mapPolygon.Path = new Geopath(positions);
            mapPolygon.ZIndex = 1;
            mapPolygon.FillColor = Colors.Transparent;
            mapPolygon.StrokeColor = color;
            mapPolygon.StrokeThickness = 2;
            mapPolygon.StrokeDashed = false;
            myMap.MapElements.Add(mapPolygon);

            if (ResetSetCenter)
            {
                double centerLatitude = positions.Average(x => x.Latitude);
                double centerLongitude = positions.Average(x => x.Longitude);
                var centerPoint = new Geopoint(new BasicGeoposition() { Latitude = centerLatitude, Longitude = centerLongitude });
                var flag = await myMap.TrySetViewAsync(centerPoint);
                //var level = GetFitableZoomLevel(positions, centerPoint);
                //myMap.ZoomLevel = level;
            }
        }

        private void LoadAll_Btn_Click(object sender, RoutedEventArgs e)
        {
            MenuItems.ForEach(async item =>
           {
               if (item.IsActive)
                   await AddMapElements(item, false);

               else // if(item.SubMenuItems?.Count() > 0)
                   item.SubMenuItems.ForEach(async subitem =>
                   {
                       await AddMapElements(subitem, false);
                   });
           });
        }

        private double GetFitableZoomLevel(List<BasicGeoposition> positions, Geopoint center)
        {
            List<Point> pointList = new List<Point>();
            positions.ForEach(item =>
            {
                Point tempPoint = new Point();
                myMap.GetOffsetFromLocation(new Geopoint(item), out tempPoint);
                pointList.Add(tempPoint);
            });
            myMap.GetOffsetFromLocation(center, out Point point);
            var minPoint = pointList.Min(p => Math.Pow(p.X, 2) + Math.Pow(p.Y, 2));
            var maxDist = pointList.Max(p => Math.Pow(p.X - point.X, 2) + Math.Pow(p.Y - point.Y, 2));

            //var maxLat = positions.Max(x => Math.Abs(x.Latitude - center.Position.Latitude));
            //var maxLot = positions.Max(x => Math.Abs(x.Longitude - center.Position.Longitude));

            //myMap.GetOffsetFromLocation(new Geopoint(new BasicGeoposition() { Latitude = center.Position.Latitude + maxLat, Longitude = center.Position.Longitude + maxLot }), out Point point1);
            

           // var t_dis = Math.Pow((point1.X - point.X), 2) + Math.Pow((point1.Y - point.Y), 2);
            var o_dis = Math.Pow(point.X, 2) + Math.Pow(point.Y, 2);

            var t_level = o_dis/ maxDist * myMap.ZoomLevel ;
            
            return t_level;
        }

    }
}