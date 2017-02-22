using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;

namespace DerpApp
{
    public class Cuisine
    {
        private string _name;
        private int _id;

        public Cuisine(string Name, int Id = 0)
        {
            _name = Name;
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

        public static List<Cuisine> GetAll()
        {
            List<Cuisine> allCuisines = new List<Cuisine>();
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines;", conn);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                Cuisine foundCuisine = new Cuisine(foundName, foundId);
                allCuisines.Add(foundCuisine);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return allCuisines;
        }
        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM cuisines;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public override bool Equals(System.Object otherCuisine)
        {
            if(!(otherCuisine is Cuisine))
            {
                return false;
            }
            else
            {
                Cuisine newCuisine = (Cuisine) otherCuisine;
                bool idEquality = (this.GetId() == newCuisine.GetId());
                bool nameEquality = (this.GetName() == newCuisine.GetName());
                return(idEquality && nameEquality);
            }
        }


    }
}
