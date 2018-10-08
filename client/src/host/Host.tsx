import * as React from 'react';
import { Login } from '../log-in/Login';
import { Shell } from '../shell/Shell';
import { createMessagingClient } from '../messaging/messagingClient';
import { HomeResultDto } from 'tardis-bank-dtos'
import { withRoot } from '../withRoot'

type HostState = {
    isAuthenticated: boolean,
    showRegistration: boolean,
    isReady: boolean
}

type HostProps = {
    authToken?: string,
}

class host extends React.Component<HostProps, HostState> {

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
                    this.setState({isAuthenticated: result.Email !== null, isReady:true});
            });

    }

    onAuthenticated (token: string) {
        localStorage.setItem('tardis-token', token);
        this.setState({isAuthenticated: true, showRegistration: false});
    }

    render() {

        if(this.state.isAuthenticated) {
            return <Shell /> 
        }
        if(this.state.showRegistration) {
            return <div>Register</div>
        }
        if(this.state.isReady) {
            return <Login onAuthenticated={this.onAuthenticated}/>
        }
        return <div>Loading ...</div>
    }

}

export const Host = withRoot(host);