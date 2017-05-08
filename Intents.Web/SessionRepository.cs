using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Intents;

namespace Intents.Web
{
    public class SessionRepository : IntentRepositoryContract
    {
        private HttpContext _currentContext
        {
            get
            {
                return HttpContext.Current;
            }
        }

        public void Add<TData>(string key, TData data)
        {
            if (this.Exists(key)) this._currentContext.Session[key] = data;
            else this._currentContext.Session.Add(key, data);
        }

        public void Clear()
        {
            this._currentContext.Session.Clear();
        }

        public bool Exists(string key)
        {
            return this._currentContext.Session[key] != null;
        }

        public TData Get<TData>(string key)
        {
            return (TData)this._currentContext.Session[key];
        }

        public bool IsReady()
        {
            return this._currentContext != null && this._currentContext.Session != null;
        }

        public void Remove<TData>(string key)
        {
            this._currentContext.Session.Remove(key);
        }
    }
}
