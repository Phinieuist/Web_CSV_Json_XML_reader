using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Web_CSV_Json_XML_reader.Models
{
    public static class ViewResultCreator
    {
        public static ViewResult Create(string pathToView)
        {
            ViewResult viewResult = new ViewResult();
            viewResult.ViewName = pathToView;
            return viewResult;
        }

        public static ViewResult Create(string pathToView, object model)
        {
            ViewResult viewResult = Create(pathToView);

            viewResult.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            return viewResult;
        }

        public static ViewResult Error(string message)
        {
            return Create("~/Views/Home/_Error.cshtml", message);
        }

        public static ViewResult Error(Exception exception)
        {
            string message = string.Concat(exception.GetType().ToString(), ": ", exception.Message, " | ");
            if (exception.InnerException is not null)
                message = GetNextErrorMessage(exception.InnerException, message);

            return Error(message);
        }

        public static string GetNextErrorMessage(Exception exception, string prevMessage)
        {
            prevMessage += string.Concat(exception.GetType().ToString(), ": ", exception.Message, " | ");
            if (exception.InnerException is not null)
                prevMessage = GetNextErrorMessage(exception.InnerException, prevMessage);

            return prevMessage;
        }
    }
}
