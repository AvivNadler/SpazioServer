using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using SpazioServer.Models;



public class DBServices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBServices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand
    ////---------------------------------------------------------------------------------
    public SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }



    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the USER functions ******
    //--------------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------------
    // This method inserts a order to the Orders table 
    //--------------------------------------------------------------------------------------------------
    public int insert(User user)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(user);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(User user)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', '{3}','{4}','{5}',{6},{7},'{8}')", user.Email, user.Password, user.UserName, user.PhoneNumber, user.Photo, user.SpaceOwner, user.Visits, user.Rank, time.ToString(format));
        String prefix = "INSERT INTO Users_2020" + "(Email,Password,UserName,PhoneNumber,Photo,SpaceOwner,visits,rank,RegisterationDate) ";
        command = prefix + sb.ToString();

        return command;
    }
    //--------------------------------------------------------------------------------------------------
    // This method reads specific user by id parameter 
    //--------------------------------------------------------------------------------------------------
    public User readUser(int id)
    {
        User u = new User();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020 Where Id=" + id;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row


                u.Id = Convert.ToInt32(dr["Id"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.UserName = (string)dr["UserName"];
                u.PhoneNumber = (string)dr["PhoneNumber"];
                u.Photo = (string)dr["Photo"];
                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
                u.Visits = Convert.ToInt32(dr["visits"]);
                u.Rank = Convert.ToDouble(dr["rank"]);
                u.RegistrationDate = dr["RegisterationDate"].ToString();
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return u;
    }
    //--------------------------------------------------------------------------------------------------
    // This method reads all the users from the users table 
    //--------------------------------------------------------------------------------------------------
    public bool userTypeCheckandUpdate(string email)
    {
        bool userType = false;
        User u = new User();
        int numEffected;
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020 Where Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        if (!u.SpaceOwner)
        {
            userType = false;
            numEffected = updateUserType(email);
            if (numEffected == 1)
            {
                userType = true;
            }
        }
        return userType;
    }
    private int updateUserType(string email)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("SET SpaceOwner = {0} WHERE Email = '{1}'", 1, email);
        String prefix = "UPDATE Users_2020 ";
        command = prefix + sb.ToString();

        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(command, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }


    public List<User> readUsers()
    {
        List<User> Users = new List<User>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                User u = new User();

                u.Id = Convert.ToInt32(dr["Id"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.UserName = (string)dr["UserName"];
                u.PhoneNumber = (string)dr["PhoneNumber"];
                u.Photo = (string)dr["Photo"];
                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
                u.Visits = Convert.ToInt32(dr["visits"]);
                u.Rank = Convert.ToDouble(dr["rank"]);
                u.RegistrationDate = dr["RegisterationDate"].ToString();

                Users.Add(u);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Users;
    }

    //--------------------------------------------------------------------------------------------------
    // This method reads all favourites spaces id of specific user by id parameter 
    //--------------------------------------------------------------------------------------------------
    public List<int> readFavouritesSpaces(int id)
    {
        List<int> spacesId = new List<int>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT SpaceId FROM Favourites_2020 Where UserId=" + id;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row


                spacesId.Add(Convert.ToInt32(dr["SpaceId"]));



            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return spacesId;
    }

    //--------------------------------------------------------------------
    // Build the Delete command method String for Users table
    //--------------------------------------------------------------------
    private String BuildDeleteUserCommand(int id)
    {
        String command;
        command = "delete from Users_2020 where Id = " + id.ToString();
        return command;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Delete specific user by id parameter 
    //--------------------------------------------------------------------------------------------------
    public int deleteUser(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteUserCommand(id);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method reads all the new users from the lat 30 days, from the users table 
    //--------------------------------------------------------------------------------------------------
    public List<User> readUsersFromLastThirtyDays()
    {
        List<User> Users = new List<User>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020 WHERE datediff(DAY,RegistrationDate,getdate())<=30";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                User u = new User();

                u.Id = Convert.ToInt32(dr["Id"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.UserName = (string)dr["UserName"];
                u.PhoneNumber = (string)dr["PhoneNumber"];
                u.Photo = (string)dr["Photo"];
                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
                u.Visits = Convert.ToInt32(dr["visits"]);
                u.Rank = Convert.ToDouble(dr["rank"]);
                u.RegistrationDate = (string)dr["RegistrationDate"];

                Users.Add(u);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Users;
    }


    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the SPACE functions ******
    //--------------------------------------------------------------------------------------------------
    public List<Space> readSpaces()
    {
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Spaces_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];
                s.Rank = Convert.ToDouble(dr["Rank"]);
                s.Uploadtime = dr["UploadDate"].ToString();

                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }

    public List<Space> readSpacesFromLastThirtyDays()
    {
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Spaces_2020 Where datediff(DAY,UploadDate,getdate())<=30 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];
                s.Rank = Convert.ToDouble(dr["Rank"]);
                s.Uploadtime = dr["UploadDate"].ToString();



                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }

    public int insert(Space space)
    {
        SqlConnection con;
        SqlCommand cmd;

        //DBServices dbs = new DBServices();

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(space);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        // the Id is made automatically in the sql so this data reader and the query above will help us get this new id
        int newSpaceInsertedId = -1;
        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        while (dr.Read())
        {   // Read till the end of the data into a row

            newSpaceInsertedId = Convert.ToInt32(dr["SpaceId"]); // get the new space id
        }
        //f.SpaceId = newSpaceInsertedId;
        //e.SpaceId = newSpaceInsertedId;
        //a.SpaceId = newSpaceInsertedId;
        //dbs.insert(f);
        //dbs.insert(e);
        //dbs.insert(a);
        try
        {
            //int numEffected = cmd.ExecuteNonQuery();
            //return numEffected;
            return newSpaceInsertedId;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Space space)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values('{0}', '{1}' , {2}, '{3}','{4}', '{5}' , {6}, '{7}','{8}', '{9}' ,'{10}', '{11}','{12}', '{13}' ,'{14}', '{15}', '{16}', '{17}', {18},'{19}')", space.Name, space.Field, space.Price, space.City, space.Street, space.Number, space.Capabillity, space.Bank, space.Branch, space.Imageurl1, space.Imageurl2, space.Imageurl3, space.Imageurl4, space.Imageurl5, space.AccountNumber, space.UserEmail, space.Description, space.TermsOfUse, space.Rank, time.ToString(format));
        String prefix = "INSERT INTO Spaces_2020" + "([SpaceName] ,[Field] ,[Price],[City],[Street] ,[Number],[Capabillity] ,[Bank] ,[Branch]  ,[Image1]  ,[Image2],[Image3],[Image4],[Image5],[AccountNumber],[FKEmail],[Description],[TermsOfUse],[Rank], [UploadDate]) OUTPUT Inserted.SpaceId ";
        command = prefix + sb.ToString();

        return command;
    }

    private String BuildDeleteSpaceCommand(int id)
    {
        String command;
        command = "delete from Spaces_2020 where SpaceId = " + id.ToString();
        return command;
    }

    public int deleteSpace(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteSpaceCommand(id);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Favourite functions ******
    //--------------------------------------------------------------------------------------------------
    public List<Favourite> readFavourites()
    {
        List<Favourite> favourites = new List<Favourite>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Favourites_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Favourite f = new Favourite();

                f.UserId = Convert.ToInt32(dr["UserId"]);
                f.SpaceId = Convert.ToInt32(dr["SpaceId"]);




                favourites.Add(f);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return favourites;
    }

    public int insert(Favourite favourite)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(favourite);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Favourite favourite)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1})", favourite.UserId, favourite.SpaceId);
        String prefix = "INSERT INTO Favourites_2020" + "(UserId,SpaceId)";
        command = prefix + sb.ToString();

        return command;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Delete specific favourite by id parameter 
    //--------------------------------------------------------------------------------------------------
    public int removeFavourite(int spaceId, int userId)
    {

        String command;
        command = "delete from Favourites_2020 where SpaceId = " + spaceId.ToString() + "AND UserId=" + userId.ToString();

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = command;      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Equipment functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(Equipment eq)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(eq);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Equipment eq)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}')", eq.Name, eq.SpaceId);
        String prefix = "INSERT INTO Equipment_2020" + "(EquipmentName,FKSpaceId) ";
        command = prefix + sb.ToString();

        return command;
    }

    public List<Equipment> readEquipments()
    {
        List<Equipment> Equipments = new List<Equipment>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Equipment_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Equipment eq = new Equipment();

                eq.Id = Convert.ToInt32(dr["EquipmentId"]);
                eq.Name = (string)dr["EquipmentName"];
                eq.SpaceId = Convert.ToInt32(dr["FKSpaceId"]);
                Equipments.Add(eq);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Equipments;
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Facility functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(Facility f)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(f);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Facility f)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}','{4}', '{5}','{6}', '{7}')", f.Parking, f.Toilet, f.Kitchen, f.Intercom, f.Accessible, f.AirCondition, f.Wifi, f.SpaceId);
        String prefix = "INSERT INTO Facilities_2020" + "(Parking, Toilet, Kitchen, Intercom ,Accessible, AirCondition, WiFi, FKSpaceId)";
        command = prefix + sb.ToString();

        return command;
    }

    public List<Facility> readFacilities()
    {
        List<Facility> Facilities = new List<Facility>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Facilities_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Facility f = new Facility();

                f.Id = Convert.ToInt32(dr["FacilityId"]);
                f.Parking = Convert.ToBoolean(dr["Parking"]);
                f.Toilet = Convert.ToBoolean(dr["Toilet"]);
                f.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                f.Intercom = Convert.ToBoolean(dr["Intercom"]);
                f.Accessible = Convert.ToBoolean(dr["Accessible"]);
                f.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                f.Wifi = Convert.ToBoolean(dr["WiFi"]);
                f.SpaceId = Convert.ToInt32(dr["FKSpaceId"]);


                Facilities.Add(f);

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Facilities;
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Availability functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(Availability a)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(a);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Availability a)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}','{4}', '{5}','{6}', '{7}')", a.Sunday, a.Monday, a.Tuesday, a.Wednesday, a.Thursday, a.Friday, a.Saturday, a.SpaceId);
        String prefix = "INSERT INTO Availability_2020" + "(Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday, FKSpaceId)";
        command = prefix + sb.ToString();

        return command;
    }

    public List<Availability> readAvailabilities()
    {
        List<Availability> Availabilities = new List<Availability>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Availability_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Availability a = new Availability();

                a.Id = Convert.ToInt32(dr["AvailabilityId"]);
                a.Sunday = (string)dr["Sunday"];
                a.Monday = (string)dr["Monday"];
                a.Tuesday = (string)dr["Tuesday"];
                a.Wednesday = (string)dr["Wednesday"];
                a.Thursday = (string)dr["Thursday"];
                a.Friday = (string)dr["Friday"];
                a.Saturday = (string)dr["Saturday"];
                a.SpaceId = Convert.ToInt32(dr["FKSpaceId"]);

                Availabilities.Add(a);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Availabilities;
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Fields by Equipment functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(FieldEq fe)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(fe);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(FieldEq fe)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}','{2}')", fe.Id, fe.Field, fe.Name);
        String prefix = "INSERT INTO FieldEquipment_2020" + "(EquipmentId,Field,EquipmentName)";
        command = prefix + sb.ToString();

        return command;
    }

    public List<FieldEq> readFieldsEq()
    {
        List<FieldEq> FieldsEq = new List<FieldEq>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM FieldEquipment_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                FieldEq fe = new FieldEq();

                fe.Id = Convert.ToInt32(dr["EquipmentId"]);
                fe.Field = (string)dr["Field"];
                fe.Name = (string)dr["EquipmentName"];



                FieldsEq.Add(fe);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return FieldsEq;
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Admin functions ******
    //--------------------------------------------------------------------------------------------------

    public List<Admin> readAdmins()
    {
        List<Admin> admins = new List<Admin>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Admins_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Admin a = new Admin();

                a.Id = Convert.ToInt32(dr["ID"]);
                a.UserName = (string)dr["UserName"];
                a.Password = (string)dr["Password"];
                a.FirstName = (string)dr["FirstName"];
                a.LastName = (string)dr["LastName"];

                admins.Add(a);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return admins;
    }

    public Admin GetAdmin(string username)
    {

        SqlConnection con = null;
        Admin a = new Admin();
        try
        {
            con = connect("database"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM Admins_2020 WHERE UserName= '" + username + "'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                a.UserName = dr["UserName"].ToString();
                a.FirstName = dr["FirstName"].ToString();
                a.LastName = dr["LastName"].ToString();
            }

            return a;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public bool CheckAdmin(string username, string password)
    {
        bool userAdminInDB = false;
        SqlConnection con = null;
        try
        {
            con = connect("database"); // create a connection to the database using the connection String defined in the web config file
            string query = "select * from Admins_2020 where UserName ='" + username + "'";
            SqlCommand cmd = new SqlCommand(query, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                if (dr["Password"].ToString() == password)
                {
                    userAdminInDB = true;
                }
            }

            return userAdminInDB;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Order functions ******
    //--------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------
    // Build the Insert command method String for Orders table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(Order order)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;
        // string timeInString = DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss");

        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1},'{2}','{3}','{4}','{5}', {6})", order.SpaceId.ToString(), order.UserId.ToString(), order.ReservationDate, order.StartHour, order.EndHour, time.ToString(format), order.Price);
        String prefix = "INSERT INTO Orders_2020 " + "(SpaceId, UserId, ReservationDate, StartHour, EndHour, OrderDate, Price) ";
        command = prefix + sb.ToString();

        return command;
    }
    //--------------------------------------------------------------------------------------------------
    // This method inserts a order to the Orders table 
    //--------------------------------------------------------------------------------------------------
    public int insert(Order order)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand(order);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------------------------------------
    // This method reads all the orders from the Orders table 
    //--------------------------------------------------------------------------------------------------
    public List<Order> readOrders()
    {
        List<Order> orders = new List<Order>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Orders_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Order o = new Order();

                o.OrderId = Convert.ToInt32(dr["OrderId"]);
                o.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                o.UserId = Convert.ToInt32(dr["UserId"]);
                o.ReservationDate = (string)dr["ReservationDate"];
                o.StartHour = (string)dr["StartHour"];
                o.EndHour = (string)dr["EndHour"];
                o.Price = Convert.ToDouble(dr["Price"]);
                o.OrderDate = dr["OrderDate"].ToString();


                orders.Add(o);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return orders;
    }

    public Order readOrder(int id)
    {
        Order o = new Order();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Orders_2020 Where OrderId='" + id + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                o.OrderId = Convert.ToInt32(dr["OrderId"]);
                o.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                o.UserId = Convert.ToInt32(dr["UserId"]);
                o.ReservationDate = (string)dr["ReservationDate"];
                o.StartHour = (string)dr["StartHour"];
                o.EndHour = (string)dr["EndHour"];
                o.Price = Convert.ToDouble(dr["Price"]);
                o.OrderDate = (string)dr["OrderDate"];

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return o;
    }


}
