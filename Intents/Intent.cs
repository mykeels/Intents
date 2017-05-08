using System;

namespace Intents
{
    [Serializable]
    public class Intent : IntentContract<object>
    {
        public Action<dynamic> action { get; set; }

        public string trigger { get; set; }

        public string name { get; set; }

        public Type type { get; set; }
        public Action beforeAddUserIntentAction { get; set; }
        public Action afterAddUserIntentAction { get; set; }

        public void execute(dynamic data)
        {
            if (this.action != null) this.action.Invoke(data);
        }

        public bool isValid()
        {
            return this.action != null && !String.IsNullOrEmpty(this.name) && !String.IsNullOrEmpty(this.trigger);
        }

        Intent IntentContract<object>.getIntent()
        {
            return this;
        }

        public Intent<TData> getIntent<TData>()
        {
            Intent<TData> intent = new Intent<TData>();
            intent.trigger = this.trigger;
            intent.name = this.name;
            intent.action = (data) => this.action((TData)data);
            if (this.beforeAddUserIntentAction != null)
            {
                intent.beforeAddUserIntentAction = () => this.beforeAddUserIntentAction.Invoke();
            }
            if (this.afterAddUserIntentAction != null)
            {
                intent.afterAddUserIntentAction = () => this.afterAddUserIntentAction.Invoke();
            }
            return intent;
        }

        public string getStorageKey()
        {
            return $"intent-{this.trigger}-{this.name}";
        }

        public void beforeAddIntent()
        {
            beforeAddUserIntentAction?.Invoke();
        }

        public void afterAddIntent()
        {
            afterAddUserIntentAction?.Invoke();
        }
    }
}
