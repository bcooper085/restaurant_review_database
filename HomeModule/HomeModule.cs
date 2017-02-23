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

            Patch["/cuisines/{id}/edit"] = parameters => {
              Cuisine newCuisine = Cuisine.Find(parameters.id);
              newCuisine.UpdateName(Request.Form["cuisine_name_edit"]);
              return View["success.cshtml", ModelMaker()];
            };

            Delete["/cuisines/{id}/delete"] = parameters => {
              Cuisine.DeleteSpecific(parameters.id);
              return View["success.cshtml", ModelMaker()];
            };

            Get["/restaurants/{id}"] = parameters => {
                Restaurant newRestaurant = Restaurant.Find(parameters.id);
                Dictionary<string, object> model = ModelMaker();
                model.Add("Restaurant Object", newRestaurant);
                model.Add("Cuisine Object", Cuisine.Find(newRestaurant.GetCuisineId()));
                model.Add("Review Object", Review.GetByRestaurant(newRestaurant.GetId()));
                return View["restaurant.cshtml", model];
            };

            Patch["/restaurants/{id}/edit"] = parameters => {
              Restaurant newRestaurant = Restaurant.Find(parameters.id);
              newRestaurant.UpdateName(Request.Form["restaurant_name_edit"]);

              string cuisineInput = Request.Form["restaurant_cuisine_edit"];
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

              newRestaurant.UpdateCuisine(cuisineId);

              return View["success.cshtml", ModelMaker()];
            };

            Delete["/restaurants/{id}/delete"] = parameters => {
              Restaurant.DeleteSpecific(parameters.id);
              return View["success.cshtml", ModelMaker()];
            };


            Post["/restaurants/{id}/add-review"] = parameters => {
                Review newReview = new Review(Request.Form["reviewer"], Request.Form["review"], parameters.id);
                newReview.Save();
                return View["success.cshtml", ModelMaker()];
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
