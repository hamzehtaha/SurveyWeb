using Microsoft.Owin;
using Owin;
using SurveyWebSite.Models;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(SurveyWebSite.Startup))]

namespace SurveyWebSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                app.MapSignalR();
            }catch(Exception ex)
            {
                QuestionModel.Logger.Log(ex.Message);
            }
        }
    }
}
