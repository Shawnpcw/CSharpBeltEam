using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private contextContext dbContext;
        public HomeController(contextContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("createUser")]
        public IActionResult createUser(User newUser)
        {
            LoginUser newLogin = new LoginUser();
            ViewBag.login = newLogin;
            if (ModelState.IsValid)
            {
                if (dbContext.users.Any(u => u.email == newUser.email))
                {
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                System.Console.WriteLine(newUser.password.ToString());
                System.Console.WriteLine("--------------------------------------------------------------------------");
                dbContext.users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("id", newUser.userid);
                System.Console.WriteLine(newUser.userid);
                return RedirectToAction("Quotes");
                // System.Console.WriteLine("*******************************************************************");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            LoginUser newLogin = new LoginUser();
            ViewBag.login = newLogin;
            ViewBag.error = "";
            return View("Index");
        }
        [HttpPost("loginaction")]
        public IActionResult LoginAction(LoginUser userSubmission)
        {
            LoginUser newLogin = new LoginUser();
            ViewBag.login = newLogin;

            if (ModelState.IsValid)
            {

                System.Console.WriteLine(userSubmission.email2);

                var userInDb = dbContext.users.FirstOrDefault(u => u.email == userSubmission.email2);
                if (userInDb == null)
                {
                    ViewBag.error = "UserName is not in DB";
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.password, userSubmission.password);
                if (result == 0)
                {
                    ViewBag.error = "Password Does not match";
                    return View("Index");
                }
                HttpContext.Session.SetInt32("id", userInDb.userid);
                return RedirectToAction("Quotes");
            }
            return View("Index");

        }
        [HttpGet("Quotes")]
        public IActionResult Quotes()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index");

            }
            int userid = (int)HttpContext.Session.GetInt32("id");
            User user = dbContext.users.SingleOrDefault(d => d.userid == (int)userid);
            ViewBag.user = user;
            ViewBag.allPosts = dbContext.posts.Include(p => p.poster).Include(p => p.Likes).OrderByDescending(p => p.Likes.Count).ToList();

            return View();
        }

        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost("Quote/makePost")]
        public IActionResult makePost(Post newPost)
        {

            if (ModelState.IsValid)
            {
                //create
                newPost.posterid = (int)HttpContext.Session.GetInt32("id");
                dbContext.posts.Add(newPost);
                dbContext.SaveChanges();

                return RedirectToAction("Quotes");
            }
            return View(newPost);
        }
        [HttpGet("like/{postid}/{userid}")]
        public IActionResult makeLike(int postid, int userid)
        {
            Like newlike = new Like();
            newlike.postid = postid;
            newlike.userid = userid;
            dbContext.likes.Add(newlike);
            dbContext.SaveChanges();
            return RedirectToAction("Quotes");
        }
        [HttpGet("delete/{postid}")]
        public IActionResult deletePost(int postid)
        {

            var post = dbContext.posts.SingleOrDefault(p => p.postid == postid);
            dbContext.posts.Remove(post);
            dbContext.SaveChanges();
            return RedirectToAction("Quotes");
        }
        [HttpGet("users/{userid}")]
        public IActionResult selectUser(int userid)
        {

            var user = dbContext.users.SingleOrDefault(p => p.userid == userid);
            ViewBag.user = user;
            var posts = dbContext.posts.Where(o => o.posterid == userid).ToList();
            var likes = dbContext.likes.Where(o => o.userid == userid).ToList();
            ViewBag.posts = posts.Count;
            ViewBag.likes = likes.Count;
            return View();
        }
        [HttpGet("Quotes/{postid}")]
        public IActionResult selectPost(int postid)
        {
            var post = dbContext.posts.Include(p => p.poster).SingleOrDefault(d => d.postid == postid);
            ViewBag.post = post;
            var likes = dbContext.likes.Include(p => p.User).ToList();
            Dictionary<int, User> usersDict = new Dictionary<int, User>();

            foreach (var like in likes)
            {
                if (like.postid == postid)
                {
                    if (usersDict.ContainsKey(like.User.userid) == false)
                    {
                        usersDict.Add(like.User.userid, like.User);
                    }
                }
            }
            ViewBag.users = usersDict;
            return View();
        }

    }
}
