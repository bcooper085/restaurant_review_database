using Nancy;
using System.Collections.Generic;

namespace DerpApp
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                Dictionary<string, object> model = new Dictionary<string, object>{};
                model.Add("Cuisines", Cuisine.GetAll());
                model.Add("Restaurants", Restaurant.GetAll());
                model.Add("Reviews", Review.GetAll());

                return View["index.cshtml", ModelMaker()];
            };

            Post["/restaurant-add"] = _ => {
                string cuisineInput = Request.Form["cuisine_input"];
                int cuisineId;
                try
                {
                    Cuisine newCuisine = Cuisine.FindByName(cuisineInput);
                    cuisineId = newCuisine.GetId();
                }
                finally
                {

                    Cuisine newCuisine = new Cuisine(cuisineInput);
                    newCuisine.Save();
                    cuisineId = newCuisine.GetId();
                }

                Restaurant newRestaurant = new Restaurant(Request.Form["restaurant_input"], cuisineId);
                newRestaurant.Save();

                return View["success.cshtml", ModelMaker()];
            };
        }

        public static Dictionary<string, object> ModelMaker()
        {
            Dictionary<string, object> model = new Dictionary<string, object>{};
            model.Add("Cuisines", Cuisine.GetAll());
            model.Add("Restaurants", Restaurant.GetAll());
            model.Add("Reviews", Review.GetAll());

            return model;
        }
    }
}
