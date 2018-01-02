using System.Collections.Generic;
using OpenAvv.Data.Models;
using OpenAvv.Data.Models.CommentSystem;
using OpenAvv.ViewModels;
using System.Security.Claims;
using OpenAvv.Data.Models.ImageSystem;

namespace OpenAvv.Data
{
    public interface IOpenAvvRepository
    {
        void AddNewStory(Story story);
        void DeleteStoryByStoryId(string storyId);
        IEnumerable<StoryViewModel> GetAllStories();
        IEnumerable<StoryViewModel> GetAllStories(string authorId);
        StoryViewModel GetStoryByStoryId(string storyId);
        void UpdateStory(Story updatedStory);
        bool IsAuthor(ClaimsPrincipal signedInUser, string storyId);


        int LikeDislikeCount(string likeType, string id);
        /// <summary>
        /// "likeType" is post, comment or reply.
        /// "id" here is story's, comment's or reply's ID. 
        /// This is for the indication of Like and Dislike button pressed by LoggedIn User.
        /// </summary>
        string LikeOrDislike(string likeType, string id, string userid);
        void UpdatePostLike(string postId, string username, string likeOrDislike);

        IList<CommentViewModel> GetPostComments(string postId);
        IList<CommentViewModel> GetReplies(string commentId);
        //List<CommentViewModel> GetChildReplies(Reply parentReply);
       // CommentViewModel GetReplyById(string id);
        bool CommentDeleteCheck(string commentid);
        bool ReplyDeleteCheck(string replyid);

        Comment UpdateCommentLike(string commentid, string userId, string likeordislike);
        void UpdateReplyLike(string replyid, string username, string likeordislike);
        Story GetPostByReply(string replyid);

        IList<Comment> GetComments();
        IList<Reply> GetReplies();
        void AddNewComment(Comment comment);
        void AddNewReply(Reply reply);
        Comment GetCommentById(string id);
        void DeleteComment(string commentid);
        void DeleteReply(string replyid);

        void AddAvatar(Image avatar, string userId);
        string GetAvatarPath(string userId);

    }
}