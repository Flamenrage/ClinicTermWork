using ClinicBusinessLogic.BindingModels;
using ClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusinessLogic.Interfaces
{
    public interface IRequestLogic
    {
        List<RequestViewModel> GetList();

        RequestViewModel GetElement(int id);

        int AddElement(RequestBindingModel model);
    }
}
