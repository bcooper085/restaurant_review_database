using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;

namespace DerpApp
{
    public class Restaurant
    {
        private string _name;
        private int _id;
        private int _cuisineId;

        public Restaurant(string Name, int CuisineId, int Id = 0)
        {
            _name = Name;
            _cuisineId = CuisineId;
            _id = Id;
        }
        public string GetName()
        {
            return _name;
        }
        public int GetId()
        {
            return _id;
        }

        public int GetCuisineId()
        {
            return _cuisineId;
        }

        public static List<Restaurant> GetAll()
        {
            List<Restaurant> allRestaurants = new List<Restaurant>();
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                int foundCuisine = rdr.GetInt32(2);
                Restaurant foundRestaurant = new Restaurant(foundName, foundCuisine, foundId);
                allRestaurants.Add(foundRestaurant);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return allRestaurants;
        }
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO restaurants(name, cuisine_Id) OUTPUT INSERTED.id VALUES(@RestaurantName, @CuisineId);", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@RestaurantName";
            nameParameter.Value = this.GetName();
            cmd.Parameters.Add(nameParameter);

            SqlParameter cuisineParameter = new SqlParameter();
            cuisineParameter.ParameterName = "@CuisineId";
            cuisineParameter.Value = this.GetCuisineId();
            cmd.Parameters.Add(cuisineParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
        }
        public static Restaurant Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @RestaurantId;", conn);
            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@RestaurantId";
            idParameter.Value = id.ToString();

            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundName = null;
            int foundCuisine = 0;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
                foundCuisine = rdr.GetInt32(2);
            }

            Restaurant foundRestaurant = new Restaurant(foundName, foundCuisine, foundId);

            return foundRestaurant;
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<Restaurant> GetByCuisine(int id)
        {

            List<Restaurant> foundByCuisineRestaurants = new List<Restaurant>{};
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE cuisine_id = @CuisineId", conn);

            SqlParameter cuisineParameter = new SqlParameter();
            cuisineParameter.ParameterName = "@CuisineId";
            cuisineParameter.Value = id;
            cmd.Parameters.Add(cuisineParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                int foundCuisineId = rdr.GetInt32(2);
                Restaurant foundRestaurant = new Restaurant(foundName, foundCuisineId, foundId);
                foundByCuisineRestaurants.Add(foundRestaurant);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }

            return foundByCuisineRestaurants;


        }




//Override Equal
        public override bool Equals(System.Object otherRestaurant)
        {
            if(!(otherRestaurant is Restaurant))
            {
                return false;
            }
            else
            {
                Restaurant newRestaurant = (Restaurant) otherRestaurant;
                bool idEquality = (this.GetId() == newRestaurant.GetId());
                bool nameEquality = (this.GetName() == newRestaurant.GetName());
                bool cuisineEquality = (this.GetCuisineId() == newRestaurant.GetCuisineId());
                return(idEquality && nameEquality && cuisineEquality);
            }
        }
    }
}
