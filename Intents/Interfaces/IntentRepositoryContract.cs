using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intents
{
    public interface IntentRepositoryContract
    {
        void Add<TData>(string key, TData data);
        TData Get<TData>(string key);
        void Remove<TData>(string key);
        bool Exists(string key);
        void Clear();
        bool IsReady();
    }
}
