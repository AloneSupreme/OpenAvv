using Microsoft.AspNetCore.Identity;
using OpenAvv.Data.Models;
using OpenAvv.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using OpenAvv.Data.Models.LikeSystem;
using OpenAvv.Data.Models.CommentSystem;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using OpenAvv.Data.Models.ImageSystem;

namespace OpenAvv.Data
{
    public class OpenAvvRepository : IOpenAvvRepository
    {
        private readonly OpenAvvDbContext _context;
        private readonly UserManager<OpenAvvUser> _userManager;

        public OpenAvvRepository(OpenAvvDbContext ctx, UserManager<OpenAvvUser> userManager)
        {
            _context = ctx;
            _userManager = userManager;
        }

        public IEnumerable<StoryViewModel> GetAllStories() //Returns list of stories
        {
            var records = (from story in _context.Stories
                         join user in _context.Users
                         on story.AuthorId equals user.Id
                         select new
                         {
                             ID = story.Id,
                             Title = story.Title,
                             AuthorName = user.FirstName +" "+ user.LastName,
                             AuthorId = user.Id,
                             Info = story.Description,
                             Description = story.Body,
                             DateCreated = story.DateCreated,
                             DateModified = story.DateModified,
                             NetLikeCount = story.NetLikeCount
                         }).ToList();
            List<StoryViewModel> ListOfStoryVMs = new List<StoryViewModel>();
            foreach (var item in records) //retrieve each item and assign to model
            {
                ListOfStoryVMs.Add(new StoryViewModel()
                {
                    Id = item.ID,
                    Title = item.Title,
                    AuthorName = item.AuthorName,
                    AuthorId = item.AuthorId,
                    Description = item.Info,
                    Body = item.Description,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    NetLikeCount = item.NetLikeCount
                });
            }
            return ListOfStoryVMs;
        }

        public IEnumerable<StoryViewModel> GetAllStories(string authorId) //Returns list of stories with AuthorID
        {
            var records = (from story in _context.Stories
                           join user in _context.Users
                           on story.AuthorId equals user.Id
                           where story.AuthorId.Equals(authorId)
                           select new
                           {
                               ID = story.Id,
                               Title = story.Title,
                               AuthorName = user.FirstName + " " + user.LastName,
                               AuthorId = user.Id,
                               Info = story.Description,
                               Description = story.Body,
                               DateCreated = story.DateCreated,
                               DateModified = story.DateModified,
                               NetLikeCount = story.NetLikeCount
                           }).ToList();
            List<StoryViewModel> ListOfStoryVMs = new List<StoryViewModel>();
            foreach (var item in records) //retrieve each item and assign to model
            {
                ListOfStoryVMs.Add(new StoryViewModel()
                {
                    Id = item.ID,
                    Title = item.Title,
                    AuthorName = item.AuthorName,
                    AuthorId = item.AuthorId,
                    Description = item.Info,
                    Body = item.Description,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    NetLikeCount = item.NetLikeCount
                });
            }
            return ListOfStoryVMs;
        }

        public void AddNewStory(Story story)
        {
            _context.Stories.Add(story);
            _context.SaveChanges();
        }

        public StoryViewModel GetStoryByStoryId(string storyId)
        {
            //retrive that record
            //Story record = (from story in _ctx.Stories
            //              where story.ID == storyId
            //              select story).First();
            //return record;
            var record = (from story in _context.Stories
                          join user in _context.Users
                          on story.AuthorId equals user.Id
                          where story.Id.Equals(storyId)
                          select new
                          {
                              ID = story.Id,
                              Title = story.Title,
                              AuthorName = user.FirstName + " " + user.LastName,
                              AuthorId = user.Id,
                              AuthorDescription = user.Description,
                              Info = story.Description,
                              Description = story.Body,
                              DateCreated = story.DateCreated,
                              DateModified = story.DateModified
                          }).First();
            //retrieve item and assign to model
            StoryViewModel storyViewModel = new StoryViewModel()
            {
                Id = record.ID,
                Title = record.Title,
                AuthorName = record.AuthorName,
                AuthorId = record.AuthorId,
                AuthorDescription = record.AuthorDescription,
                Description = record.Info,
                Body = record.Description,
                DateCreated = record.DateCreated,
                DateModified = record.DateModified,
                LikeCount = LikeDislikeCount("postlike",record.ID),
                DislikeCount = LikeDislikeCount("postdislike", record.ID)
            };

            return storyViewModel;
        }

        public void UpdateStory(Story updatedStory)
        {
            //Updating the record
            Story record = (from story in _context.Stories
                          where story.Id.Equals(updatedStory.Id)
                          select story).First();
            record.Description = updatedStory.Description;
            record.Title = updatedStory.Title;
            record.Body = updatedStory.Body;
            record.DateModified = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteStoryByStoryId(string storyId)
        {
            Story record = (from story in _context.Stories
                          where story.Id.Equals(storyId)
                          select story).First();
            _context.Stories.Remove(record);
            _context.SaveChanges();
        }

        public bool IsAuthor(ClaimsPrincipal signedInUser, string storyId)
        {
            var result = (from story in _context.Stories
                           join user in _context.Users
                           on story.AuthorId equals user.Id
                           where story.Id.Equals(storyId)
                           select new {
                               AuthorId = user.Id
                           }).First();
            string AuthorId = result.AuthorId;

            string SignedInUserId = _userManager.GetUserId(signedInUser);
            return (AuthorId.Equals(SignedInUserId));
        }

        public int LikeDislikeCount(string likeType, string id)
        {
            switch (likeType)
            {
                case "postlike":
                    return _context.PostLikes.Where(p => p.StoryId == id && p.Like == true).Count();
                case "postdislike":
                    return _context.PostLikes.Where(p => p.StoryId == id && p.Dislike == true).Count();
                case "commentlike":
                    return _context.CommentLikes.Where(p => p.CommentId == id && p.Like == true).Count();
                case "commentdislike":
                    return _context.CommentLikes.Where(p => p.CommentId == id && p.Dislike == true).Count();
                case "replylike":
                    return _context.ReplyLikes.Where(p => p.ReplyId == id && p.Like == true).Count();
                case "replydislike":
                    return _context.ReplyLikes.Where(p => p.ReplyId == id && p.Dislike == true).Count();
                default:
                    return 0;
            }
        }
        /// <summary>
        /// "likeType" is post, comment or reply.
        /// "id" here is story's, comment's or reply's ID. 
        /// This is for the indication of Like and Dislike button pressed by LoggedIn User.
        /// </summary>
        public string LikeOrDislike(string likeType, string id, string userid) 
        {
            bool like= false;
            bool dislike = false;
            switch (likeType)
            {
                case "post":
                    like = (_context.PostLikes.AsNoTracking().Where(p => p.StoryId.Equals(id) && p.Like == true && p.UserId.Equals(userid)).Count() == 0) ? false : true;
                    dislike = (_context.PostLikes.AsNoTracking().Where(p => p.StoryId.Equals(id) && p.Dislike == true && p.UserId.Equals(userid)).Count() == 0) ? false : true;
                    //return _context.PostLikes.Where(p => p.StoryId.Equals(id) && p.Like == true && p.UserId.Equals(userid)).Count() == 0 ? "Dislike": "Like";
                    return Help1(like, dislike);
                case "comment":
                    like = (_context.CommentLikes.AsNoTracking().Where(p => p.CommentId.Equals(id) && p.Like == true && p.UserId.Equals(userid)).Count() == 0) ? false : true;
                    dislike = (_context.CommentLikes.AsNoTracking().Where(p => p.CommentId.Equals(id) && p.Dislike == true && p.UserId.Equals(userid)).Count() == 0) ? false : true;
                    return Help1(like, dislike);
                case "reply":
                    like = (_context.ReplyLikes.AsNoTracking().Where(p => p.ReplyId.Equals(id) && p.Like == true && p.UserId.Equals(userid)).Count() == 0) ? false : true;
                    dislike = (_context.ReplyLikes.AsNoTracking().Where(p => p.ReplyId.Equals(id) && p.Dislike == true && p.UserId.Equals(userid)).Count() == 0) ? false : true;
                    return Help1(like, dislike);
                default:
                    return "None";
            }
        }
        string Help1(bool like, bool dislike)
        {
            if (like == true) return "Like";
            else if (dislike == true) return "Dislike";
            else return "None";
        }

        public void UpdatePostLike(string postId, string userid, string likeOrDislike)
        {
            var postLike = _context.PostLikes.Where(x => x.UserId == userid && x.StoryId == postId).FirstOrDefault();
            if (postLike != null)
            {
                switch (likeOrDislike)
                {
                    case "like":
                        if (postLike.Like == false) { postLike.Like = true; postLike.Dislike = false; }
                        else postLike.Like = false;
                        break;
                    case "dislike":
                        if (postLike.Dislike == false) { postLike.Dislike = true; postLike.Like = false; }
                        else postLike.Dislike = false;
                        break;
                }
                if (postLike.Like == false && postLike.Dislike == false) _context.PostLikes.Remove(postLike);
            }
            else
            {
                switch (likeOrDislike)
                {
                    case "like":
                        postLike = new PostLike() { StoryId = postId, UserId = userid, Like = true, Dislike = false };
                        _context.PostLikes.Add(postLike);
                        break;
                    case "dislike":
                        postLike = new PostLike() { StoryId = postId, UserId = userid, Like = false, Dislike = true };
                        _context.PostLikes.Add(postLike);
                        break;
                }
            }
            var post = _context.Stories.Where(x => x.Id == postId).FirstOrDefault();
            post.NetLikeCount = LikeDislikeCount("postlike", postId) - LikeDislikeCount("postdislike", postId);
            _context.SaveChanges();
        }

        public IList<CommentViewModel> GetPostComments(string postId)
        {
            IList<CommentViewModel> records = (from comment in _context.Comments
                                               join user in _context.Users
                                               on comment.UserId equals user.Id
                                               //where comment.StoryId.Equals(postId)
                                               where (comment.StoryId.Equals(postId) && comment.Deleted.Equals(false))
                                               //orderby comment.LikeCount descending
                                               select new CommentViewModel
                                               {
                                                   Id = comment.Id,
                                                   Body = comment.Body,
                                                   DateCreated = comment.DateCreated,
                                                   StoryId = comment.StoryId,
                                                   CommenterId = comment.UserId,
                                                   LikeCount = comment.LikeCount,
                                                   DislikeCount = comment.DislikeCount,
                                                   CommenterName = user.FirstName + " " + user.LastName,
                                                   //Replies = comment.Replies,
                                                   //CommentLikes = comment.CommentLikes
                                               }).ToList();
            return records;
        }
        public IList<CommentViewModel> GetReplies(string commentId)
        {
            IList<CommentViewModel> records = (from reply in _context.Replies
                                               join user in _context.Users
                                               on reply.UserId equals user.Id
                                               //where comment.StoryId.Equals(postId)
                                               where (reply.CommentId.Equals(commentId) && reply.Deleted.Equals(false))
                                               select new CommentViewModel
                                               {
                                                   Id = reply.Id,
                                                   Body = reply.Body,
                                                   DateCreated = reply.DateCreated,
                                                   StoryId = reply.StoryId,
                                                   CommenterId = reply.UserId,
                                                   LikeCount = reply.LikeCount,
                                                   DislikeCount = reply.DislikeCount,
                                                   CommenterName = user.FirstName + " " + user.LastName,
                                                   //Replies = comment.Replies,
                                                   //ReplyLikes = reply.ReplyLikes
                                               }).ToList();
            return records;
        }

        //public List<CommentViewModel> GetReplies(CommentViewModel comment)
        //{
        //    var replies = _context.Replies.Where(p => p.CommentId == comment.Id).ToList();
        //    List<CommentViewModel> cmtReplies = new List<CommentViewModel>();
        //    foreach (var reply in replies)
        //    {
        //        var chReplies = GetChildReplies(reply);
        //        cmtReplies.Add(new CommentViewModel() { Body = reply.Body, ParentReplyId = reply.ParentReplyId, DateCreated = reply.DateCreated, Id = reply.Id, CommenterId = reply.User.Id, ChildReplies = chReplies });
        //    }
        //    return replies;
        //}

        //public List<CommentViewModel> GetChildReplies(Reply parentReply)
        //{
        //    List<CommentViewModel> chldReplies = new List<CommentViewModel>();
        //    if (parentReply != null)
        //    {
        //        var childReplies = _context.Replies.Where(p => p.ParentReplyId == parentReply.Id).ToList();
        //        foreach (var reply in childReplies)
        //        {
        //            var chReplies = GetChildReplies(reply);
        //            chldReplies.Add(new CommentViewModel() { Body = reply.Body, ParentReplyId = reply.ParentReplyId, DateCreated = reply.DateCreated, Id = reply.Id, CommenterId = reply.User.Id, ChildReplies = chReplies });
        //        }
        //    }
        //    return chldReplies;
        //}

        //public CommentViewModel GetReplyById(string id)
        //{
        //    var record = _context.Replies.Where(p => p.Id == id).FirstOrDefault();

        //    return 
        //}


        public bool CommentDeleteCheck(string commentid)
        {
            return _context.Comments.Where(x => x.Id == commentid).Select(x => x.Deleted).FirstOrDefault();
        }
        public bool ReplyDeleteCheck(string replyid)
        {
            return _context.Replies.Where(x => x.Id == replyid).Select(x => x.Deleted).FirstOrDefault();
        }


        public Comment UpdateCommentLike(string commentid, string userId, string likeordislike)
        {
            var commentLike = _context.CommentLikes.Where(x => x.UserId == userId && x.CommentId == commentid).FirstOrDefault();
            if (commentLike != null)
            {
                switch (likeordislike)
                {
                    case "like":
                        if (commentLike.Like == false) { commentLike.Like = true; commentLike.Dislike = false; }
                        else commentLike.Like = false;
                        break;
                    case "dislike":
                        if (commentLike.Dislike == false) { commentLike.Dislike = true; commentLike.Like = false; }
                        else commentLike.Dislike = false;
                        break;
                }
                if (commentLike.Like == false && commentLike.Dislike == false) _context.CommentLikes.Remove(commentLike);
            }
            else
            {
                switch (likeordislike)
                {
                    case "like":
                        commentLike = new CommentLike() { CommentId = commentid, UserId = userId, Like = true, Dislike = false };
                        _context.CommentLikes.Add(commentLike);
                        break;
                    case "dislike":
                        commentLike = new CommentLike() { CommentId = commentid, UserId = userId, Like = false, Dislike = true };
                        _context.CommentLikes.Add(commentLike);
                        break;
                }
            }
            var comment = _context.Comments.Where(x => x.Id == commentid).FirstOrDefault();
            _context.SaveChanges();
            comment.LikeCount = LikeDislikeCount("commentlike", commentid);
            comment.DislikeCount = LikeDislikeCount("commentdislike", commentid);
            
            return comment;
        }

        public void UpdateReplyLike(string replyid, string userid, string likeordislike)
        {
            var replyLike = _context.ReplyLikes.Where(x => x.UserId == userid && x.ReplyId == replyid).FirstOrDefault();
            if (replyLike != null)
            {
                switch (likeordislike)
                {
                    case "like":
                        if (replyLike.Like == false) { replyLike.Like = true; replyLike.Dislike = false; }
                        else replyLike.Like = false;
                        break;
                    case "dislike":
                        if (replyLike.Dislike == false) { replyLike.Dislike = true; replyLike.Like = false; }
                        else replyLike.Dislike = false;
                        break;
                }
                if (replyLike.Like == false && replyLike.Dislike == false) _context.ReplyLikes.Remove(replyLike);
            }
            else
            {
                switch (likeordislike)
                {
                    case "like":
                        replyLike = new ReplyLike() { ReplyId = replyid, UserId = userid, Like = true, Dislike = false };
                        _context.ReplyLikes.Add(replyLike);
                        break;
                    case "dislike":
                        replyLike = new ReplyLike() { ReplyId = replyid, UserId = userid, Like = false, Dislike = true };
                        _context.ReplyLikes.Add(replyLike);
                        break;
                }
            }
            _context.SaveChanges();
        }


        public Story GetPostByReply(string replyid)
        {
            var postid = _context.Replies.Where(x => x.Id == replyid).Select(x => x.StoryId).FirstOrDefault();
            return _context.Stories.Where(x => x.Id == postid).FirstOrDefault();
        }

        public IList<Comment> GetComments()
        {
            return _context.Comments.ToList();
        }
        public IList<Reply> GetReplies()
        {
            return _context.Replies.ToList();
        }
        public void AddNewComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
        public void AddNewReply(Reply reply)
        {
            _context.Replies.Add(reply);
            _context.SaveChanges();
        }



        public Comment GetCommentById(string id)
        {
            return _context.Comments.Where(p => p.Id == id).FirstOrDefault();
        }

        public void DeleteComment(string commentid)
        {
            var comment = _context.Comments.Where(x => x.Id == commentid).FirstOrDefault();
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
        public void DeleteReply(string replyid)
        {
            var reply = _context.Replies.Where(x => x.Id == replyid).FirstOrDefault();
            _context.Replies.Remove(reply);
            _context.SaveChanges();
        }

        public void AddAvatar(Image avatar, string userId)
        {
            _context.Images.Add(avatar);
            OpenAvvUser user = _context.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
            user.Avatar = avatar;
            _context.SaveChanges();
            //return avatar;
        }

        public string GetAvatarPath(string userId)
        {
            Image avatar = _context.Users.Where(x => x.Id.Equals(userId)).Select(y => y.Avatar).FirstOrDefault();
            return _context.Images.Where(x => x.Equals(avatar)).Select(z => z.Path).FirstOrDefault();
        }

    }
}