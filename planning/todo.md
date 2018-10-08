* Add top level control component that:
** Hosts the Shell, Login and Register components
** Holds state to determine if the client is authenticated
** Holds state to determine if the Login and Regsiter component is visible
** Has an onAuthenticate handler which is passed to the Login component

* Messaging client and AuthenticateMessagingClient
** get<TMessage, TResponse>()
** post<TMessage, TResponse>()
** put<TMessage, TResponse>()
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

## Messaging client

1. When authenticate call a method to set the bearer token
2. This is a factory that creates all the api functions with bearer token set

