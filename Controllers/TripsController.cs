using Airflights.Models;
using FirstApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airflights.Controllers
{
    public class TripsController : Controller
    {
       

        public IActionResult Index(int page = 1, int pageSize = 5, string search = "")
        {
            // Replace this with your actual data retrieval logic.
            List<Trip> allTrips = Repository.AllTrips.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                // Filter the trips based on the search query
                 allTrips = allTrips.Where(trip =>
                    string.IsNullOrEmpty(search) || trip.Destination?.Contains(search, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }


            int totalTrips = allTrips.Count;
            int totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

            List<Trip> tripsOnPage = allTrips
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new TripViewModel
            {
                Trips = tripsOnPage,
                CurrentPage = page,
                TotalPages = totalPages,
                Search = search
            };

            return View(viewModel);
        }

        // HTTP GET VERSION
        public IActionResult Create()
        {
            return View();
        }

        // HTTP POST VERSION  
        [HttpPost]
        public IActionResult Create(Trip trip)
        {
            //if (!ModelState.IsValid)
            //    return View();

            Repository.Create(trip);
            return View("Thanks", trip);
        }

        public IActionResult Update(int ID)
        {
            Trip trip = Repository.AllTrips.FirstOrDefault(e => e.ID == ID);
            return View(trip);
        }


        [HttpPost]
        public IActionResult Update(Trip trip, int ID)
        {
            if (!ModelState.IsValid)
                return View();

            Trip obj = Repository.AllTrips.FirstOrDefault(e => e.ID == ID);
            if (obj != null)
            {
                obj.Destination = trip.Destination;
                obj.TripsNo = trip.TripsNo;
                obj.TripsCount = trip.TripsCount;
                obj.Staff = trip.Staff;
                obj.SeatCount = trip.SeatCount;
                obj.PlaneType = trip.PlaneType;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int ID)
        {
            Trip trip = Repository.AllTrips.FirstOrDefault(e => e.ID == ID);
            Repository.Delete(trip);
            return RedirectToAction("Index");
        }
    }
}
