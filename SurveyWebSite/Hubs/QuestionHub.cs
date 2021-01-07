using Microsoft.AspNet.SignalR;
using SurveyWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWebSite.Hubs
{
    public class questionHub : Hub
    {
        public void refreshData()
        {
            try
            {
                Clients.All.display();
            }catch (Exception ex)
            {
                QuestionModel.Logger.Log(ex.Message);
            }
        }
    }
}