using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationManger;
using Question;
using SurveyWebSite.Models;
using OperationManger;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Data.SqlClient;
using SurveyWebSite.Hubs;

namespace SurveyWebSite.Controllers
{
    public class QuestionController : Controller
    {
        public static FormCollection Form = new FormCollection();
        private static BaseLog.Logger Logger = new BaseLog.Logger();
        public static string  SessionID = "" ;
         
        // GET: Question
        /// <summary>
        /// This Home View get all question from my list in manger to show it
        /// </summary>
        public ActionResult Home()
        {
            try
            {
                string sessionID = HttpContext.Session.SessionID;
                SessionID = sessionID; 
                Operation.SessionFlags.Add(sessionID, false);
                int ResultOfGet = Operation.GetQustion(ref MyListClass.ListOfQuestions); 
                if (ResultOfGet == OperationManger.GenralVariables.Succeeded)
                return View(MyListClass.ListOfQuestions);
                else
                {
                    string Error = Operation.CheckMessageError(ResultOfGet);
                    return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = Error });
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorHome });
            }
        }

        /// <summary>
        /// This for refresh my partail view atfer any edit in list 
        /// </summary>
        /// <returns></returns>
        public ActionResult RefreshData()
        {
            try
            {

                if (Operation.SessionFlags[SessionID])
                {
                    Operation.SessionFlags[SessionID] = false;
                    MyListClass.ListOfQuestions = Operation.ListOfAllQuestion;
                    return PartialView(@SurveyWebSite.Resources.Constants.PartailList, MyListClass.ListOfQuestions);
                }
                else
                    return PartialView(@SurveyWebSite.Resources.Constants.PartailList, MyListClass.ListOfQuestions);

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorRefrsh });
            }
        }
        /// <summary>
        /// This create view for get a question to add it 
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateQuestion(int Type)
        {
            try
            {
                TypeOfQuestion QuestionType = (TypeOfQuestion)Type;
                switch (QuestionType)
                {
                    case TypeOfQuestion.Slider:
                        QuestionModel Slider = new QuestionModel();
                        Slider.TypeOfQuestion = TypeOfQuestion.Slider;
                        return View(Slider);
                    case TypeOfQuestion.Smily:
                        QuestionModel Smile = new QuestionModel();
                        Smile.TypeOfQuestion = TypeOfQuestion.Smily;
                        return View(Smile);
                    case TypeOfQuestion.Stars:
                        QuestionModel Star = new QuestionModel();
                        Star.TypeOfQuestion = TypeOfQuestion.Stars;
                        return View(Star);
                    default:
                        return View(@SurveyWebSite.Resources.Messages.ErrorCreate);
                }


            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorCreate });
            }
        }
        /// <summary>
        /// After enter the data and check of validate then call model binder to build the object 
        /// then add it in database using manger class
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateQuestion(QuestionModel NewQustionModel)
        {
            try
            {
                Qustion NewQuestion = SpecifyTheTypeAndCreateTheQuestion(NewQustionModel);
                if (NewQuestion != null)
                {
                    int ResultOfCheck = Operation.CheckTheData(NewQuestion);
                    if (ResultOfCheck == OperationManger.GenralVariables.Succeeded)
                    {
                        int ResultOfCreate = Operation.AddQustion(NewQuestion);
                        if (ResultOfCreate == OperationManger.GenralVariables.Succeeded)
                        {
                            ModelState.Clear();
                            return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
                        }
                        else
                        {
                            string Error = Operation.CheckMessageError(ResultOfCreate);
                            return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = Error });
                        }
                    }
                    else
                    {
                        ViewBag.Message = Operation.CheckMessageError(ResultOfCheck);
                        NewQustionModel.TypeOfQuestion = NewQuestion.TypeOfQuestion;
                        return View(NewQustionModel);
                    }

                }
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = "Error while create question" });
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorCreate });
            }
        }
        /// <summary>
        /// This function take ID to show the data will delete it
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var QuestionWillDelete = Operation.SelectById(id);
                if (QuestionWillDelete == null)
                {
                    return View(@SurveyWebSite.Resources.Constants.ErrorNotFound);
                }
                return View(QuestionWillDelete);
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorDelete });
            }
        }
        /// <summary>
        /// After user press Yes will go to delete post to delete it from database using mnager
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id ,FormCollection form)
        {
            try
            {
                var QuestionWillDelete = Operation.SelectById(id);
                int ResultOfDelete =  Operation.DeleteQustion(QuestionWillDelete);
                if (ResultOfDelete == OperationManger.GenralVariables.Succeeded)
                {
                    return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
                }
                else
                {
                    string Error = Operation.CheckMessageError(ResultOfDelete);
                    return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = Error });
                }
            }catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorDelete });
            }
        }
        /// <summary>
        /// This get method take id the question to show the data  
        /// </summary>
        public ActionResult EditQuestion(int Id)
        {
            try
            {
                var QuestionWillEdit = Operation.SelectById(Id);
                QuestionModel NewQustionModel = new QuestionModel();
                switch (QuestionWillEdit.TypeOfQuestion)
                {
                    case TypeOfQuestion.Slider:
                        NewQustionModel.TypeOfQuestion = TypeOfQuestion.Slider;
                        Slider QuestionSlider = (Slider)QuestionWillEdit;
                        NewQustionModel.Id = QuestionSlider.Id;
                        NewQustionModel.IdForType = QuestionSlider.IdForType;
                        NewQustionModel.NewText = QuestionSlider.NewText;
                        NewQustionModel.Order = QuestionSlider.Order;
                        NewQustionModel.StartValue = QuestionSlider.StartValue;
                        NewQustionModel.EndValue = QuestionSlider.EndValue;
                        NewQustionModel.StartCaption = QuestionSlider.StartCaption;
                        NewQustionModel.EndCaption = QuestionSlider.EndCaption;
                        break;
                    case TypeOfQuestion.Smily:
                        NewQustionModel.TypeOfQuestion = TypeOfQuestion.Smily;
                        Smiles QuestionSmile = (Smiles)QuestionWillEdit;
                        NewQustionModel.Id = QuestionSmile.Id;
                        NewQustionModel.IdForType = QuestionSmile.IdForType;
                        NewQustionModel.NewText = QuestionSmile.NewText;
                        NewQustionModel.Order = QuestionSmile.Order;
                        NewQustionModel.NumberOfSmiles = QuestionSmile.NumberOfSmiles;
                        break;
                    case TypeOfQuestion.Stars:
                        NewQustionModel.TypeOfQuestion = TypeOfQuestion.Stars;
                        Stars QuestionStar = (Stars)QuestionWillEdit;
                        NewQustionModel.Id = QuestionStar.Id;
                        NewQustionModel.IdForType = QuestionStar.IdForType;
                        NewQustionModel.NewText = QuestionStar.NewText;
                        NewQustionModel.Order = QuestionStar.Order;
                        NewQustionModel.NumberOfStars = QuestionStar.NumberOfStars;
                        break;

                }
                return View(NewQustionModel);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorEdit });
            }

        }
        /// <summary>
        /// Edit post when user press yes will check the validate data then call edit function from manger
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditQuestion(QuestionModel NewQustion)
        {
            try
            {
                Qustion QuestionWillEdit = SpecifyTheTypeAndCreateTheQuestion(NewQustion);
                QuestionWillEdit.Id = NewQustion.Id;
                if (QuestionWillEdit != null)
                {
                    int ResultOfCheck = Operation.CheckTheData(QuestionWillEdit);
                    if (ResultOfCheck == OperationManger.GenralVariables.Succeeded)
                    {
                        int ResultOfCreate = Operation.EditQustion(QuestionWillEdit);
                        if (ResultOfCreate == OperationManger.GenralVariables.Succeeded)
                        {
                            ModelState.Clear();
                            return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
                        }
                        else
                        {
                            string Error = Operation.CheckMessageError(ResultOfCreate);
                            return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = Error });
                        }
                    }
                    else
                    {
                        ViewBag.Message = Operation.CheckMessageError(ResultOfCheck);
                        return View(NewQustion);
                    }

                }
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = "Error while create question" });

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorEdit });
            }

        }
        /// <summary>
        /// To change lanuage and take the language  
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeLanuage (string Language)
        {
            try
            {
                if (!String.IsNullOrEmpty(Language))
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Language);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                }
                HttpCookie cookie = new HttpCookie(SurveyWebSite.Resources.Constants.Languages);
                cookie.Value = Language;
                Response.Cookies.Add(cookie);
                return RedirectToAction(@SurveyWebSite.Resources.Constants.HomeView);
            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return RedirectToAction(SurveyWebSite.Resources.Constants.ErrorView, new { ErrorMessage = SurveyWebSite.Resources.Messages.ErrorChangeLanguage });
            }
        }
        /// <summary>
        /// This errro view take the error and show it in error view any error 
        /// </summary>
        public ActionResult ErrorView (string ErrorMessage)
        {
            try
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View(); 
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return View(); 
            }
        }
        
        
        private Qustion SpecifyTheTypeAndCreateTheQuestion (QuestionModel NewQustion)
        {
            try
            {
                if (NewQustion.StartValue > 0)
                {
                    Slider NewQuestion = new Slider();
                    if (NewQustion.Id > 0 && NewQustion.IdForType > 0)
                    {
                        NewQuestion.Id = NewQustion.Id;
                        NewQuestion.IdForType = NewQustion.IdForType;
                    }
                    NewQuestion.TypeOfQuestion = TypeOfQuestion.Slider;
                    NewQuestion.NewText = NewQustion.NewText;
                    NewQuestion.Order = NewQustion.Order;
                    NewQuestion.StartValue = NewQustion.StartValue;
                    NewQuestion.EndValue = NewQustion.EndValue;
                    NewQuestion.StartCaption = NewQustion.StartCaption;
                    NewQuestion.EndCaption = NewQustion.EndCaption;
                    return NewQuestion;
                }
                else if (NewQustion.NumberOfSmiles > 0)
                {
                    Smiles NewQuestion = new Smiles();
                    if (NewQustion.Id > 0 && NewQustion.IdForType > 0)
                    {
                        NewQuestion.Id = NewQustion.Id;
                        NewQuestion.IdForType = NewQustion.IdForType;
                    }
                    NewQuestion.TypeOfQuestion = TypeOfQuestion.Smily;
                    NewQuestion.NewText = NewQustion.NewText;
                    NewQuestion.Order = NewQustion.Order;
                    NewQuestion.NumberOfSmiles = NewQustion.NumberOfSmiles;
                    return NewQuestion;
                }
                else
                {
                    Stars NewQuestion = new Stars();
                    if (NewQustion.Id > 0 && NewQustion.IdForType > 0)
                    {
                        NewQuestion.Id = NewQustion.Id;
                        NewQuestion.IdForType = NewQustion.IdForType;
                    }
                    NewQuestion.TypeOfQuestion = TypeOfQuestion.Stars;
                    NewQuestion.NewText = NewQustion.NewText;
                    NewQuestion.Order = NewQustion.Order;
                    NewQuestion.NumberOfStars = NewQustion.NumberOfStars;
                    return NewQuestion;
                }
            }catch( Exception ex)
            {
                Logger.Log(ex.Message);
                return null; 
            }

        }

        public ActionResult GetData()
        {
            var ListQuestion = Operation.GetAllQuestion();
            return PartialView("_ListPartail", ListQuestion);
        }
        

    }
}