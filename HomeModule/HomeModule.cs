using Nancy;
using System.Collections.Generic;

namespace DerpApp
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                return View["index.cshtml", ModelMaker()];
            };

            Post["/delete-all"] = _ => {
                    Cuisine.DeleteAll();
                    Restaurant.DeleteAll();
                    Review.DeleteAll();
                    return View["success.cshtml", ModelMaker()];
            };

            Get["/cuisines/{id}"]= parameters => {
                Cuisine newCuisine = Cuisine.Find(parameters.id);
                Dictionary<string, object> model = ModelMaker();
                model.Add("Cuisine Object", newCuisine);
                model.Add("Restaurant List", Restaurant.GetByCuisine(newCuisine.GetId()));
                return View["cuisine.cshtml", model];
            };

            Get["/restaurants/{id}"] = parameters => {
                Restaurant newRestaurant = Restaurant.Find(parameters.id);
                Dictionary<string, object> model = ModelMaker();
                model.Add("Restaurant Object", newRestaurant);
                model.Add("Cuisine Object", Cuisine.Find(newRestaurant.GetCuisineId()));
                return View["restaurant.cshtml", model];
            };

            Post["/restaurant-add"] = _ => {

                string cuisineInput = Request.Form["cuisine_input"];
                int cuisineId;

                if(Cuisine.FindByName(cuisineInput).GetName() == null)
                {

                    Cuisine newCuisine = new Cuisine(cuisineInput);
                    newCuisine.Save();
                    cuisineId = newCuisine.GetId();
                }
                else
                {
                    cuisineId = Cuisine.FindByName(cuisineInput).GetId();
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
