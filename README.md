# Azure Active Directory/ASP.Net MVC/GraphAPI B2BPortal
## Sample/Prototype project enabling self-service B2B capabilities for an Azure AD Tenant

This code is a sample to demonstrate the Azure Active Directory [B2B Invitation API](https://docs.microsoft.com/graph/api/invitation-post).  It was last updated in 2017.

If you wish to enable users from other organizations to request access, and in approved be provisioned with guest accounts and  assigned to groups, apps and SharePoint Online sites, then instead use [Azure AD entitlement management](https://docs.microsoft.com/azure/active-directory/governance/entitlement-management-overview) to configure policies that [manage access for external users](https://docs.microsoft.com/azure/active-directory/governance/entitlement-management-external-users#how-access-works-for-external-users).

## Quick Start

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Factive-directory-dotnet-graphapi-b2bportal-web%2Fmaster%2Fazuredeploy.json" target="_blank"><img src="http://azuredeploy.net/deploybutton.png"/></a>

[Detailed step-by-step deployment instructions](./Setup.md)

__Details__
* Allows self-service provisioning of guest accounts in a tenant. Portal enables this via API calls to the Microsoft Graph
* Leverages Azure DocumentDB. For development, a downloadable emulator is available: https://aka.ms/documentdb-emulator
* ARM template deploys the following:
  * Azure Web App
  * Azure DocumentDB
* Requires the following (see step-by-step deployment instructions above for details):
  1. Azure AD application with the following:
    * Microsoft Graph - app permissions
      * Read and write directory data
      * Read and write all users' full profiles
    * Microsoft Graph - delegated permissions
      * Sign in and read user profile
  2. Azure AD application with the following:
    * Microsoft Graph - delegated permissions
      * Sign in and read user profile
      * Multi-Tenant enabled
  * Optional - custom DNS name and SSL cert

__Operation__

* Guests access the home page and may enter their login email to request access to the host tenant/company. Optionally, they may click to "Require Sign-In" - this will allow them to login to the guest's home tenant, authenticate, then return with the form pre-filled AND with the request authenticated and validated.
* Once the request is submitted, the request will be queued in a DocumentDB repo.
* A user in the home company with the "Guest Inviter" role granted can then access the portal, log in, and browse the pending requests, either approving, denying, or leaving in a pending state for others to review. Additionally, internal comments can be attached to the request records.
* Optionally, authorized users may login and add a "Partner Organization" profile record. This will allow potential guests with a matching domain suffix, to be optionally auto-approved for B2B guest access in the tenant.
* Whether a user is automatically approved, or manually approved, once an approval occurs, a welcome email is generated to the requester with a link that allows for redemption of the request. 

# As-Is Code

This code is made available as a sample to demonstrate usage of the Azure Active Directory B2B Invitation API. It should be customized by your dev team or a partner, and should be reviewed before being deployed in a production scenario.

# Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

