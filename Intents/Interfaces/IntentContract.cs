using System;

namespace Intents
{
    public interface IntentContract<TData>
    {
        Action beforeAddUserIntentAction { get; set; }
        Action afterAddUserIntentAction { get; set; }
        Action<TData> action { get; set; }
        string trigger { get; set; }
        string name { get; set; }
        void execute(TData data);
        Intent getIntent();
        bool isValid();
        string getStorageKey();
        void beforeAddIntent();
        void afterAddIntent();
    }
}
