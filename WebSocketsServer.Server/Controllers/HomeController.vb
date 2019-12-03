

Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        ViewBag.Title = "UI"
        Return View()
    End Function

End Class
