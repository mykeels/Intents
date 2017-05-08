using System;

namespace Intents
{
    [Serializable]
    public class UserIntent : Intent
    {
        public new Action<dynamic> action { get; set; }
        public dynamic data { get; set; }

        public void execute()
        {
            if (this.action != null) this.action.Invoke(data);
            //else if (base.action != null) base.action.Invoke(data);
        }

        public new UserIntent<TData> getIntent<TData>()
        {
            UserIntent<TData> intent = new UserIntent<TData>();
            intent.trigger = this.trigger;
            intent.name = this.name;
            intent.action = (data) => this.action((TData)data);
            intent.data = (TData)this.data;
            return intent;
        }

        public new bool isValid() //because actions may not be defined for UserIntentData
        {
            return !String.IsNullOrEmpty(this.name) && !String.IsNullOrEmpty(this.trigger) && (this.data != null);
        }
    }
}
