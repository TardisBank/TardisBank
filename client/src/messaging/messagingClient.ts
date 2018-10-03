export const post = <TMessage extends {}, TResponse extends {}>(
    location: string,
    request: TMessage): Promise<TResponse> => {
    return fetch(location, {
        method: "POST",
        body: JSON.stringify(request)
    }).then(result => {
        if(!result.ok) {
            throw new Error(`Request for ${location} failed with ${result.statusText}`);
        }
        return result.json().then(json => { return <TResponse>json});
    });
}

export type LoginRequest = {
    email: string;
    password: string;
}