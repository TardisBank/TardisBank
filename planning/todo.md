# Planning log (2018-11-05)

## Domain and layout

The UI is currently designed with a left navigation that shows "Children, Transactions and Schedules". This structure is incorrect though as a Child is an Account and an Account has Transactions and Schedules. TODO:

* Change the left navigation to be a list of the Children (Accounts)
* Add a button in the navigations to add an Child
* When child is selected the main content area should show two tabs: Transactions and Schedules

TODO: Create github issues for this work

## Devops 

The docker build process is great and the client is now using yarn instead of npm to build things. However I run a local npm mirror so it works on train. The yarn lock file writes the URL of the package which is very annoying! This means that when it runs in the docker container it can't find the package. The same will be true when it runs in CI. Unfortunately the tool used to make in infrastrucute, `create-react-app`, depends upon yarn so we can't switch back to `npm`

It appears that using yarn-offline-cache should fix it

* Remove local registry from .npmrc
* Setup yarn-offline-cache for all projects
* Delete lock file
* Reinstall everything

The limit with this is that docker builds will not work offline (bad for train development). It might be worth copying them in to the container.

## Storybook

* Add a storybook to the project, add the knobs and action-logger extensions
	* This could be tricky as it will need to work with type script and the `create-react-app` version of webpack
* Add stories for the exisiting 
* Update the `package.json` to add a command to start it
* BONUS: Deploy is to a URL when a merge to master (even better, a PR)
	* This would require some serious tooling to setup the DNS and deploy it to something like S3. Fun though.

# Planning log (#UNKNOWN)

## Main application

* Add top level control component that:
	* ~~Hosts the Shell, Login and Register components~~
	* ~~Holds state to determine if the client is authenticated~~
	* ~~Holds state to determine if the Login and Regsiter component is visible~~
	* ~~Has an onAuthenticate handler which is passed to the Login component~~

* Messaging client and AuthenticateMessagingClient
	* get<TMessage, TResponse>()
	* post<TMessage, TResponse>()
	* put<TMessage, TResponse>()
* Wire DTOS
* Some services? (Probably not yet)
* HOC or RenderProp component for data loading

## Bootstrap phase

1. App starts reads for the bearer cookie
2. Call home on the API
	a. If links are the site then we are logged in
	b. If not and some error then we are not logged in

LoggedIn:
1. Set the main state as authenticed
2. Configure the messaging client with the bearer token

NoLoggedIn:

1. Set the app state as not authenticated

