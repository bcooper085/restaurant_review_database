using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace DerpApp
{
    public class ReviewTest : IDisposable
    {
        public ReviewTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurant_test;Integrated Security=SSPI;";
        }
        public void Dispose()
        {
            Review.DeleteAll();
            Cuisine.DeleteAll();
            Restaurant.DeleteAll();
        }

        [Fact]
        public void Test_IfEmptyOnLoad_Empty()
        {
            int result = Review.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_IfNameIsEqual_Equal()
        {
            Review first = new Review("Person", "This is my review", 1);
            Review second = new Review("Person", "This is my review", 1);

            Assert.Equal(first, second);
        }

        [Fact]
        public void Test_SavesToDatabase_Save()
        {
            Review newReview = new Review("Person", "This is my review", 1);
            newReview.Save();

            List<Review> result = Review.GetAll();
            List<Review> testResult = new List<Review>{newReview};

            Assert.Equal(testResult, result);
        }

        [Fact]
        public void Test_FindSearchedReview_Find()
        {
            Review newReview = new Review("Person", "This is my review", 1);
            newReview.Save();

            Review foundReview = Review.Find(newReview.GetId());

            Assert.Equal(newReview, foundReview);
        }

        [Fact]
        public void Test_SpecificReviewsBasedOnRestaurant_Find()
        {
            Restaurant newRestaurant = new Restaurant("Pizza Palace", 1);
            newRestaurant.Save();
            Review newReview = new Review("Person", "This is my review", newRestaurant.GetId());
            newReview.Save();

            Restaurant testRestaurant = new Restaurant("Mexican", 1);
            testRestaurant.Save();
            Review testReview = new Review("Person", "This is my review", testRestaurant.GetId());
            testReview.Save();


            List<Review> foundReviews = Review.GetByRestaurant(newRestaurant.GetId());
            List<Review> expectedReviewList = new List<Review>{newReview};

            Assert.Equal(expectedReviewList, foundReviews);
        }

        [Fact]
        public void Test_DeleteIndividualReview_Delete()
        {
            Review newReview = new Review("Person", "This is my review", 1);
            newReview.Save();
            Review.DeleteSpecific(newReview.GetId());

            List<Review> testResult = new List<Review>{};
            List<Review> result = Review.GetAll();

            Assert.Equal(testResult, result);
        }

        [Fact]
        public void Test_UpdateReview_Update()
        {
            Review newReview = new Review("Person", "This is my review", 1);
            newReview.Save();

            newReview.UpdateReview("This is my other review");

            Assert.Equal("This is my other review", newReview.GetReview());
        }

    }
}
