using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using Xunit;

namespace DerpApp
{
    public class CuisineTest : IDisposable
    {
        public CuisineTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurant_test;Integrated Security=SSPI;";
        }
        public void Dispose()
        {
            Cuisine.DeleteAll();
        }

        [Fact]
        public void Test_IfEmptyOnLoad_Empty()
        {
            int result = Cuisine.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_IfNameIsEqual_Equal()
        {
            Cuisine first = new Cuisine("Mexican");
            Cuisine second = new Cuisine("Mexican");

            Assert.Equal(first, second);
        }

        [Fact]
        public void Test_SavesToDatabase_Save()
        {
            Cuisine newCuisine = new Cuisine("Mexican");
            newCuisine.Save();

            List<Cuisine> result = Cuisine.GetAll();
            List<Cuisine> testResult = new List<Cuisine>{newCuisine};

            Assert.Equal(testResult, result);
        }

        [Fact]
        public void Test_FindSearchedCuisine_Find()
        {
            Cuisine newCuisine = new Cuisine("Mexican");
            newCuisine.Save();

            Cuisine foundCuisine = Cuisine.Find(newCuisine.GetId());

            Assert.Equal(newCuisine, foundCuisine);
        }
    }
}
