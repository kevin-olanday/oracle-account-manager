using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Management.Automation;
using Microsoft.VisualBasic.FileIO;
using System.Collections.ObjectModel;
using System.Text;

namespace OraclePasswordChanger
{
    public partial class Index : System.Web.UI.Page
    {

        protected void Generate_Password(object sender, EventArgs e)
        {
            int lengthOfPassword = 8;
            string valid = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder strB = new StringBuilder(100);
            Random random = new Random();
            while (0 < lengthOfPassword--)
            {
                strB.Append(valid[random.Next(valid.Length)]);
            }
            tb_Password.Attributes["value"] = strB.ToString();
        }

        public void Log_File(string userName, string type, string database, string user, string reason)
        {

            StreamWriter log;
            if (!File.Exists("D:\\applications\\oracleaccountmanager\\logfile.log"))
            {
                /*log = new StreamWriter("D:\\applications\\oracleaccountmanager\\logfile.log");*/
                log = new StreamWriter("D:\\applications\\oracleaccountmanager\\logfile.log");
            }
            else
            {
                log = File.AppendText("D:\\applications\\oracleaccountmanager\\logfile.log");
            }

            // Write to the file:
            log.WriteLine("Date: " + DateTime.Now);
            log.WriteLine("Database: " + database);
            log.WriteLine("Username: " + userName);
            log.WriteLine("Type: " + type);
            log.WriteLine("Performed By: " + user);
            log.WriteLine("Reason: " + reason);
            log.WriteLine(System.Environment.NewLine);
            // Close the stream:

            log.Close();

        }


        [WebMethod]
        public static string[] Get_Databases(string prefix)
        {
            List<string> databases = new List<string>();
            var path = @"D:\\applications\\oracleaccountmanager\\tnsalias.csv";

            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    string Name = fields[1];

                    if (Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                         databases.Add(Name);
                    }
                                
                }
            }
            return databases.ToArray();
        }



        public string OracleConnString(string servicename, string user, string pass)
        {

            var path = @"D:\\applications\\oracleaccountmanager\\tnsalias.csv";
            string[] host;
            string[] port;
            string service = "";
            string sid = "";
            string final = "";

            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();


                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    string name = fields[1];
                    if (name.Contains(servicename))
                    {
                        int instanceCount = Convert.ToInt32(fields[2]);
                        //System.Diagnostics.Debug.WriteLine("Instance Count: " + instanceCount);
                        host = fields[4].Split(';');
                        port = fields[5].Split(';');

                        for (int a = 0; a < instanceCount; a++)
                        {
                            //System.Diagnostics.Debug.WriteLine("a: " + a);
                            //System.Diagnostics.Debug.WriteLine("Host: " + host[a]);
                            //System.Diagnostics.Debug.WriteLine("Port: " + port[a]);
                            service = fields[6];
                            sid = fields[7];

                            if (string.IsNullOrWhiteSpace(sid))
                            {
                                String connection = String.Format(
                                  "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})" +
                                  "(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};",
                                  host[a],
                                  port[a],
                                  service,
                                  user,
                                  pass);

                                final = final + connection + "+";
                            }
                            else
                            {
                                String connection = String.Format(
                                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})" +
                                "(PORT={1}))(CONNECT_DATA=(SID={2})));User Id={3};Password={4};",
                                host[a],
                                port[a],
                                sid,
                                user,
                                pass);

                                final = final + connection + "+";
                            }
                        }
                    }
                }
            }

            return final;
        }

        protected void Reset_Password(object sender, EventArgs e)
        {
            string servicename = Request.Form["searchQuery"];
            string userName = tb_Username.Text;
            string password = tb_Password.Text;
            string reason = tb_Reason.Text;
            string initiator = Environment.UserName;
            string xaccount = "x" + initiator;
            string normalaccount = initiator.TrimStart('x');
            string type = "Reset Password";

            if ((userName.ToUpper() == normalaccount.ToUpper()) || (userName.ToUpper() == xaccount.ToUpper()))
            {
                l_Error.Text = "You are not allowed to reset your own password.";
                return;
            }

            if (string.IsNullOrWhiteSpace(servicename))
            {
                l_Error.Text = "Please select a database.";
                return;
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                l_Error.Text = "Please enter a username.";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                l_Error.Text = "Please enter a password to set.";
                return;
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                l_Error.Text = "Please enter a valid reason.";
                return;
            }


            //System.Diagnostics.Debug.WriteLine(servicename);


            string rawconnectionstring = OracleConnString(servicename, "secops_ora_pwdmgmt", "CjARd72_lMsuGqP");
            string[] connectionarray = rawconnectionstring.TrimEnd('+').Split('+');

            foreach (string connectionstring in connectionarray)
            {
                System.Diagnostics.Debug.WriteLine(connectionstring);
                using (var conn = new OracleConnection(connectionstring))
                {
                    var state = "close";
                    try
                    {                   
                        conn.Open();
                        state = "open";

                    }
                    catch(OracleException ex)
                    {
                        conn.Close();
                        l_Success.Text = "";
                        l_Error.Text = ex.Message;
                        //System.Diagnostics.Debug.WriteLine(ex);
                    }
                
                    if(state == "open")
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = conn;
                            cmd.CommandText = "ALTER USER " + userName + " ACCOUNT UNLOCK";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "ALTER USER " + userName + " IDENTIFIED BY \"" +  password + "\"";
                            cmd.ExecuteNonQuery();
                            l_Error.Text = "";
                            Log_File(userName, type, servicename, Environment.UserName, reason);
                            l_Success.Text = "Successfully reset password of '" + userName + "' on '" + servicename + "'";

                            var shell = PowerShell.Create();
                            // Add the script to the PowerShell object
                            shell.Commands.AddScript("C:\\Scripts\\Powershell\\OraclePasswordChanger-SendEmail.ps1 -user " + userName + " -password " + password + " -database " + servicename + " -initiator " + initiator + " -reason \"" + reason + "\"" );
                            // Execute the script
                            Collection<PSObject> results = shell.Invoke();

                        }
                        catch (OracleException ex)
                        {
                            l_Success.Text = "";
                            l_Error.Text = ex.Message;
                        }
                        finally
                        {
                            conn.Close();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "select", " selectTab1();", true);
                        }

                        break;
                    }
                }
            }
        }

        protected void Unlock_Account(object sender, EventArgs e)
        {
            string servicename = Request.Form["searchQuery2"];
            string userName = tb_Username2.Text;
            string recipients = tb_Recipients.Text;
            System.Diagnostics.Debug.WriteLine(recipients);
            string reason = tb_Reason2.Text;
            string initiator = Environment.UserName;
            string xaccount = "x" + initiator;
            string normalaccount = initiator.TrimStart('x');
            string type = "Unlock Account";



            if (string.IsNullOrWhiteSpace(servicename))
            {
                l_Error.Text = "Please select a database.";
                return;
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                l_Error.Text = "Please enter a username.";
                return;
            }
            if (string.IsNullOrWhiteSpace(reason))
            {
                l_Error.Text = "Please enter a valid reason.";
                return;
            }


            //System.Diagnostics.Debug.WriteLine(servicename);


            string rawconnectionstring = OracleConnString(servicename, "secops_ora_pwdmgmt", "CjARd72_lMsuGqP");
            string[] connectionarray = rawconnectionstring.TrimEnd('+').Split('+');

            foreach (string connectionstring in connectionarray)
            {
                System.Diagnostics.Debug.WriteLine(connectionstring);
                using (var conn = new OracleConnection(connectionstring))
                {
                    var state = "close";
                    try
                    {
                        conn.Open();
                        state = "open";

                    }
                    catch (OracleException ex)
                    {
                        conn.Close();
                        l_Success.Text = "";
                        l_Error.Text = ex.Message;
                        //System.Diagnostics.Debug.WriteLine(ex);
                    }

                    if (state == "open")
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = conn;
                            cmd.CommandText = "ALTER USER " + userName + " ACCOUNT UNLOCK";
                            cmd.ExecuteNonQuery();
                            l_Error2.Text = "";
                            Log_File(userName, type, servicename, Environment.UserName, reason);
                            l_Success2.Text = "Successfully unlocked '" + userName + "' on '" + servicename + "'";

                            var shell = PowerShell.Create();
                            // Add the script to the PowerShell object
                            shell.Commands.AddScript("C:\\Scripts\\Powershell\\OracleAccountUnlocker-SendEmail.ps1 -user " + userName + " -recipients \"" + recipients + "\" -database " + servicename + " -initiator " + initiator + " -reason \"" + reason + "\"");
                            // Execute the script
                            Collection<PSObject> results = shell.Invoke();

                        }
                        catch (OracleException ex)
                        {
                            l_Success2.Text = "";
                            l_Error2.Text = ex.Message;
                        }
                        finally
                        {
                            conn.Close();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "select", " selectTab2();", true);
                        }

                        break;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}