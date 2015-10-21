using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;

using Site.Models;
using Site.Data.API;
using Site.Data.API.Repository;

using Ninject;

namespace Site.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
            if (_errors == null)
                _errors = new List<string>();
        }

        public string GoogleAnalyticsCode
        {
            get
            {
                var code = WebConfigurationManager.AppSettings["GoogleAnalyticsCode"];

                return code;
            }
        }

        private IList<string> _errors;
        public IList<string> Errors
        {
            get
            {
                if (_errors == null)
                    _errors = new List<string>();

                return _errors;
            }
            set { _errors = value; }
        }
    }
}
