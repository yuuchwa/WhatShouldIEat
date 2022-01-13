using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Reddit Client API */
using Reddit;
using Reddit.Controllers;
using WhatShouldIEat.Model;

namespace WhatShouldIEat.Services
{
    interface IRedditClientService
    {
        public List<Post> RequestRecipePosts(Recipe recipe);

        public Comment FindInstructionInPost(string originalPoster, List<Comment> postComments);

        public int GetPosOfWord(string strSouce, string strStart);

        public string GetIngredientsInComment(string strSource);

        public List<Comment> FindCommentsFromOP(string originalPoster, List<Comment> comments);

        public string GetInstructionFromComment(Comment comment);
    }
}
