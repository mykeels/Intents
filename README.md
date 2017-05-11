# Intents

Ever wanted something like the Facebook LIKE button that defers the LIKE action till the user is authenticated? Or the [tweet button](https://twitter.com/intent/tweet) that saves the tweet and tweets it only after login?

If you build a web application with these kind of problems, you'd run into problems like these

That's what Intents helps you do.

## How it works

- Intents are actions that are subscribed to, and executed only when triggered.
  - Because they are actions, each Intent has a lambda expression or `Action<TData>` which defines the action.
  - When an Intent is subcribed to, it is called a `UserIntent` and it contains data of type `TData` which is passed to the `Action<TData>` for execution.
- When an Intent is registered, it becomes globally accessible via the `IntentManager.GetIntentManager()` singleton.
- Intents can be saved in any storage system. One of my favorite is in Session Memory. You can implement the `Intents.IntentRepositoryContract` [interface](Intents/Interfaces/IntentRepositoryContract.cs) to use other storage media.

## How to use (Web)

- Add the `Intents` and `Intents.Web` Nuget packages to your project

```
Install-Package Intents
Install-Package Intents.Web
```

- Import `Intents` and `Intents.Web` into your `global.asax.cs` class

```cs
using Intents;
using Intents.Web;
```

- Register the Intents on Application Startup in your `global.asax.cs` [file](Intents.Mvc/Global.asax.cs)

```cs
protected void Application_Start() {
    IntentManager.GetIntentManager(new SessionRepository()).Register(new Intent<string>()
        {
            name = "tweet",
            trigger = "login",
            action = (string tweetData) =>
            {
                Console.WriteLine("Tweet: " + tweetData);
            }
        });
} 
//registers an intent to tweet something when "login" is triggered
```

- Create Storage for each user when a Session Starts

```cs
protected void Session_Start()
{
    IntentManager.GetIntentManager().CreateSessionStorage();
}
```

- To trigger our Intent when the user logins in execute the following statement when login is successful.

```cs
IntentManager.GetIntentManager().Trigger("login");
```

This will trigger all "login" intents the user has subscribed to. But how does the user subscribe to Intents?

- To subscribe to intents, the `IntentManager` class has an `AddIntentData` method that handles this.

```cs
 IntentManager.GetIntentManager().AddIntentData<string>("login", "tweet", "Hello @MBuhari ... Nigeria supports you :p");
```

- The `Intents.Web` library also comes with an `IntentsController.cs` [class](Intents.Web/IntentsController.cs) which exposes Mvc JsonResult endpoints with which users can manipulate their Intents via Http Requests.

  It includes the following endpoints:

    - Subscribe (POST or GET)
    
    Creates a new subscription to a registered Intent
      
      - trigger (string)
      - name (string)
      - data (stringified JSON data)
      - redirectUri (string) (optional)
   
   - Trigger (GET)
    
    Triggers all Intents with a particular "trigger" attribute value
      
      - trigger (string)
    
   
   - GetIntents (GET)
    
    Lists all Intents registered on the system
   
   - GetUserIntents (GET)
    
    Lists all user subscriptions to Intents

- Lean back and watch those Intents fly ... :D

## Author(s)

- Ikechi Michael I.

## License

This project is licensed under the Apache License