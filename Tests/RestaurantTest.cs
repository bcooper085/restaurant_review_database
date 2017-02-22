using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace DerpApp
{
    public class RestaurantTest : IDisposable
    {
        public RestaurantTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurant_test;Integrated Security=SSPI;";
        }
        public void Dispose()
        {
            Restaurant.DeleteAll();
        }

        [Fact]
        public void Test_IfEmptyOnLoad_Empty()
        {
            int result = Restaurant.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_IfNameIsEqual_Equal()
        {
            Restaurant first = new Restaurant("Pizza Palace", 1);
            Restaurant second = new Restaurant("Pizza Palace", 1);

            Assert.Equal(first, second);
        }

        [Fact]
        public void Test_SavesToDatabase_Save()
        {
            Restaurant newRestaurant = new Restaurant("Pizza Palace", 1);
            newRestaurant.Save();

            List<Restaurant> result = Restaurant.GetAll();
            List<Restaurant> testResult = new List<Restaurant>{newRestaurant};

            Assert.Equal(testResult, result);
        }

        [Fact]
        public void Test_FindSearchedRestaurant_Find()
        {
            Restaurant newRestaurant = new Restaurant("Pizza Palace", 1);
            newRestaurant.Save();

            Restaurant foundRestaurant = Restaurant.Find(newRestaurant.GetId());

            Assert.Equal(newRestaurant, foundRestaurant);
        }

        [Fact]
        public void Test_SpecificRestaurantsBasedOnCuisine_Find()
        {
            Cuisine newCuisine = new Cuisine("Italian");
            newCuisine.Save();
            Restaurant newRestaurant = new Restaurant("Pizza Palace", newCuisine.GetId());
            newRestaurant.Save();

            Cuisine testCuisine = new Cuisine("Mexican");
            testCuisine.Save();
            Restaurant testRestaurant = new Restaurant("Mexican Palace", testCuisine.GetId());
            testRestaurant.Save();


            List<Restaurant> foundRestaurants = Restaurant.GetByCuisine(newCuisine.GetId());
            List<Restaurant> expectedRestaurantList = new List<Restaurant>{newRestaurant};

            Assert.Equal(expectedRestaurantList, foundRestaurants);
        }

        [Fact]
        public void Test_DeleteIndividualRestaurant_Delete()
        {
            Restaurant newRestaurant = new Restaurant("Pizza Palace", 1);
            newRestaurant.Save();
            Restaurant.DeleteSpecific(newRestaurant.GetId());

            List<Restaurant> testResult = new List<Restaurant>{};
            List<Restaurant> result = Restaurant.GetAll();

            Assert.Equal(testResult, result);
        }
    }
}
