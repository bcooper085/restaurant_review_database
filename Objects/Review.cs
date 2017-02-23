using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;

namespace DerpApp
{
    public class Review
    {
        private string _reviewer;
        private string _review;
        private int _id;
        private int _restaurantId;

        public Review(string Name, string Review, int RestaurantId, int Id = 0)
        {
            _reviewer = Name;
            _review = Review;
            _restaurantId = RestaurantId;
            _id = Id;
        }
        public string GetName()
        {
            return _reviewer;
        }
        public int GetId()
        {
            return _id;
        }

        public string GetReview()
        {
            return _review;
        }

        public int GetRestaurantId()
        {
            return _restaurantId;
        }

        public static List<Review> GetAll()
        {
            List<Review> allReviews = new List<Review>();
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM reviews;", conn);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                string foundReview = rdr.GetString(2);
                int foundRestaurant = rdr.GetInt32(3);
                Review foundFullReview = new Review(foundName, foundReview, foundRestaurant, foundId);
                allReviews.Add(foundFullReview);
            }

            if(rdr != null)
            {
                rdr.Close();
            }

            if(conn != null)
            {
                conn.Close();
            }

            return allReviews;
        }
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO reviews(reviewer, review, restaurant_Id) OUTPUT INSERTED.id VALUES(@ReviewerName, @Review, @RestaurantId);", conn);

            SqlParameter nameParameter = new SqlParameter();
            nameParameter.ParameterName = "@ReviewerName";
            nameParameter.Value = this.GetName();
            cmd.Parameters.Add(nameParameter);

            SqlParameter reviewParameter = new SqlParameter();
            reviewParameter.ParameterName = "@Review";
            reviewParameter.Value = this.GetReview();
            cmd.Parameters.Add(reviewParameter);

            SqlParameter restaurantParameter = new SqlParameter();
            restaurantParameter.ParameterName = "@RestaurantId";
            restaurantParameter.Value = this.GetRestaurantId();
            cmd.Parameters.Add(restaurantParameter);

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
        public static Review Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE id = @ReviewId;", conn);
            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@ReviewId";
            idParameter.Value = id.ToString();

            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundName = null;
            string foundReview = null;
            int foundRestaurant = 0;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
                foundReview = rdr.GetString(2);
                foundRestaurant = rdr.GetInt32(3);
            }

            Review foundFullReview = new Review(foundName, foundReview, foundRestaurant, foundId);

            return foundFullReview;
        }

        public void UpdateReview(string NewReview)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE reviews SET review = @NewReview OUTPUT INSERTED.review WHERE id = @ReviewId;", conn);
            SqlParameter newReviewParameter = new SqlParameter();
            newReviewParameter.ParameterName = "@NewReview";
            newReviewParameter.Value = NewReview;
            cmd.Parameters.Add(newReviewParameter);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@ReviewId";
            idParameter.Value = this.GetId();
            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._review = rdr.GetString(0);
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

        public static void DeleteSpecific(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM reviews WHERE id = @ReviewId;", conn);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@ReviewId";
            idParameter.Value = id;
            cmd.Parameters.Add(idParameter);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM reviews;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<Review> GetByRestaurant(int id)
        {

            List<Review> foundByRestaurantReviews = new List<Review>{};
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE restaurant_id = @RestaurantId;", conn);

            SqlParameter restaurantParameter = new SqlParameter();
            restaurantParameter.ParameterName = "@RestaurantId";
            restaurantParameter.Value = id;
            cmd.Parameters.Add(restaurantParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                string foundReview = rdr.GetString(2);
                int foundRestaurantId = rdr.GetInt32(3);
                Review foundFullReview = new Review(foundName, foundReview, foundRestaurantId, foundId);
                foundByRestaurantReviews.Add(foundFullReview);
            }

            if(rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }

            return foundByRestaurantReviews;


        }




//Override Equal
        public override bool Equals(System.Object otherReview)
        {
            if(!(otherReview is Review))
            {
                return false;
            }
            else
            {
                Review newReview = (Review) otherReview;
                bool idEquality = (this.GetId() == newReview.GetId());
                bool nameEquality = (this.GetName() == newReview.GetName());
                bool restaurantEquality = (this.GetRestaurantId() == newReview.GetRestaurantId());
                return(idEquality && nameEquality && restaurantEquality);
            }
        }
    }
}
