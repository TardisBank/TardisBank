export const createMessagingClient = () => {

    const bearerToken = (): string | null => {
        return localStorage.getItem('tardis-token')
    }

    const requestInit = (method: string, body?: any) => {
        let initObj: RequestInit = {
            method
        }

        if (body) {
            initObj = {
                ...initObj,
                body: JSON.stringify(body)
            }
        }

        if (bearerToken()) {
            initObj = {
                ...initObj,
                headers: {
                    "Authorization": `Bearer ${bearerToken()}`
                }
            }
        }

        return initObj;
    }

    const post = <TMessage extends {}, TResponse extends {}>(
        location: string,
        request: TMessage): Promise<TResponse> => {

        const requestHeader =requestInit("POST", request);

        return fetch(location, requestHeader)
            .then(result => {
                if (!result.ok) {
                    throw new Error(`Request for ${location} failed with ${result.statusText}`);
                }
                return result.json().then(json => { return <TResponse>json });
            });
    }

    const get = <TResponse extends {}>(
        location: string
    ): Promise<TResponse> => {

        return fetch(location, requestInit("GET"))
            .then(result => {
                if (!result.ok) {
                    throw new Error(`Request for ${location} failed with ${result.statusText}`);
                }
                return result.json().then(json => { return <TResponse>json });

            });
    }

    const output: MesssgingClient = {
        post,
        get
    };

    return output;
}

type PostFn = <TMessage extends {}, TResponse extends {}>(
    location: string,
    request: TMessage
) => Promise<TResponse>;

type GetFn = <TResponse extends {}>(
    location: string,
) => Promise<TResponse>;

export type MesssgingClient = {
    post: PostFn
    get: GetFn
}

export type LoginRequest = {
    email: string;
    password: string;
}