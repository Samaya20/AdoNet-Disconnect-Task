using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace DisconnectTask
{
    public partial class MainWindow : Window
    {

        private string conStr = "Data Source=DESKTOP-0781CLG\\SQLEXPRESS;" +
            "Initial Catalog=Library;" +
            "Integrated Security=True;" +
            "Connect Timeout=30;" +
            "Trust Server Certificate=True;";
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;


        public MainWindow()
        {
            InitializeComponent();
            dataAdapter = new SqlDataAdapter();
            dataTable = new DataTable();

            LoadAuthors();
        }

        private void LoadAuthors()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conStr);

                sqlConnection.Open();

                string selectQuery = "SELECT * FROM Authors";
                dataAdapter.SelectCommand = new SqlCommand(selectQuery, sqlConnection);

                dataTable.Clear();
                dataAdapter.Fill(dataTable);


                dataGridView.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(conStr);

                sqlConnection.Open();

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                dataAdapter.Update(dataTable);


                MessageBox.Show("Updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchName = searchTextBox.Text.Trim();

                var filteredAuthors = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("FirstName").StartsWith(searchName)) // bu hissəni contains ilə də yazmaq olardı
                    .CopyToDataTable();

                dataGridView.ItemsSource = filteredAuthors.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
