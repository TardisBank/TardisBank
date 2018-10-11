import * as React from 'react';
import { Login } from '../log-in/Login';
import { Shell } from '../shell/Shell';
import { getMessagingClient } from '../messaging/messagingClient';
import { HomeResultDto } from 'tardis-bank-dtos'
import { withRoot } from '../withRoot'

type HostState = {
    isAuthenticated: boolean,
    showRegistration: boolean,
    isReady: boolean
}

type HostProps = {
    authToken?: string,
    storageKey: string
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
        getMessagingClient().get<HomeResultDto>("api/")
            .then(result => {
                    this.setState({isAuthenticated: result.Email !== null, isReady:true});
            }).catch(error => {
                // TODO: Check for the actual status code
                // TODO: Move this into the messaging client so all requests are handled
                if(error.message.includes('Unauthorized')) {
                    localStorage.removeItem(this.props.storageKey);
                    this.setState(() => {
                        return {
                            isAuthenticated: false,
                            isReady: true
                        };
                    })
                }
                else {
                    throw error;
                }
            });
    }

    onAuthenticated (token: string) {
        localStorage.setItem(this.props.storageKey, token);
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