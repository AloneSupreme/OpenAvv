using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenAvv.Data.Models;
using OpenAvv.Data;
using OpenAvv.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OpenAvv.Data.Models.CommentSystem;

namespace OpenAvv.Controllers
{
    [Authorize]
    public class StoriesController : Controller
    {
        private readonly IOpenAvvRepository _repository;
        private readonly UserManager<OpenAvvUser> _userManager;

        public StoriesController(IOpenAvvRepository repository, UserManager<OpenAvvUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<StoryViewModel> model = _repository.GetAllStories();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [ActionName("Stories")]
        public IActionResult Stories(string id)
        {
            IEnumerable<StoryViewModel> model = _repository.GetAllStories(id);
            return View("Index",model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Story formStoryModel)
        {
            if (ModelState.IsValid)
            {
                formStoryModel.AuthorId = _userManager.GetUserId(User);
                formStoryModel.DateCreated = DateTime.Now;

                _repository.AddNewStory(formStoryModel);

                ViewData["Message"] = "Success";
                ModelState.Clear();
            }
            else
            {
                ViewData["Message"] = "Error";
            }
            return View();
        }

        
        [HttpGet]
        public IActionResult Edit(string id)
        {

            if (_repository.IsAuthor(User, id)) { //Checking whether the author have permission to edit the story
                StoryViewModel model = _repository.GetStoryByStoryId(id);
                return View(model);
            }
            ViewData["AllowAuthorError"] = "You are not allowed to access that URL"; //Otherwise returning to Index page
            IEnumerable<StoryViewModel> IndexModel = _repository.GetAllStories();
            return View("Index",IndexModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Story updatedStory)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateStory(updatedStory);
                ViewData["Message"] = "Success";
                ModelState.Clear();
            }
            else
            {
                ViewData["Message"] = "Error";
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (_repository.IsAuthor(User, id)) //Checking whether the author have permission to edit the story
            {
                _repository.DeleteStoryByStoryId(id);
                return RedirectToAction("Index");
            }
            ViewData["AllowAuthorError"] = "You are not allowed to access that URL";//Otherwise returning to Index page
            IEnumerable<StoryViewModel> IndexModel = _repository.GetAllStories();
            return View("Index", IndexModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(string id)
        {
            StoryViewModel model = _repository.GetStoryByStoryId(id);
            IList<CommentViewModel> postComments = _repository.GetPostComments(id).ToList();

            foreach (var comment in postComments)
            {
                comment.LikeCount = _repository.LikeDislikeCount("commentlike", comment.Id);
                comment.DislikeCount = _repository.LikeDislikeCount("commentdislike", comment.Id);
                comment.LikeOrDislike = _repository.LikeOrDislike("comment", comment.Id, _userManager.GetUserId(User));
                //comment.NetLikeCount = likes - dislikes;
                //if (comment.Replies != null) comment.Replies.Clear();
                IList<CommentViewModel> replies = _repository.GetReplies(comment.Id);
                comment.Replies = replies;
                //foreach (var reply in replies)
                //{
                //    var rep = _repository.GetReplyById(reply.Id);
                //    comment.Replies.Add(rep);
                //}
                comment.ReplyCount = replies.Count;
            }
            postComments = postComments.OrderByDescending(x => x.LikeCount+ x.DislikeCount).ToList();
            model.Comments = postComments;
            //var post = _repository.GetStoryByStoryId(id);
            //Comments(post);
            return View(model);   
        }

        
        //public ActionResult Comments(StoryViewModel post)
        //{
        //    //ViewBag.CurrentSort = sortOrder;
        //    //ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
        //    //ViewBag.BestSortParm = sortOrder == "Best" ? "best_desc" : "Best";

        //    var postComments = _repository.GetPostComments(post.Id).OrderByDescending(d => d.DateCreated).ToList();

        //    foreach (var comment in postComments)
        //    {
        //        var likes = _repository.LikeDislikeCount("commentlike", comment.Id);
        //        var dislikes = _repository.LikeDislikeCount("commentdislike", comment.Id);
        //        comment.NetLikeCount = likes - dislikes;
        //        if (comment.Replies != null) comment.Replies.Clear();
        //        List<CommentViewModel> replies = _repository.GetParentReplies(comment);
        //        foreach (var reply in replies)
        //        {
        //            var rep = _repository.GetReplyById(reply.Id);
        //            comment.Replies.Add(rep);
        //        }
        //    }
        //    postComments = postComments.OrderBy(x => x.DateCreated).ToList();

        //    return PartialView(postComments);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewComment(string commentBody, string postid)
        {
            List<int> numlist = new List<int>();
            int num = 0;
            var comments = _repository.GetComments().ToList();
            if (comments.Count() != 0)
            {
                foreach (var cmnt in comments)
                {
                    var comid = cmnt.Id;
                    Int32.TryParse(comid.Replace("cmt", ""), out num);
                    numlist.Add(num);
                }
                numlist.Sort();
                num = numlist.Last();
                num++;
            }
            else
            {
                num = 1;
            }
            var newid = "cmt" + num.ToString();
            var comment = new Comment()
            {
                Id = newid,
                StoryId = postid,
                DateCreated = DateTime.Now,
                UserId = _userManager.GetUserId(User),
                Body = commentBody,
                LikeCount = 0,
                DislikeCount = 0
            };
            _repository.AddNewComment(comment);
            return RedirectToAction("Details", new { id = postid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewReply(string replyBody, string postid, string commentid)
        {
            //List<int> numlist = new List<int>();
            //int num = 0;
            //var comments = _repository.GetComments().ToList();
            //if (comments.Count() != 0)
            //{
            //    foreach (var cmnt in comments)
            //    {
            //        var comid = cmnt.Id;
            //        Int32.TryParse(comid.Replace("cmt", ""), out num);
            //        numlist.Add(num);
            //    }
            //    numlist.Sort();
            //    num = numlist.Last();
            //    num++;
            //}
            //else
            //{
            //    num = 1;
            //}
            //var newid = "cmt" + num.ToString();
            var reply = new Reply()
            {
                //Id = newid,
                StoryId = postid,
                CommentId = commentid,
                DateCreated = DateTime.Now,
                UserId = _userManager.GetUserId(User),
                Body = replyBody,
                LikeCount = 0,
                DislikeCount = 0
            };
            _repository.AddNewReply(reply);
            return RedirectToAction("Details", new { id = postid });
        }
        [HttpPost]
        public IActionResult UpdateCommentLike(UpdateCommentLikeModel obj)
        {
            
            Comment comment = _repository.UpdateCommentLike(obj.commentid, _userManager.GetUserId(User), obj.likeordislike);
            string likeOrComment = _repository.LikeOrDislike("comment", obj.commentid, _userManager.GetUserId(User));
            Tuple<string, string, int, int> tuple = new Tuple<string, string, int, int>(obj.commentid,likeOrComment, comment.LikeCount, comment.DislikeCount);
            return Json(tuple);//RedirectToAction("Details", new { id = obj.postid });
        }
        public IActionResult LoadReplyPartialView()
        {
            return View(viewName: "_ReplyPartialView");
        }

        public class UpdateCommentLikeModel
        {
            public string commentid { get; set; }
            public string likeordislike { get; set; }
            public string postid { get; set; }

        }

    }
}