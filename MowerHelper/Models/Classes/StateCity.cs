using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MowerHelper.Models.BLL.Common;
namespace MowerHelper.Models.Classes
{
    public class StateCity
    {

        private IEnumerable<SelectListItem> _CountryList;
        public IEnumerable<SelectListItem> CountryList
        {
            get { return _CountryList; }
            set { _CountryList = value; }
        }
        private IEnumerable<SelectListItem> _StateList;
        public IEnumerable<SelectListItem> StateList
        {
            get { return _StateList; }
            set { _StateList = value; }
        }
        private IEnumerable<SelectListItem> _Techlist;
        public IEnumerable<SelectListItem> Techlist
        {
            get { return _Techlist; }
            set { _Techlist = value; }
        }
        private IEnumerable<SelectListItem> _inslist;
        public IEnumerable<SelectListItem> inslist
        {
            get { return _inslist; }
            set { _inslist = value; }
        }
        private IEnumerable<SelectListItem> _RoleList;
        public IEnumerable<SelectListItem> RoleList
        {
            get { return _RoleList; }
            set { _RoleList = value; }
        }
        private IEnumerable<SelectListItem> _CategoryList;
        public IEnumerable<SelectListItem> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; }
        }
        private IEnumerable<SelectListItem> _CityList;
        public IEnumerable<SelectListItem> CityList
        {
            get { return _CityList; }
            set { _CityList = value; }
        }
       // public IList<SearchPayer> PayerList { get; set; }
    }
}