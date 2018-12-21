using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MapTool
{
    public class Configurations
    {
        public static IEnumerable<MenuItem> _MenuItems = new List<MenuItem>
        {
            new MenuItem()
            {
                Header =MapType.界河.ToString(),
                Type = MapType.界河,
                SubMenuItems = GetMenuItems(@"Assets\Borders",MapType.界河)
            },
            new MenuItem()
            {
                Header = MapType.湖泊.ToString(),
                Type = MapType.湖泊,
                SubMenuItems = GetMenuItems(@"Assets\Lakes",MapType.湖泊)
            },
            new MenuItem()
            {
                Header =  MapType.岛屿.ToString(),
                Type = MapType.岛屿,
                SubMenuItems = GetMenuItems(@"Assets\Islands", MapType.岛屿)
            },
            new MenuItem()
            {
                Header =  MapType.南海九段线.ToString(),
                Type = MapType.南海九段线,
                IsActive =true,
                SubMenuItems = GetMenuItems(@"Assets\NighSegmentLines",null)
            },
            new MenuItem()
            {
                Header =  MapType.边界线.ToString(),
                Type = MapType.边界线,
                IsActive =true,
                SubMenuItems = GetMenuItems(@"Assets\BorderLines",null),
            },
            new MenuItem()
            {
                Header =  MapType.中国边界线.ToString(),
                Type = MapType.中国边界线,
                IsActive =true,
                SubMenuItems = GetMenuItems(Path.Combine(AppContext.BaseDirectory, @"Assets\ChinaBoundaryLines.txt")),
            },
            new MenuItem()
            {
                Header =  MapType.LoadAll.ToString(),
                Type = MapType.LoadAll,
                IsActive =true
            },
            new MenuItem()
            {
                Header =  MapType.ClearAll.ToString(),
                Type = MapType.ClearAll,
                IsActive =true
            }
        };

        public static List<MenuItem> GetMenuItems(string dirPath, MapType? type)
        {
            List<MenuItem> results = new List<MenuItem>();

            var files = GetFilesByDictionary(dirPath);

            files.Where(x => x.FileType == ".txt").ToList().ForEach(item =>
            {
                results.Add(new MenuItem()
                {
                    Header = item.DisplayName,
                    Type = type,
                    MapGeoPoints = GetLocationCollection(item.Path)
                });
            });

            return results;
        }
        public static IReadOnlyList<StorageFile> GetFilesByDictionary(string dirPath)
        {
            //StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //StorageFolder assets = appInstalledFolder.GetFolderAsync("Assets").GetAwaiter().GetResult();
            //StorageFolder borders = assets.GetFolderAsync("Borders").GetAwaiter().GetResult();

            string path = Path.Combine(AppContext.BaseDirectory, dirPath);
            StorageFolder folder = StorageFolder.GetFolderFromPathAsync(path).GetAwaiter().GetResult();
            var files = folder.GetFilesAsync().GetAwaiter().GetResult();

            return files;
        }
        public static async Task<string[]> GetTextContent(string folderpath)
        {

            //var uri = new Uri(@"ms-appx:///Assets/Borders/beilunhe.tex");
            var uri = new Uri(@"ms-appx:///Assets/Borders/beilunheTExt.txt");
            string[] contents;

            StorageFile item = await StorageFile.GetFileFromApplicationUriAsync(uri);

            using (Stream file = await item.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(file))
                {
                    var s = read.ReadToEnd();
                    Regex reg = new Regex("[MapLocation=(.+)],");
                    //string modified = reg.Replace(line, "NAME=WANG;");
                    contents = Regex.Split(s, @"[MapLocation=(.+)],", RegexOptions.IgnorePatternWhitespace);
                }
            }
            var fileContent = "[MapLocation (21.655444048310898, 107.80337953195655)],[MapLocation (21.657852167906057, 107.80464553461158)],[MapLocation (21.658839335752873, 107.80586862192237)],[MapLocation (21.658515208339352, 107.80823299387467)],[MapLocation (21.657711962761553, 107.80915767885493)],[MapLocation (21.654067904020224, 107.81402656534684)],[MapLocation (21.653808639899182, 107.81514236429703)],[MapLocation (21.65386847012229, 107.81606504419815)],[MapLocation (21.65452106789904, 107.8180840704565)],[MapLocation (21.654406941014173, 107.82012054422867)],[MapLocation (21.6530851421915, 107.8228047583227)],[MapLocation (21.650657546429002, 107.82600876978408)],[MapLocation (21.6504182200634, 107.8291630475856)],[MapLocation (21.64993956614171, 107.8304075925685)],[MapLocation (21.648324097437907, 107.8330898015834)],[MapLocation (21.64675581265297, 107.83458397052229)],[MapLocation (21.641590124183793, 107.83786699435652)],[MapLocation (21.640572920506855, 107.83962652347029)],[MapLocation (21.640533030020578, 107.84155771396101)],[MapLocation (21.640313632149116, 107.84254476687849)],[MapLocation (21.638718001239056, 107.84456178805769)],[MapLocation (21.637980015981015, 107.8468148436302)],[MapLocation (21.638358981854807, 107.84801647326887)],[MapLocation (21.639635491166416, 107.84953996798933)],[MapLocation (21.641949135535555, 107.85014078280867)],[MapLocation (21.643445006560327, 107.85009786746443)],[MapLocation (21.644442245300937, 107.85044119021833)],[MapLocation (21.645878256987118, 107.85172865054548)],[MapLocation (21.647201557729304, 107.85558838085733)],[MapLocation (21.648916759468747, 107.85827058987222)],[MapLocation (21.650013680129263, 107.85945076183877)],[MapLocation (21.65109064866796, 107.86063093380533)],[MapLocation (21.651629129923904, 107.86187547878824)],[MapLocation (21.651529411324375, 107.86342043118081)],[MapLocation (21.650340756767292, 107.86468643383584)],[MapLocation (21.649782326738006, 107.8646435184916)],[MapLocation (21.648409408820058, 107.86527606562149)],[MapLocation (21.647103502002636, 107.86624166086685)],[MapLocation (21.64645486811601, 107.86830159739029)],[MapLocation (21.646554590221115, 107.87152024820816)],[MapLocation (21.64553742151709, 107.87355872705947)],[MapLocation (21.644260964376926, 107.87467452600967)],[MapLocation (21.641069772157252, 107.87561866358291)],[MapLocation (21.63939436801097, 107.87697049692642)],[MapLocation (21.63737758927354, 107.8809262697751)],[MapLocation (21.63614095171664, 107.88232101846285)],[MapLocation (21.634285975530418, 107.88242830682344)],[MapLocation (21.63297735171642, 107.88231753447407)],[MapLocation (21.628131092847184, 107.88268011557821)],[MapLocation (21.627901702644976, 107.88280886161093)],[MapLocation (21.627694752476174, 107.88315754878286)],[MapLocation (21.627200741285463, 107.88451551255645)],[MapLocation (21.626802604299627, 107.88560359820487)],[MapLocation (21.62623938493634, 107.88678882425765)],[MapLocation (21.625179685021294, 107.88906333750228)],[MapLocation (21.623772248208255, 107.89062831624803)],[MapLocation (21.622132463875374, 107.89146139711353)],[MapLocation (21.620317203934192, 107.89139702409717)],[MapLocation (21.61766409071008, 107.89047434419605)],[MapLocation (21.615734050579036, 107.89055483120711)],[MapLocation (21.614676767254196, 107.89134876507552)],[MapLocation (21.613021006137686, 107.89332287091048)],[MapLocation (21.611684414058857, 107.89321558254989)],[MapLocation (21.60911094079202, 107.88900987881453)],[MapLocation (21.608173307368773, 107.88821594494613)],[MapLocation (21.607175818331598, 107.88767950314315)],[MapLocation (21.606298022289266, 107.88755075711043)],[MapLocation (21.605659621822127, 107.88757221478255)],[MapLocation (21.604702015839337, 107.8880442835692)],[MapLocation (21.604063608329497, 107.88883821743761)],[MapLocation (21.60324564459077, 107.89180633583271)],[MapLocation (21.60324564459077, 107.89343711891377)],[MapLocation (21.60278678485902, 107.89532539406025)],[MapLocation (21.6020286655923, 107.89629098930561)],[MapLocation (21.600711922688603, 107.89682743110863)],[MapLocation (21.599075952693426, 107.89684888878075)],[MapLocation (21.595903714275167, 107.89547559776513)]";
            string pattern = @"\[MapLocation|\]\,";

            contents = Regex.Split(fileContent, pattern, RegexOptions.IgnorePatternWhitespace).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            //using (IRandomAccessStream readStream = await item.OpenAsync(FileAccessMode.Read))
            //{
            //    using (DataReader dataReader = new DataReader(readStream))
            //    {
            //        uint numBytesLoaded = await dataReader.LoadAsync((uint)readStream.Size);
            //        string fileContent = dataReader.ReadString(numBytesLoaded);
            //        Regex reg = new Regex("[MapLocation=(.+)],");
            //        contents = Regex.Split(fileContent, @"[MapLocation=(.+)],", RegexOptions.IgnorePatternWhitespace);
            //    }
            //}
            return contents;
            //using (IRandomAccessStream readStream = await item.OpenAsync(FileAccessMode.Read))
            //{
            //    using (DataReader dataReader = new DataReader(readStream))
            //    {
            //        UInt64 size = readStream.Size;
            //        if (size <= UInt32.MaxValue)
            //        {
            //            UInt32 numBytesLoaded = await dataReader.LoadAsync((UInt32)size);
            //            string fileContent = dataReader.ReadString(numBytesLoaded);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 根据文件路径读取坐标点集合
        /// </summary>
        /// <param name="filePath">tex文件路径</param>
        /// <returns></returns>
        public static List<BasicGeoposition> GetLocationCollection(string filePath)
        {
            var result = new List<BasicGeoposition>();

            using (FileStream file = File.OpenRead(filePath))
            {
                string pattern1 = @"[^\d.\d]"; // 正则表达式剔除非数字字符（不包含小数点.）

                {
                    string pattern = @"\[MapLocation|\]\,";
                    using (StreamReader read = new StreamReader(file))
                    {
                        var s = read.ReadToEnd();

                        var contents = Regex.Split(s, pattern, RegexOptions.IgnorePatternWhitespace);

                        contents.Where(x => !String.IsNullOrEmpty(x)).ToList().ForEach(item =>
                        {
                            var geos = Regex.Split(item, pattern1).Where(m => !String.IsNullOrEmpty(m)).Select(m => double.Parse(m)).ToArray();

                            if (2 == geos.Length)
                                result.Add(new BasicGeoposition() { Latitude = geos[0], Longitude = geos[1] });
                        });
                    }
                }
            }

            return result;
        }
        
        public static List<MenuItem> GetMenuItems(string dirPath)
        {
            List<MenuItem> results = new List<MenuItem>();

            using (StreamReader read = new StreamReader(dirPath))
            {
                var s = read.ReadToEnd();
                var jo = JsonConvert.DeserializeObject<JObject>(s);
                var list = jo["features"].ToList().Select(x => x["geometry"]["coordinates"][0]).ToList();

                list.ForEach(item =>
                {
                    var tempMenu = new MenuItem()
                    {
                        Header = MapType.中国边界线.ToString(),
                        Type = MapType.中国边界线,
                        MapGeoPoints = new List<BasicGeoposition>()
                    };
                    tempMenu.MapGeoPoints.AddRange(item.Select(x => new BasicGeoposition() { Latitude = double.Parse(x[1].ToString()), Longitude = double.Parse(x[0].ToString()) }).ToList());
                    results.Add(tempMenu);
                });
            }
            return results;
        }
    }


    public class MenuItem
    {
        public string Header { get; set; }

        public MapType? Type { get; set; }

        public bool IsActive { get; set; } = false;

        public List<BasicGeoposition> MapGeoPoints { get; set; }


        public List<MenuItem> SubMenuItems { get; set; }

        //private async Task<List<MenuItem>> GetAllFiles(string directionaryPath)
        //{
        //    StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        //    StorageFolder assets = await appInstalledFolder.GetFolderAsync("Assets");
        //    var list = await assets.CreateFolderAsync("Borders",CreationCollisionOption.OpenIfExists);
        //   // var Borders = await assets.GetFolderAsync("Borders");
        //    var BorderFiles = await assets.GetFilesAsync();
        //    List<MenuItem> menus = new List<MenuItem>();
        //    //StorageFolder mapFolder = await StorageFolder.GetFolderFromPathAsync(item11);
        //    //IReadOnlyList<StorageFile> fileList = await mapFolder.GetFilesAsync();
        //    var files = BorderFiles.Where(x => x.FileType == "*.tex").ToList(); 
        //    files.ForEach(async item => {
        //        using (StreamReader sr = new StreamReader(item.FileType, Encoding.Default))
        //        {
        //            // var ww =MapLocation(47.97199433897777, 117.73344489695654);
        //            var texts = await sr.ReadToEndAsync();
        //            var ss = texts.Replace("MapLocation", "");

        //            //string line = "ADDR=1234;NAME=ZHANG;PHONE=6789";
        //            Regex reg = new Regex("[MapLocation=(.+)],");
        //            //string modified = reg.Replace(line, "NAME=WANG;");
        //            var contents = Regex.Split(ss, @"[MapLocation=(.+)],", RegexOptions.IgnorePatternWhitespace);
        //            Console.WriteLine(contents.Length);
        //        }
        //    });
        //    return menus;

        //}
    }

    public enum MapType
    {
        界河 = 1,
        湖泊 = 2,
        岛屿 = 3,
        南海九段线 = 4,
        边界线 = 5,
        中国边界线 = 6,
        LoadAll = 7,
        ClearAll = 8,

    }
}
