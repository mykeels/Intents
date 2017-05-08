using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intents
{
    [Serializable]
    public class IntentManager
    {
        private static IntentManager _intentManager;
        private readonly IntentRepositoryContract _intentRepository;
        private List<Intent> _intents { get; set; }
        private IntentManager(IntentRepositoryContract _repository)
        {
            this._intentRepository = _repository;
            this._intents = new List<Intent>();
        }

        public static IntentManager GetIntentManager(IntentRepositoryContract _intentRepo = null)
        {
            if (_intentManager == null)
            {
                _intentManager = new IntentManager(_intentRepo);
            }
            return _intentManager;
        }

        public IntentManager Register<TData>(IntentContract<TData> _intent)
        {
            if (_intent != null && _intent.isValid() && !_intents.Select(intent => intent.trigger + "-" + intent.name).Contains(_intent.trigger + "-" + _intent.name))
            { //make sure no intent exists with same trigger and name

                this._intents.Add(_intent.getIntent());
                this.CreateSessionStorage();
            }
            return this;
        }

        public IntentManager CreateSessionStorage()
        {
            this._intents?.ForEach(_intent =>
            {
                if (this._intentRepository.IsReady() && !this._intentRepository.Exists(_intent.getStorageKey()))
                    this._intentRepository.Add(_intent.getStorageKey(), new List<UserIntent>());
            });
            return this;
        }

        public bool TriggerExists(string triggerName)
        {
            return !string.IsNullOrEmpty(triggerName) && this._intents.Count(intent => string.Equals(intent.trigger, triggerName, StringComparison.CurrentCultureIgnoreCase)) > 0;
        }

        public bool NameExists(string trigger, string name)
        {
            return !String.IsNullOrEmpty(trigger) && !String.IsNullOrEmpty(name) && this._intents.Count(intent => intent.trigger.ToLower() + "-" + intent.name.ToLower() == trigger.ToLower() + "-" + name.ToLower()) > 0;
        }

        public Intent<TData> GetIntent<TData>(string trigger, string name)
        {
            var ret = this._intents.FirstOrDefault(intent => intent.trigger.ToLower() == trigger.ToLower() && intent.name.ToLower() == name.ToLower());
            return ret?.getIntent<TData>();
        }

        public IEnumerable<Intent> GetIntents(string trigger = null)
        {
            return this._intents.Where(intent => String.IsNullOrEmpty(trigger) || String.Equals(intent.trigger, trigger, StringComparison.CurrentCultureIgnoreCase));
        }

        public IntentManager AddIntentData<TData>(UserIntent<TData> _intentData)
        {
            if (_intentData.isValid())
            {
                if (!this._intentRepository.Exists(_intentData.getStorageKey())) throw new Exception($"Intent with key [{_intentData.getStorageKey()}] not registered");
                var intentCategory = this.GetIntents(_intentData.trigger).FirstOrDefault(intent => intent.name.ToLower() == _intentData.name.ToLower());
                intentCategory.beforeAddIntent();
                var sessionIntents = this._intentRepository.Get<List<UserIntent>>(_intentData.getStorageKey());
                sessionIntents.Add(_intentData.getIntent());
                intentCategory.afterAddIntent();
            }
            return this;
        }

        public IntentManager AddIntentData<TData>(string trigger, string name, TData data)
        {
            return this.AddIntentData(new UserIntent<TData>() { data = data, name = name, trigger = trigger });
        }

        public IntentManager Trigger(string trigger)
        {
            this.GetIntents(trigger).ToList().ForEach(intent =>
            {
                if (this._intentRepository.IsReady())
                {
                    var sessionIntents = (List<UserIntent>)this._intentRepository.Get<List<UserIntent>>(intent.getStorageKey());
                    if (sessionIntents != null)
                    {
                        foreach (var sessionIntent in sessionIntents)
                        {
                            ((UserIntent)sessionIntent).execute();
                        }
                    }
                    this._intentRepository.Add(intent.getStorageKey(), new List<UserIntent>());
                }
            });
            return this;
        }

        public IEnumerable<UserIntent> GetUserIntentData(string trigger = null)
        {
            foreach (var intent in this.GetIntents(trigger).ToList())
            {
                var sessionIntents = this._intentRepository.Get<List<UserIntent>>(intent.getStorageKey());
                if (sessionIntents != null)
                {
                    foreach (var sessionIntent in sessionIntents)
                    {
                        yield return ((UserIntent)sessionIntent);
                    }
                }
                else this._intentRepository.Add(intent.getStorageKey(), new List<UserIntent>());
            }
        }

        public void ClearUserIntentData(string trigger = null, string name = null)
        {
            var intents = GetIntents(trigger).Where(intent => name == null || intent.name.ToLower() == name.ToLower());
            if (intents != null)
            {
                foreach (var intent in intents)
                {
                    this._intentRepository.Add(intent.getStorageKey(), new List<UserIntent>());
                }
            }
        }
    }
}
