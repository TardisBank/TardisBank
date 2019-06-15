let messagingClient: MesssgingClient;
type MessagingClientOptions = {
  storageKey: string;
  rootUrl: string;
};

export const initMessagingClient = (options: MessagingClientOptions) => {
  messagingClient = createMessagingClient(options);
};

export const getMessagingClient = () => {
  if (!messagingClient) {
    throw new Error("Messaging client has not been initialised");
  }
  return messagingClient;
};

const createMessagingClient = ({
  storageKey,
  rootUrl
}: MessagingClientOptions) => {
  const bearerToken = (): string | null => {
    return localStorage.getItem(storageKey);
  };

  const requestInit = (method: string, body?: any) => {
    let initObj: RequestInit = {
      method
    };

    if (body) {
      initObj = {
        ...initObj,
        body: JSON.stringify(body)
      };
    }

    if (bearerToken()) {
      initObj = {
        ...initObj,
        headers: {
          Authorization: `Bearer ${bearerToken()}`
        }
      };
    }

    return initObj;
  };

  const setLocation = (path: string) => `${rootUrl}/${path}`;

  const post = <TMessage extends {}, TResponse extends {}>(
    location: string,
    request: TMessage
  ): Promise<TResponse> => {
    const requestHeader = requestInit("POST", request);

    return fetch(setLocation(location), requestHeader)
      .then(result => {
        if (!result.ok) {
          throw new Error(
            `Request for ${setLocation(location)} failed with ${
              result.statusText
            }`
          );
        }
        return result.json();
      })
      .then(json => {
        return json as TResponse;
      });
  };

  const get = <TResponse extends {}>(location?: string): Promise<TResponse> => {
    const getLocation = location ? setLocation(location) : rootUrl;
    return fetch(getLocation, requestInit("GET")).then(result => {
      if (!result.ok) {
        throw new Error(
          `Request for ${getLocation} failed with ${result.statusText}`
        );
      }
      return result.json().then(json => {
        return json as TResponse;
      });
    });
  };

  const output: MesssgingClient = {
    post,
    get
  };

  return output;
};

type PostFn = <TMessage extends {}, TResponse extends {}>(
  location: string,
  request: TMessage
) => Promise<TResponse>;

type GetFn = <TResponse extends {}>(location?: string) => Promise<TResponse>;

export type MesssgingClient = {
  post: PostFn;
  get: GetFn;
};

export type LoginRequest = {
  email: string;
  password: string;
};
