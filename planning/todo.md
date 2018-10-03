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


