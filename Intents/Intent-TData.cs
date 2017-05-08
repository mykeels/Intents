using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intents
{
    [Serializable]
    public class Intent<TData> : IntentContract<TData>
    {
        public Action<TData> action { get; set; }
        public Action beforeAddUserIntentAction { get; set; }
        public Action afterAddUserIntentAction { get; set; }

        public string trigger { get; set; }

        public string name { get; set; }

        public void execute(TData data)
        {
            this.action?.Invoke(data);
        }

        public void beforeAddIntent()
        {
            beforeAddUserIntentAction?.Invoke();
        }

        public void afterAddIntent()
        {
            afterAddUserIntentAction?.Invoke();
        }

        public bool isValid()
        {
            return this.action != null && !String.IsNullOrEmpty(this.name) && !String.IsNullOrEmpty(this.trigger);
        }

        public Intent getIntent()
        {
            Intent intent = new Intent();
            intent.type = this.GetType();
            intent.action = (data) =>
            {
                this.action.Invoke((TData)data);
            };
            if (this.afterAddUserIntentAction != null)
            {
                intent.afterAddUserIntentAction = () =>
                {
                    this.afterAddUserIntentAction.Invoke();
                };
            }
            if (this.beforeAddUserIntentAction != null)
            {
                intent.beforeAddUserIntentAction = () =>
                {
                    this.beforeAddUserIntentAction.Invoke();
                };
            }
            intent.name = this.name;
            intent.trigger = this.trigger;
            return intent;
        }

        public string getStorageKey()
        {
            return $"intent-{this.trigger}-{this.name}";
        }
    }
}
