using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerAndAccountInformationApp
{
    public partial class CustomerAndAccountInformationUI : Form
    {
        public CustomerAndAccountInformationUI()
        {
            InitializeComponent();

         
        }


        private string connectionString = ConfigurationManager.ConnectionStrings["BankAppConString"].ConnectionString;

        private void saveAccountButton_Click(object sender, EventArgs e)
        {

            Account account=new Account();

            account.name = customerNameTextBox.Text;
            account.email = emailTextBox.Text;
            account.accountNumber = accountNumberTextBox.Text;
            account.openingDate = openingDateTextBox.Text;
            account.balance = 0;

            if (account.name == "" || account.email == "" || account.accountNumber == "" || account.openingDate == "")
            {
                MessageBox.Show("Fill out All the TextBoxes");
            }

            else if (IsEmailExists(account.email))
            {
                MessageBox.Show("Change your Email Id this already Exists");
            }

            else if (IsAccountNumberExists(account.accountNumber))
            {
                MessageBox.Show("This Account Number already Exists");
                
            }
            else if (account.accountNumber.Length < 8)
            {
                MessageBox.Show("Account Number must be greater than 7 Character");
            }

            else
            {
                SqlConnection connection = new SqlConnection(connectionString);

                string query = "INSERT INTO customer VALUES('"+account.name+"','"+account.email+"','"+account.accountNumber+"','"+account.openingDate+"','"+account.balance+"')";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
               int rowsAffected= command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {

                    accountInfoListView.Items.Clear();
                    MessageBox.Show("Customer Information Saved Successfully");
                    GetTextBoxesClear();
                    GetDataInListView();
                    
                }
                else
                {
                    MessageBox.Show("Operation Failed");
                }
            }
        }

        private void depositButton_Click(object sender, EventArgs e)
        {

            Account account=new Account();

            account.accountNumber = accountNumberBox.Text;
            account.balance = double.Parse(amountTextBox.Text);


            float balance = GetBalance(account.accountNumber);


            if (account.accountNumber == "" || amountTextBox.Text == "")
            {
                MessageBox.Show("Fill out the boxes to perform operation");
            }

            else if (!IsAccountNumberExists(account.accountNumber))
            {
                MessageBox.Show("This account number doesn't exist in your bank");
            }

            else
            {


                if (account.balance <= 0)
                {
                    MessageBox.Show("You can't deposit zero or less than zero amount");
                }

                else
                {
                    accountInfoListView.Items.Clear();


                    float result = balance + (float) account.balance;


                    SqlConnection connection = new SqlConnection(connectionString);

                    string query = "UPDATE  customer SET balance='" + result + "'WHERE cu_accountno='" +
                                   account.accountNumber + "'";

                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Deposit Successfully");
                        accountNumberBox.Text = "";
                        amountTextBox.Text = "";
                        GetDataInListView();
                    }
                    else
                    {
                        MessageBox.Show("Operation Failed");
                    }

                }
            }

        }

        private void withdrawButton_Click(object sender, EventArgs e)
        {



            Account account = new Account();

            account.accountNumber = accountNumberBox.Text;
            account.balance = double.Parse(amountTextBox.Text);


            if (account.accountNumber == "" || amountTextBox.Text == "")
            {
                MessageBox.Show("Fill out the boxes to perform operation");
            }

            else if (!IsAccountNumberExists(account.accountNumber))
            {
                MessageBox.Show("This account number doesn't exist in your bank");
            }

            else
            {


                float balance = GetBalance(account.accountNumber);


                if (account.balance <= 0)
                {
                    MessageBox.Show("You can't withdraw zero or less than zero amount");
                }

                else if (balance < account.balance)
                {
                    MessageBox.Show("U don't have enough amount to perform this operation");
                }

                else
                {
                    accountInfoListView.Items.Clear();


                    float result = balance - (float) account.balance;

                    SqlConnection connection = new SqlConnection(connectionString);

                    string query = "UPDATE  customer SET balance='" + result + "'WHERE cu_accountno='" +
                                   account.accountNumber + "'";

                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("WithDraw Money Successfully");
                        accountNumberBox.Text = "";
                        amountTextBox.Text = "";
                        GetDataInListView();
                    }
                    else
                    {
                        MessageBox.Show("Operation Failed");
                    }

                }
            }
        }

        private void searchAccountNumberButton_Click(object sender, EventArgs e)
        {

            Account account=new Account();
          
            List<Account> accounts = new List<Account>();

            account.accountNumber = accountNumberTextBox1.Text;

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT cu_accountno,cu_name,cu_openingDate,balance FROM customer WHERE (cu_accountno LIKE'" + account.accountNumber +"%')";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Account account1 = new Account();

                account1.accountNumber = reader["cu_accountno"].ToString();

                account1.name = reader["cu_name"].ToString();

                account1.openingDate = reader["cu_openingDate"].ToString();

                account1.balance = float.Parse(reader["balance"].ToString());

                accounts.Add(account1);
            }

            reader.Close();
            connection.Close();

            accountInfoListView.Items.Clear();

            foreach (var account1 in accounts)
            {
                ListViewItem item = new ListViewItem();

                item.Text = account1.accountNumber;
                item.SubItems.Add(account1.name);
                item.SubItems.Add(account1.openingDate);
                item.SubItems.Add(account1.balance.ToString());

                accountInfoListView.Items.Add(item);
            }

        }

        private void GetTextBoxesClear()
        {
            customerNameTextBox.Text = string.Empty;
            emailTextBox.Text = string.Empty;
            accountNumberTextBox.Text = string.Empty;
            openingDateTextBox.Text = string.Empty;
        }


        private bool IsEmailExists(string email)
        {
            bool emailexists = false;

            SqlConnection connection=new SqlConnection(connectionString);

            string query="SELECT cu_email FROM customer WHERE cu_email='"+email+"'";
            
            SqlCommand command=new SqlCommand(query ,connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                emailexists = true;
            }

            reader.Close();
            connection.Close();

            return emailexists;
        }

        private bool IsAccountNumberExists(string accountno)
        {
            bool accountexists = false;

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT cu_accountno FROM customer WHERE cu_accountno='" + accountno + "'";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                accountexists = true;
            }

            reader.Close();
            connection.Close();

            return accountexists;
            
        }


        private void GetDataInListView()
        {
            List< Account >accounts=new List<Account>();

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT cu_accountno,cu_name,cu_openingDate,balance FROM customer";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
               Account account =new Account();

                account.accountNumber = reader["cu_accountno"].ToString();
                
                account.name = reader["cu_name"].ToString();

                account.openingDate = reader["cu_openingDate"].ToString();

                account.balance = float.Parse(reader["balance"].ToString());

                accounts.Add(account);

            }

            reader.Close();
            connection.Close();


            foreach (var account in accounts)
            {
                ListViewItem item=new ListViewItem();

                item.Text = account.accountNumber;
                item.SubItems.Add(account.name);
                item.SubItems.Add(account.openingDate);
                item.SubItems.Add(account.balance.ToString());

                accountInfoListView.Items.Add(item);
            }

        }

        private void CustomerAndAccountInformationUI_Load(object sender, EventArgs e)
        {
            accountInfoListView.Items.Clear();
            GetDataInListView();
        }


        private float  GetBalance(string accountNo)
        {
            float balance = 0;
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT balance FROM customer WHERE cu_accountno='" + accountNo + "'";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                balance = float.Parse(reader["balance"].ToString());
            }

            reader.Close();
            connection.Close();

            return balance;

        }

    }
}
