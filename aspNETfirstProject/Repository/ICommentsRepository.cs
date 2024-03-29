﻿using aspNETfirstProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace aspNETfirstProject.Repository
{
    public interface ICommentsRepository
    {
        Task<IList<Comment>> GetComments(int ItemId);
        JsonResult AddComment(Comment comment);
        JsonResult DeleteComment(int Id);
    }
}