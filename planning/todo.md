# Planning log (2018-11-05)

## Domain and layout

The UI is currently designed with a left navigation that shows "Children, Transactions and Schedules". This structure is incorrect though as a Child is an Account and an Account has Transactions and Schedules. TODO:

- Change the left navigation to be a list of the Children (Accounts)
- Add a button in the navigations to add an Child
- When child is selected the main content area should show two tabs: Transactions and Schedules

TODO: Create github issues for this work

## Devops

The docker build process is great and the client is now using yarn instead of npm to build things. However I run a local npm mirror so it works on train. The yarn lock file writes the URL of the package which is very annoying! This means that when it runs in the docker container it can't find the package. The same will be true when it runs in CI. Unfortunately the tool used to make in infrastrucute, `create-react-app`, depends upon yarn so we can't switch back to `npm`

It appears that using yarn-offline-cache should fix it

- Remove local registry from .npmrc
- Setup yarn-offline-cache for all projects
- Delete lock file
- Reinstall everything

The limit with this is that docker builds will not work offline (bad for train development). It might be worth copying them in to the container.

## Storybook

- Add a storybook to the project, add the knobs and action-logger extensions \* This could be tricky as it will need to work with type script and the `create-react-app` version of webpack
- Add stories for the exisiting
- Update the `package.json` to add a command to start it
- BONUS: Deploy is to a URL when a merge to master (even better, a PR) \* This would require some serious tooling to setup the DNS and deploy it to something like S3. Fun though.

# Planning log (#UNKNOWN)

## Main application

- Add top level control component that:
  _ ~~Hosts the Shell, Login and Register components~~
  _ ~~Holds state to determine if the client is authenticated~~
  _ ~~Holds state to determine if the Login and Regsiter component is visible~~
  _ ~~Has an onAuthenticate handler which is passed to the Login component~~

- Messaging client and AuthenticateMessagingClient
  _ get<TMessage, TResponse>()
  _ post<TMessage, TResponse>() \* put<TMessage, TResponse>()
- Wire DTOS
- Some services? (Probably not yet)
- HOC or RenderProp component for data loading

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

## CI and CD

The goal is to automate the build test and deploy process. This should be possible using a CI that supports
docker images and a hosting provider that also supports Docker.

### Steps for the client

1. Build the image from the docker file
2. Run test in docker file
3. On success tag and push the image to docker hub

### Steps for the client

1. Build the docker file
2. Run the unit tests only in the docker file
3. Bring up the a database and run the schema
4. Run the Api tests
5. On success tag and push the image to docker hub

### Steps for web server

1. Build the image to configure nginx
2. Tag and push the image to docker hub

Now that all the component parts are in docker hub it should be possible to run them using docker-compose. However the
curent file will have to be re-written slightly:

1. Add a section for the client
2. Mount this to some internal volume
3. Update the client section to use the nginx web server
4. Mount the static content to the contianer
