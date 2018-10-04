import * as React from 'react';
import { Login } from '../log-in/Login';
import { createMessagingClient } from '../messaging/messagingClient';
import { HomeResultDto } from 'tardis-bank-dtos'

type HostState = {
    isAuthenticated: boolean,
    showRegistration: boolean
}

type HostProps = {
    authToken?: string,
}

export class Host extends React.Component<HostProps, HostState> {

    constructor(props: HostProps) {
        super(props);

        this.onAuthenticated = this.onAuthenticated.bind(this);
    }

    componentWillMount() {
        this.setState({isAuthenticated: false, showRegistration: false});
    }

    componentDidMount() {
        createMessagingClient().get<HomeResultDto>("api/")
            .then(result => {
                if(result.Email) {
                    this.setState({isAuthenticated: true});
                }
            });

    }

    onAuthenticated (token: string) {
        localStorage.setItem('tardis-token', token);
        this.setState({isAuthenticated: true, showRegistration: false});
    }

    render() {

        if(this.state.isAuthenticated) {
            return <div>Shell</div>
        }
        if(this.state.showRegistration) {
            return <div>Register</div>
        }
        return <Login onAuthenticated={this.onAuthenticated}/>
    }

}