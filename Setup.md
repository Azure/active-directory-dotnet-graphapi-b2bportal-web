## Setup

Two Azure Active Directory apps must be created in your tenant. The first is the administrative app:

* Log into the Azure portal, and click on Azure Active Directory, then click on Properties

  ![alt text][App1a]

* Click to copy the "Directory ID". This is also referred to as a "Tenant Id". Save this string, you'll need it in a bit.
* Click on App registrations

  ![alt text][App1]

* Click "+ New application registration" and enter the name of your app (like "B2B Admin App"). This title will be seen when users are prompted for their credentials.
* Select "Web app / API", and enter the Sign-on URL. If you're setting this up before you deploy the app to Azure, you can enter https://loopback as a placeholder. Click "Create".

  ![alt text][App2]

* From the application list, find the app you just created and click to open and edit it
* Click on "Required permissions", then click "+ Add". On "Select an API", click and select "Microsoft Graph"
* Click "Select permissions". On the "Enable Access" panel that appears, check the following items:
  * APPLICATION PERMISSIONS
    * Read and write directory data
    * Read and write all users' full profiles
  * DELEGATED PERMISSIONS
    * Sign in and read user profile
    * Access the directory as the signed-in user
* Click "Select"

  ![alt text][App3]

* Back on the "Required permissions" panel, click the "Grant Permissions" button at the top. This authorized all users in your company to use the application, with the permissions you just assigned. (The actual users that can use the app will be limited to a few select roles, which we'll cover in a bit.)

  ![alt text][App3a]

* Back on the Settings panel, click "Keys". Under Description, enter a name for the application key, like "Key 1". Under Expires, select 1 or 2 years. (NOTE: you or someone in your organization will need to make a note to come back and refresh this key before it expires.)
* Click "Save". An application secret will be generated and displayed. COPY this key and record it - you'll need it in a minute when setting up the web application. NOTE: this key will not be displayed again and cannot be retrieved. If you lose it, you'll have to come back, delete it, and create another one.

  ![alt text][App4]

* Finally, before we are done with the first app, record the "Application ID". You can click to the right of it in the main panel and it will copy it to your clipboard. Record it along with the app secret from above - these two strings will be needed to setup the web app.

  ![alt text][App5]

 The second app is the "pre-auth" app. This is a multi-tenant application that you'll create to allow your prospective guests to authenticate against their home Azure Active Directory tenant before completing their sign-up request. This allows you to know that they are who they say they are.
  * Follow the steps to create an app above. Name this one something like "B2B Pre-authentication App". It can include your company name, and you can customize it with a logo if you like. Use https://loopback for this sign-on URL too (for now).
  * Again, find the app you just created and click to edit it. On this one, on the Properties, page, you need to toggle the "Multi-tenanted" button to "Yes". Click "Save".

    ![alt text][App6]

  * Under "Required permissions", you will again add the Microsoft Graph API. This time you only need to check one item:
    * DELEGATED PERMISSIONS
      * Sign in and read user profile
  * Click "Select"
  * Follow the same steps to generate an app secret, as above. Again remember to copy the key and save it for the web app setup.
  * Follow the same steps to copy the "Application ID" and save this one too.
  * There is one additional step we need to take for the pre-auth app - we need to edit the "Manifest". This is a text file that gives us detailed access to some of the inner features of Azure AD applications. We need to enable "oauth2AllowImplicitFlow" for this application. By default, it is "false" and we will change it to "true".
    In the main panel, click "Manifest" to open the manifest editor.

    ![alt text][Manifest]

  * Find the line "oauth2AllowImplicitFlow", and change "false" to "true". Don't include quotes, just replace the word.
  * Click "Save".
  * That's it!

__Web Application Setup__

At this point, you should have 5 items saved: the tenant ID, the admin app ID and secret, and the pre-auth app ID and secret. You can now click the "Deploy to Azure" button on the landing page of this GitHub Repo, and it will take you to Azure. Log in, and you'll see a form like this one:

   ![alt text][ARMDeploy]

  * Enter the name of your new Resource Group. This is where the web application and DocumentDB will be deployed.
  * Select a region to deploy into.
  * Enter the Hosting Plan Name. This is the name of the compute resources that will power your web application. This name will be used throughout the deployment as part of the web app and DocumentDB names as well.
  * SKU refers to the size and price of your hosting plan. See https://azure.microsoft.com/en-us/pricing/details/app-service/ for details.
  * SKU capacity refers to the number of compute instances deployed in your farm. NOTE: this code is currently tested and optimized for a single deployment. A Redis Cache should be implemented as a shared state engine for a farm deployment.
  * Tenant Name: this is the name of the Azure Active Directory tenant where you deployed your applications, like "contoso.onmicrosoft.com", or "contoso.com".
  * Tenant Id: Paste the TenantID you copied earlier.
  * Client Id_admin: Paste the ApplicationID from your admin app.
  * Client Secret_admin: Paste the application secret from your admin app.
  * Client Id_pre Auth: Paste the ApplicationID from your preauth app.
  * Client Secret_pre Auth: Paste the application secret from your preauth app.

The remaining fields are optional - if you like, you can set this app to use your own mail server to send the invitations once they are approved. If you choose this option, you get more flexibility in the content and structure of your invitation emails. By default (without defining a mail server), Azure Active Directory B2B will send the invitations on your behalf, using a standard email template. You do have an option to inject a custom message within the context of this email if you like.

That's it. Click "Purchase" (there's no charge for this software - you are agreeing to pay for the Azure compute resources you are about to provision). Within 2-5 minutes, the deployment will complete and your application will be ready.

__Cleanup__

Now that the app is deployed, there's a final step. We need to go back into our Azure Active Directory apps and update the URLs to match the URL of our new web app. 
  * Navigate to your new web application - it's under "App Services". Click to copy the URL. __NOTE: this URL will not be using the HTTPS protocol - you will need to add the "s" to the protocol after you paste it in.__You will need to add both the HTTP & HTTPS urls for your apps to the reply URL.

    ![alt text][Url]
  
  * Edit each of your Azure AD apps. First, update the Home page URL:

    ![alt text][Url3]
  
  * Now edit the reply URL:

    ![alt text][Url2]
  
All done!

__Configuration__

The first step will be for an administrator, or someone with "Guest Inviter" privileges in the tenant, to log in. The app will notice that this is the first time you've logged in and will walk you through the initial configuration. Once that's complete, you can give the web address to potential guests.

Azure App Services supports creating custom web addresses. The customization of App Services is beyond the scope of this article, but details can be found here: https://docs.microsoft.com/en-us/azure/app-service-web/custom-dns-web-site-buydomains-web-app. NOTE: custom domain names are available with the Basic or above SKU. Basic and above also includes a 99.95% SLA. 


[App1]: ./DocImages/App1.png "Open Azure AD Application Panel"
[App1a]: ./DocImages/App1a.png "Copy tenant id"
[App2]: ./DocImages/App2.png "Create Application"
[App3]: ./DocImages/App3.png "Add API access"
[App3a]: ./DocImages/App3a.png "Grant permissions"
[App4]: ./DocImages/App4.png "Generate app secret"
[App5]: ./DocImages/App5.png "Copy app id"
[App6]: ./DocImages/App6.png "Set to multi-tenant"
[Manifest]: ./DocImages/Manifest.png "Editing the manifest"
[ARMDeploy]: ./DocImages/ARMDeploy.png "ARM Deployment form in Azure"
[Url]: ./DocImages/Url.png "Copy web URL"
[Url2]: ./DocImages/Url2.png "Update reply address"
[Url3]: ./DocImages/Url3.png "Update home page"
