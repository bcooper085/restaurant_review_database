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
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO cuisines(name) OUTPUT INSERTED.id VALUES(@CuisineName);", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@CuisineName";
            nameParameter.Value = this.GetName();
            cmd.Parameters.Add(nameParameter);

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

        public static Cuisine Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines WHERE id = @CuisineId;", conn);
            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@CuisineId";
            idParameter.Value = id.ToString();

            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundName = null;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
            }

            Cuisine foundCuisine = new Cuisine(foundName, foundId);

            return foundCuisine;
        }

        public static Cuisine FindByName(string searchedName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines WHERE name = @CuisineName;", conn);
            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@CuisineName";
            idParameter.Value = searchedName;

            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundName = null;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
            }

            Cuisine foundCuisine = new Cuisine(foundName, foundId);

            return foundCuisine;
        }

        public void UpdateName(string NewName)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE cuisines SET name = @NewName OUTPUT INSERTED.name WHERE id = @CuisineId;", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@NewName";
            nameParameter.Value = NewName;
            cmd.Parameters.Add(nameParameter);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@CuisineId";
            idParameter.Value = this.GetId();
            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._name = rdr.GetString(0);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteSpecific(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM cuisines WHERE id = @CuisineId;", conn);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@CuisineId";
            idParameter.Value = id;
            cmd.Parameters.Add(idParameter);
            cmd.ExecuteNonQuery();
            conn.Close();
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
