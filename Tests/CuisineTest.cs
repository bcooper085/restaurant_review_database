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
            Review.DeleteAll();
            Cuisine.DeleteAll();
            Restaurant.DeleteAll();
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

        [Fact]
        public void Test_DeleteIndividualCuisine_Delete()
        {
            Cuisine newCuisine = new Cuisine("Pizza Palace", 1);
            newCuisine.Save();
            Cuisine.DeleteSpecific(newCuisine.GetId());

            List<Cuisine> testResult = new List<Cuisine>{};
            List<Cuisine> result = Cuisine.GetAll();

            Assert.Equal(testResult, result);
        }

        [Fact]
        public void Test_UpdateCuisineName_Update()
        {
            Cuisine newCuisine = new Cuisine("Pizza Palace", 1);
            newCuisine.Save();

            newCuisine.UpdateName("Mexican Palace");

            Assert.Equal("Mexican Palace", newCuisine.GetName());
        }

        [Fact]
        public void Test_FindCuisineByName_FindByName()
        {
            Cuisine testCuisine = new Cuisine("Italian");
            testCuisine.Save();

            Cuisine actualCuisine = Cuisine.FindByName(testCuisine.GetName());

            Assert.Equal(testCuisine, actualCuisine);
        }

        [Fact]
        public void Test_AddCuisineIfNone_Save()
        {
            Cuisine testCuisine = new Cuisine("Mexican");
            testCuisine.Save();
            Cuisine testCuisine2 = new Cuisine("Chinese");
            testCuisine2.Save();
            Cuisine testCuisine3 = new Cuisine("Thai");
            testCuisine3.Save();
            string cuisineInput = "Italian";
            int cuisineId;

            if(Cuisine.FindByName(cuisineInput).GetName() == null)
            {

                Cuisine newCuisine = new Cuisine(cuisineInput);
                newCuisine.Save();
                cuisineId = newCuisine.GetId();
                Console.WriteLine("You made it to if");
            }
            else
            {
                cuisineId = Cuisine.FindByName(cuisineInput).GetId();
                Console.WriteLine("You made it to else");
            }

            Restaurant newRestaurant = new Restaurant("Pizza Palace", cuisineId);
            newRestaurant.Save();

            string expected = "Italian";
            string actual = Cuisine.GetAll()[3].GetName();
            foreach(Cuisine cuisine in Cuisine.GetAll())
            {
                Console.WriteLine(cuisine.GetName());
            }

            Assert.Equal(expected, actual);
        }

    }
}
