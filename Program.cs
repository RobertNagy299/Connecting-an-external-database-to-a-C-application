using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Win32;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace ConsoleApp1
{
    class Program
    {
       
        public static string gamedata = ""; //string, in which a player's data is stored, each field separated by a delimiter. In this case the delimiter is a '|' character
      
        //Method that initializes a newly registered player's in-game stats.
        public static void FillGameDataString()
        {
            string heroshopnumber = "HS:4|";

            string kilencelemutomb = "ALN:111011111|";

            string lightning = "LIGUPG:1|LIGB:0|LIGA:0|";

            string quicksand = "QUIUPG:1|QUIB:0|QUIA:0|";

            string egyptiangod = "EGYUPG:1|EGYB:0|EGYA:0|";

            string bigquicksand = "BQSUPG:1|BQSB:0|BQSA:0|";

            string greendiamondcounter = "GDC:100|";

            string starcounter = "STC:0|";

            string queen = "Q:1|";

            string maps = "MAP:0000000000|";

            gamedata = heroshopnumber + kilencelemutomb + lightning + quicksand
                + egyptiangod + bigquicksand + greendiamondcounter + starcounter + queen + maps;

        }

        //Method to update an already registered player's in-game data after achieving something.
        public static void UpdateGamedataString(string heroshopnumber, string kilencelemutomb, string lightning,


            string quicksand, string egyptiangod, string bigquicksand, string greendiamondcounter, string starcounter,

             string queen, string maps) 
        {
            gamedata = heroshopnumber + kilencelemutomb + lightning + quicksand
            + egyptiangod + bigquicksand + greendiamondcounter + starcounter + queen + maps;
            
            
            //data fields used for storing the game data of a player:
            /*   HS = Hero Shop,  
                 ALN = Array of Length Nine (stores character powerup and customization flags) 
                 LIGUPG = Lightning upgrade level,  LIGB: Lightning Bought: initially 0, LIGA: Lightning Automatic: intially 0
                 QUI = Quicksand, QUIB, QUIA, (same logic as with the lightning)
                 EGY = EgyptianGod
                 BQS  = BigQuickSand
                 GDC = Green Diamond Counter
                 STC = Star Counter
                 Q = Queen (Cleopatra)
                 MAP = maps, A string of length 10.
             */
            // data field begins at ':',
            // data field ends at '|' 


        }

     
        //MOVED TO PHP !!!!
        //Method to update an already registered player's in-game data
        public static void UpdateDatabase(string nickname, string password) 
        {

            String username1 = HttpUtility.UrlEncode(nickname);
            String password1 = HttpUtility.UrlEncode(password);
            String gamedata1 = HttpUtility.UrlEncode(gamedata);

            string response = SendPost("https://robiadatbazisa.000webhostapp.com/UpdateUserGameData.php", String.Format("username={0}&password_={1}&gamedata={2}", username1, password1,gamedata1));
            Console.WriteLine(response);
              
        }

        //**MOVED TO PHP !!!!
        //Method to insert a newly registered player into the database
        public static void WriteIntoDatabase(string nickname, string email, string password) 
        {
            WebClient client = new WebClient();
            NameValueCollection UserInfo = new NameValueCollection();
            UserInfo.Add("username", nickname);
            UserInfo.Add("email", email);
            UserInfo.Add("password_", password);
            UserInfo.Add("gamedata", gamedata);

            byte[] InsertUser = client.UploadValues("https://robiadatbazisa.000webhostapp.com/InsertUser.php", "POST" ,UserInfo);
           
            client.Headers.Add("Content-Type", "binary/octet-stream");

        }

           
        //Sepcial method, returns 1 if username exists, 2 if email exists, 3 if neither exist, 2 if both exist
        // MOVED TO PHP !!!!
        public static int CheckIfThisExists(string username, string email)
        {
            String username1 = HttpUtility.UrlEncode(username);
            String email1 = HttpUtility.UrlEncode(email);

            string response = SendPost("https://robiadatbazisa.000webhostapp.com/CheckIfThisExists.php", String.Format("username={0}&email={1}", username1, email1));

            return Convert.ToInt32(response);       
        }


        //Method to upload data into the PHP script
        public static string SendPost(string url, string postData)
        {
            string webpageContent = string.Empty;

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;

                using (Stream webpageStream = webRequest.GetRequestStream())
                {
                    webpageStream.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        webpageContent = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw or return an appropriate response/exception
            }

            return webpageContent;
        }
        

        /*MOVED TO PHP !!!!*/
        //Method to get the player's in-game data from the database, based on their nickname and password.
        public static void ReadFromDatabase(string nickname,  string password)
        {
           // WebClient client = new WebClient();
          
            String username = HttpUtility.UrlEncode(nickname);
            String password_ = HttpUtility.UrlEncode(password);


            gamedata =  SendPost("https://robiadatbazisa.000webhostapp.com/SelectGameData.php", String.Format("username={0}&password_={1}", username, password_));
           
            Console.WriteLine(gamedata);

         
        }

       
        // MOVED TO PHP !!!!
        //Method that updates the user's password based on their nickname, email, and the new password they entered.
        //Intended to be used as "forgot password" function, hence, we are not asking for the old password here.
        public static void UpdateUserPassword(string nickname, string email, string newpassword)
        {

            String username = HttpUtility.UrlEncode(nickname);
            String newpassword1 = HttpUtility.UrlEncode(newpassword);
            String email1 = HttpUtility.UrlEncode(email);

            string response = SendPost("https://robiadatbazisa.000webhostapp.com/UpdateUserPassword.php", String.Format("username={0}&newpassword={1}&email={2}", username, newpassword1,email1));

            Console.WriteLine(response);
           
        }

        //MOVED TO PHP !!!!
        //Method to check if the user exists, based on nickname and email.
        public static bool CheckIfThisUserExists(string username, string email)
        {
            String username1 = HttpUtility.UrlEncode(username);
            String email1 = HttpUtility.UrlEncode(email);

            string response = SendPost("https://robiadatbazisa.000webhostapp.com/CheckIfUserExists.php", String.Format("username={0}&email={1}", username1, email1));
           // Console.WriteLine(response);
            if(response == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

           
            
      
        }

        /*MOVED TO PHP !!!!*/
        //Method to delete user based on their nickname and password
        //In the database, nicknames must be unique, because that's how players identify each other in-game,
        //so it is enough to ask for a nickname and password when deleting an account.
        public static void DeleteUserFromDB(string nickname, string password)
        {
            try
            {
                String username = HttpUtility.UrlEncode(nickname);
                String password_ = HttpUtility.UrlEncode(password);


                string response = SendPost("https://robiadatbazisa.000webhostapp.com/DeleteUser.php", String.Format("username={0}&password_={1}", username, password_));

                Console.WriteLine(response);
            }
            catch(Exception error1)
            {
                //hiba dobas
                Console.WriteLine(error1.Message);
            }

               
        }

        //Method that sends a complaint message into a separate table in the database.
        public static void SendComplaint(string content)
        {
            try
            {
                String content1 = HttpUtility.UrlEncode(content);
                String currentime = HttpUtility.UrlEncode(DateTime.UtcNow.ToString());


                string response = SendPost("https://robiadatbazisa.000webhostapp.com/SendComplaint.php", String.Format("content={0}&timedata={1}", content1, currentime));

                Console.WriteLine(response);
            }
            catch (Exception error1)
            {
                //Error handling
                Console.WriteLine(error1.Message);
            }


        }
        //Method that inserts the player's playtime (measured in seconds) into the database.
        public static void InsertPlayTime(string username, float playtime)
        {
            try
            {
                String username1 = HttpUtility.UrlEncode(username);
                String playtime1 = HttpUtility.UrlEncode(playtime.ToString());


                string response = SendPost("https://robiadatbazisa.000webhostapp.com/InsertPlayTime.php", String.Format("username={0}&playtime={1}", username1, playtime1));

                Console.WriteLine(response);
            }
            catch (Exception error1)
            {
                //hiba dobas
                Console.WriteLine(error1.Message);
            }
        }

        //Method that reads the username from the database, based on a given email address
        public static void FetchUsernameThroughEmail(string email)
        {
            String email1 = HttpUtility.UrlEncode(email);


            //The username is stored in this string 
            string username = SendPost("https://robiadatbazisa.000webhostapp.com/FetchUsernameThroughEmail.php", String.Format("email={0}", email1));

            Console.WriteLine(username);

        }

        //Method that changes a player's username based on their email address.
        //(The query looks for an entry in the database with the same email address, and updates the nickname field of that entry)
        public static void ChangeUsernameThroughEmail(string email, string newname)
        {
            String email1 = HttpUtility.UrlEncode(email);
            String newn1 = HttpUtility.UrlEncode(newname);

           
            string usernamechange = SendPost("https://robiadatbazisa.000webhostapp.com/ChangeUsernameThroughEmail.php", String.Format("email={0}&newname={1}", email1,newn1));

           // Console.WriteLine(username);

        }

        //Method that changes a user's username based on their old name and current password.
        public static void ChangeUsernameThroughOldUsernameAndPassword(string oldname,string password ,string newname)
        {
            String oldname1 = HttpUtility.UrlEncode(oldname);
            String newn1 = HttpUtility.UrlEncode(newname);
            String pass1 = HttpUtility.UrlEncode(password);

            //The response is stored in this string 
            string username = SendPost("https://robiadatbazisa.000webhostapp.com/ChangeUsernameThroughOldUsernameAndPassword.php", String.Format("oldname={0}&newname={1}&password={2}", oldname1, newn1,pass1));

             Console.WriteLine(username);

        }

        static void Main(string[] args)
        {
            FillGameDataString();
         
           Console.WriteLine("Enter a username, an email address and a password!");
            // Console.WriteLine("Enter your name, password");
           //  Console.WriteLine("Enter your name, email");
            // Console.WriteLine("Enter your name, email and new password");
            string u, e, p;
           // string name = "Andrew";
           // string email = "gemail@gmail.com";
           // string pass = "SheQa44";
           // WriteIntoDatabase(name, email, pass);
                
           
            u = Console.ReadLine();
            e = Console.ReadLine();
            p = Console.ReadLine();

            WriteIntoDatabase(u, e, p);

            //  e = Console.ReadLine();
            // p = Console.ReadLine();

           
            // float playtime = 99.12f;
          //  InsertPlayTime(u, playtime);


            // ReadFromDatabase(u, p);


           // ChangeUsernameThroughOldUsernameAndPassword(u,e,p);

           // DeleteUserFromDB(u, p);

            //  UpdateUserPassword(u, e, p);
            //  gamedata = "NEW";
            // UpdateDatabase(u, p);
            // int ans = CheckIfThisExists(u, e);
            // Console.WriteLine(ans);
             // ChangeUsernameThroughEmail(e,u);
            // bool ans = CheckIfThisUserExists(u,e);
            //  Console.WriteLine(ans);
            //  SendComplaint(u);

            /* try
             {
                 Console.WriteLine("Do you already have an account? press 1 if yes, 0 if no");
                 int ans = Convert.ToInt32(Console.ReadLine());
                 if (ans == 0)
                 {
                     Console.WriteLine("Enter your nickname: ");
                     string nick = Console.ReadLine();
                     Console.WriteLine("Enter your email: ");
                     string email = Console.ReadLine();
                     Console.WriteLine("Enter your password: ");
                     string pass = Console.ReadLine();

                     WriteIntoDatabase(nick, email, pass);
                 }
                 else
                 {
                     Console.WriteLine("Enter your nickname: ");
                     string nick = Console.ReadLine();
                     Console.WriteLine("Enter your password: ");
                     string pass = Console.ReadLine();

                     ReadFromDatabase(nick, pass);

                     Console.WriteLine("Do you want to delete your account? 1 for yes, 0 for no");
                     int delans = Convert.ToInt32(Console.ReadLine());
                     if(delans == 1)
                     {
                         DeleteUserFromDB(nick);
                         Console.WriteLine("Exit application");
                     }


                     Console.WriteLine("Enter a number");
                     int num = Convert.ToInt32(Console.ReadLine());


                     int firstindex = gamedata.IndexOf("HS:");
                     int secondindex = gamedata.IndexOf("|ALN");
                     string HeroShopNum = gamedata.Substring(firstindex + 3, secondindex - 3);

                     Console.WriteLine("Heroshop: " + HeroShopNum);

                     string NewHSnum = "HS:" + $"{num}" + "|";


                     string kilencelemutomb = "ALN:111011111|";

                     string lightning = "LIGUPG:1|LIGB:0|LIGA:0|";

                     string quicksand = "QUIUPG:1|QUIB:0|QUIA:0|";

                     string egyptiangod = "EGYUPG:1|EGYB:0|EGYA:0|";

                     string bigquicksand = "BQSUPG:1|BQSB:0|BQSA:0|";

                     string greendiamondcounter = "GDC:100|";

                     string starcounter = "STC:0|";

                     string queen = "Q:1|";

                     string maps = "MAP:0000000000|";

                     UpdateGamedataString(NewHSnum, kilencelemutomb, lightning, quicksand, egyptiangod, bigquicksand,
                         greendiamondcounter, starcounter, queen, maps);

                     UpdateDatabase(nick, pass);


                 }


             }
             catch(Exception err)
             {
                 Console.Write(err.Message);
             }*/






            Console.ReadLine();
        }
    }
}
