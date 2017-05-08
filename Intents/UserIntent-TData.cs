using System;

namespace Intents
{
    [Serializable]
    public class UserIntent<TData> : Intent<TData>
    {
        public new Action<TData> action { get; set; }
        public TData data { get; set; }

        public UserIntent()
        {

        }

        public void execute()
        {
            if (this.action != null) this.action.Invoke(data);
        }


        public new UserIntent getIntent()
        {
            UserIntent intent = new UserIntent();
            intent.type = this.GetType();
            if (this.action == null)
                this.action = IntentManager.GetIntentManager().GetIntent<TData>(this.trigger, this.name).action;
            intent.action = (data) =>
            {
                this.action.Invoke((TData)data);
            };
            intent.name = this.name;
            intent.trigger = this.trigger;
            intent.data = this.data;
            return intent;
        }

        public new bool isValid() //because actions may not be defined for UserIntentData
        {
            return !String.IsNullOrEmpty(this.name) && !String.IsNullOrEmpty(this.trigger) && (this.data != null);
        }
    }
}
