using System;
using System.Collections.Generic;
using System.Text;

namespace OneTrack.PM.Entities.DTOs.Security
{
    public class UserPermissions
    {
        private int _moduleId;
        private string _moduleName;
        private string _pageName;
        private string _url;
        private string _parentForm;
        private string _parentUrl;
        private bool _read;
        private bool _create;
        private bool _update;
        private bool _delete;
        private bool _export;
        private bool _print;
        private bool _approve;
        private bool _freeze;

        #region Public Variables
        public int ModuleId
        {
            get { return _moduleId; }
            set { _moduleId = value; }
        }
        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }
        public string PageName
        {
            get { return _pageName; }
            set { _pageName = value; }
        }
        public string ParentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }
        public string ParentUrl
        {
            get { return _parentUrl; }
            set { _parentUrl = value; }
        }
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public bool Read
        {
            get { return _read; }
            set { _read = value; }
        }
        public bool Create
        {
            get { return _create; }
            set { _create = value; }
        }
        public bool Update
        {
            get { return _update; }
            set { _update = value; }
        }
        public bool Delete
        {
            get { return _delete; }
            set { _delete = value; }
        }
        public bool Export
        {
            get { return _export; }
            set { _export = value; }
        }
        public bool Print
        {
            get { return _print; }
            set { _print = value; }
        }
        public bool Approve
        {
            get { return _approve; }
            set { _approve = value; }
        }
        public bool Freeze
        {
            get { return _freeze; }
            set { _freeze = value; }
        }
        #endregion
        public UserPermissions(int modaulId, string moduleName, string pageName, string url, string parentForm, string parentUrl, bool read, bool create, bool update, bool delete, bool export, bool print, bool approve, bool freeze)
        {
            _moduleId = modaulId;
            _moduleName = moduleName;
            _pageName = pageName;
            _parentForm = parentForm;
            _parentUrl = parentUrl;
            _url = url;
            _read = read;
            _create = create;
            _update = update;
            _delete = delete;
            _print = print;
            _export = export;
            _approve = approve;
            _freeze = freeze;
        }
        private static string SerializePermissions(UserPermissions up)
        {
            StringBuilder value = new StringBuilder();
            value.Append(up.ModuleId);
            value.Append(";" + up.ModuleName);
            value.Append(";" + up.PageName);
            value.Append(";" + up.Url);
            value.Append(";" + up.ParentForm);
            value.Append(";" + up.ParentUrl);
            value.Append(";" + up.Read);
            value.Append(";" + up.Create);
            value.Append(";" + up.Update);
            value.Append(";" + up.Delete);
            value.Append(";" + up.Export);
            value.Append(";" + up.Print);
            value.Append(";" + up.Approve);
            value.Append(";" + up.Freeze);
            return value.ToString();
        }

        private static UserPermissions DeSerializePermissions(string up)
        {
            string[] details = up.Split(';');
            return new UserPermissions(int.Parse(details[0])
                , details[1]
                , details[2]
                , details[3]
                , details[4]
                , details[5]
                , bool.Parse(details[6])
                , bool.Parse(details[7])
                , bool.Parse(details[8])
                , bool.Parse(details[9])
                , bool.Parse(details[10])
                , bool.Parse(details[11])
                , bool.Parse(details[12])
                , bool.Parse(details[13]));
        }

        public static string SerializePermissionsList(List<UserPermissions> its)
        {
            StringBuilder value = new StringBuilder();
            try
            {
                foreach (UserPermissions item in its)
                {
                    if (value.ToString() == string.Empty)
                    {
                        value.Append(SerializePermissions(item));
                    }
                    else
                    {
                        value.Append("$" + SerializePermissions(item));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return value.ToString();
        }

        public static List<UserPermissions> DeSerializePermissionsList(string its)
        {
            string[] details = its.Split('$');
            List<UserPermissions> items = new List<UserPermissions>();
            foreach (string item in details)
            {
                items.Add(DeSerializePermissions(item));
            }
            return items;
        }
    }
}
