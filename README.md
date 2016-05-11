# Stormpath ASP.NET Example Application

Example web application using ASP.NET MVC 5 and Stormpath. Clone it and kick the tires!

## Getting Started

1. **[Sign up](https://api.stormpath.com/register) for Stormpath**

2. **Get your key file**

  [Download your key file](https://support.stormpath.com/hc/en-us/articles/203697276-Where-do-I-find-my-API-key-) from the Stormpath Console.

3. **Store your key as environment variables**

  Open your key file and grab the **API Key ID** and **API Key Secret**, then run these commands in PowerShell (or the Windows Command Prompt) to save them as environment variables:

  ```
  setx STORMPATH_CLIENT_APIKEY_ID "[value from properties file]"
  setx STORMPATH_CLIENT_APIKEY_SECRET "[value from properties file]"
  ```
  
4. **Store your Stormpath Application href in an environment variable**

  Grab the `href` (called **REST URL** in the Stormpath Console UI) of your Application. It should look something like this:

  `https://api.stormpath.com/v1/applications/q42unYAj6PDLxth9xKXdL`

  Save this as an environment variable:

  ```
  setx STORMPATH_APPLICATION_HREF "[your Application href]"
  ```

5. **Clone this repository**

  ```
  git clone https://github.com/stormpath/stormpath-aspnet-example.git
  ```
  
6. **Build and run!**
  
  Open `StormpathExample.sln` and build the solution.

  Start up the application and try navigating to the protected route `http://localhost:50084/Profile`. You should be redirected to the login page, where you can log in or create an account. Once you are logged in, you'll be able to access the route.
