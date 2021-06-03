Micorsoft Azure StaticWeb App Service is a new service for high performance static web site.  One good thing is that it has a free model. So anyone can use this service to hosting personal web.  When you need server side function, you are supposed to use Azure Function.  Azure Function is also free up to a limit.  But the problem is Azure function must be hosted in Function Web App, which is not free.  This sample app is created to demo that using free StaticWeb paired with Free Web App, so that you can run them without costing a cent. All you need is calling

            services.AddCors(options => ....

This sample does not use any authentication, and has no security enabled, it is only a Proof of Concept at this stage.  A running sample is in http://ponydemo.azurewebsites.net


How to try :

1. go to http://ponydemo.azurewebsites.net/Clients, register your Static Page url, remember your client id
2. go to http://ponydemo.azurewebsites.net/SimpleFormPost/GenerateHTML , enter you form fields as ";" separated list, enter your client id, after submit, a html and js is returned, copy this text to your page
3. go to http://ponydemo.azurewebsites.net/FormPosteds to see if you form has been posted

