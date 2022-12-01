using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using Interface_Weather.Model;

namespace Interface_Weather
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            DateTime DateNull = new DateTime(1970, 1, 1);
            string url = "https://api.openweathermap.org/data/2.5/onecall?lat=58.0174&lon=56.2855&exclude=hourly,minutely&units=metric&appid=91df9e1c13ae5ab6bd45b1fe53d8f28f";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            BitmapImage Pict;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            Weather weather = JsonConvert.DeserializeObject<Weather>(response);

            DataContext = weather;

            foreach (Day a in weather.daily)
            {
                Grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                if (weather.daily.IndexOf(a) <= 4)
                {
                    Grid1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
                }
                else if (weather.daily.IndexOf(a) == 5)
                {
                    Grid1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });
                }

                TextBlock textBlockdt = new TextBlock { Text = DateNull.AddSeconds(a.dt).ToShortDateString(), TextAlignment = TextAlignment.Center };
                Grid.SetColumn(textBlockdt, weather.daily.IndexOf(a));
                Grid.SetRow(textBlockdt, 0);
                Grid1.Children.Add(textBlockdt);

                TextBlock textBlockDOW = new TextBlock { Text = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(DateNull.AddSeconds(a.dt).DayOfWeek).ToString(), TextAlignment = TextAlignment.Center };
                Grid.SetColumn(textBlockDOW, weather.daily.IndexOf(a));
                Grid.SetRow(textBlockDOW, 1);
                Grid1.Children.Add(textBlockDOW);

                TextBlock textBlockMorn = new TextBlock { Text = Convert.ToString(a.temp.morn), TextAlignment = TextAlignment.Center };
                Grid.SetColumn(textBlockMorn, weather.daily.IndexOf(a));
                Grid.SetRow(textBlockMorn, 2);
                Grid1.Children.Add(textBlockMorn);

                TextBlock textBlockDay = new TextBlock { Text = Convert.ToString(a.temp.day), TextAlignment = TextAlignment.Center };
                Grid.SetColumn(textBlockDay, weather.daily.IndexOf(a));
                Grid.SetRow(textBlockDay, 3);
                Grid1.Children.Add(textBlockDay);

                TextBlock textBlockEve = new TextBlock { Text = Convert.ToString(a.temp.eve), TextAlignment = TextAlignment.Center };
                Grid.SetColumn(textBlockEve, weather.daily.IndexOf(a));
                Grid.SetRow(textBlockEve, 4);
                Grid1.Children.Add(textBlockEve);

                TextBlock textBlockNight = new TextBlock { Text = Convert.ToString(a.temp.night), TextAlignment = TextAlignment.Center };
                Grid.SetColumn(textBlockNight, weather.daily.IndexOf(a));
                Grid.SetRow(textBlockNight, 5);
                Grid1.Children.Add(textBlockNight);
            }

            foreach (Day a in weather.daily)
            {
                //Grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                Grid1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                Pict = Picture(a.weather[0].id);
                Image BlockPict = new Image { Source = Pict, Width = Pict.DpiX, Height = Pict.DpiY };
                Grid.SetColumn(BlockPict, weather.daily.IndexOf(a));
                Grid.SetRow(BlockPict, 6);
                Grid1.Children.Add(BlockPict);
            }


        }
        public static BitmapImage Picture(int Id)
        {

            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Picture;Trusted_Connection=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    "Select w.folderPath From [dbo].[Weather] as w Inner join [dbo].[fallout] as f on w.id = f.id_pict Where f.id = @id", connection);

                command.Parameters.Add("@id", SqlDbType.Int);

                connection.Open();

                command.Parameters["@id"].Value = Id;

                byte[] img = (byte[])command.ExecuteScalar();
                BitmapImage BitImg = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(img))
                {
                    BitImg.BeginInit();
                    BitImg.CacheOption = BitmapCacheOption.OnLoad;
                    BitImg.StreamSource = stream;
                    BitImg.EndInit();
                    BitImg.Freeze();
                }
                return BitImg;
            }
        }

        
    }

}
