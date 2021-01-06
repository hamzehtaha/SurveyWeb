﻿using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWebSite.Hubs
{
    public class QuestionHub :Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<QuestionHub>();
            context.Clients.All.DisplayQuestion(); 
        }
    }
}