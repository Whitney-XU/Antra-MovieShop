﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieShopMVC.Models;
using ApplicationCore.ServiceInterface;
using Infrastructure.Services;

namespace MovieShopMVC.Controllers
{
    public class HomeController : Controller
    {
        //Each and every request in MVC controller
        //localhost/jome/index

        //1.*** Constructor Injection
        //2. Method Injection
        //3. Property Injection
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        public HomeController(IMovieService movieService,IGenreService genreService)
        {
            _movieService = movieService;
            _genreService = genreService;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public async Task<IActionResult> Index()
        {
            //var movies =await _movieService.GetTopRevenueMovies();
            //var myTYpe = movies.GetType();
            //3 ways to send the data from Controller/action to View
            //1. *** Models (strong typed models)
            //2. ViewBag
            //3. ViewData
            //ViewBag.MovieCount = movies.Count();
            var movie = await _movieService.GetTopRevenueMovies();
            return View(movie);
        }

        public async Task<IActionResult> Genre(int gid)
        {
            var genre = await _genreService.GetGenreDetails(gid);
            return View("~/Views/Home/GenreMovies.cshtml",genre);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
