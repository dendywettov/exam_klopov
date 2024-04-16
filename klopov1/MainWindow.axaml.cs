using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace klopov1;

public partial class MainWindow : Window
{
    private MySqlConnection _connection;
    private string connectionString = "server=localhost;port=3306;database=abd;user id=root;password=root)";
    private List<users_pon> _pons;

    
    private List<filters> _filters;
    public MainWindow()
    {
        InitializeComponent();
        string sql = "SELECT * FROM stuff";
        ShowTable(sql);
        filter_user();
    }

    public class users_pon
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public int phone { get; set; }
        public DateTime year { get; set; }
        public string gender { get; set; }
        public int salle { get; set; }
    }

    public class filters
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    private void ShowTable(string sql)
    {
        _pons = new List<users_pon>();
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var current = new users_pon()
            {
                id = reader.GetInt32("id"),
                name = reader.GetString("name"),
                last_name = reader.GetString("last_name"),
                phone = reader.GetInt32("phone"),
                year = reader.GetDateTime("year"),
                gender = reader.GetString("gender"),
                salle = reader.GetInt32("salle"),
            };
            _pons.Add(current);
        }

        Grid.ItemsSource = _pons;
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {

            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string insert = "INSERT INTO stuff (name, last_name, phone, year, gender, salle) VALUES ('"+text2.Text+"', '"+text3.Text+"', '"+text4.Text+"', '"+text5.Text+"','"+text6.Text+"','"+text7.Text+"')";
            MySqlCommand command = new MySqlCommand(insert, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
            text1.Text = "Succesfully data";
        }
        catch (Exception exception)
        {
            text1.Text = "Incorrect data";
        }
    }

    private void Update_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string update = "UPDATE stuff SET name = '"+text2.Text+"', last_name = '"+text3.Text+"', phone = '"+text4.Text+"', year = '"+text5.Text+"', gender = '"+text6.Text+"', salle = '"+text7.Text+"' WHERE id = '"+text1.Text+"'";
            MySqlCommand command = new MySqlCommand(update, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
            text1.Text = "Succesfully data";
        }
        catch (Exception exception)
        {
            text1.Text = "Incorrect data";
        }
    }

    private void Delete_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
            string update = "DELETE FROM Stuff WHERE id = '"+text1.Text+"'";
            MySqlCommand command = new MySqlCommand(update, _connection);
            command.ExecuteNonQuery();
            _connection.Close();
            text1.Text = "Succesfully data";
        }
        catch (Exception exception)
        {
            text1.Text = "Incorrect data";
        }
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        _pons = new List<users_pon>();
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        string sql = "SELECT * FROM stuff";
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var current = new users_pon()
            {
                id = reader.GetInt32("id"),
                name = reader.GetString("name"),
                last_name = reader.GetString("last_name"),
                phone = reader.GetInt32("phone"),
                year = reader.GetDateTime("year"),
                gender = reader.GetString("gender"),
                salle = reader.GetInt32("salle")
            };
            _pons.Add(current);
        }

        Grid.ItemsSource = _pons;
    }

    private void filter_user()
    {
        _filters = new List<filters>();
        _connection = new MySqlConnection(connectionString);
        _connection.Open();
        string sql = "SELECT id, name FROM stuff";
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read() && reader.HasRows)
        {
            var current = new filters()
            {
                id = reader.GetInt32("id"),
                name = reader.GetString("name"),
            };
            _filters.Add(current);
        }
        var combobox = this.Find<ComboBox>("Box");
        combobox.ItemsSource = _filters;
        _connection.Close();


    }

    private void Box_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var combobox = (ComboBox)sender;
        var current = combobox.SelectedItem as filters;
        var filter = _pons.Where(x => x.name == current.name).ToList();
        Grid.ItemsSource = filter;
    }

    private void Search_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        string sql1 = "SELECT id, name, last_name, phone, year, gender, salle FROM stuff WHERE name LIKE '%"+search.Text+"%' OR last_name LIKE '%"+search.Text+"%' ";
        ShowTable(sql1);
    }

    private void A_Z_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = "SELECT id, name, last_name, phone, year, gender, salle FROM stuff ORDER BY name desc";
        ShowTable(sql);
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow _mainWindow = new MainWindow();
        this.Hide();
        _mainWindow.Show();
    }
}
