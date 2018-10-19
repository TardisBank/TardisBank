import * as React from 'react';
import { Shell } from '../shell/Shell';
import { getMessagingClient } from '../messaging/messagingClient';
import { HomeResultDto } from 'tardis-bank-dtos'
import { withRoot } from '../withRoot'
import { LoginContianer } from 'src/log-in/LoginContainer';
import { RegisterContianer } from 'src/register/RegisterContainer';

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
        this.setState({ isAuthenticated: false, showRegistration: false });
    }

    componentDidMount() {
        getMessagingClient().get<HomeResultDto>("api/")
            .then(result => {
                this.setState({ isAuthenticated: result.Email !== null, isReady: true });
            }).catch(error => {
                // TODO: Check for the actual status code
                // TODO: Move this into the messaging client so all requests are handled
                if (error.message.includes('Unauthorized')) {
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

    onAuthenticated(token: string) {
        localStorage.setItem(this.props.storageKey, token);
        this.setState({ isAuthenticated: true, showRegistration: false });
    }

    toggleSignIn = () => {
        this.setState(state => {
            return {
                ...state,
                showRegistration: !state.showRegistration
            }
        })
    }

    render() {
        if (this.state.isAuthenticated) {
            return <Shell />
        }
        if (this.state.showRegistration) {
            return <RegisterContianer toggleSignIn={this.toggleSignIn} />
        }
        if (this.state.isReady) {
            return <LoginContianer 
                onAuthenticated={this.onAuthenticated} 
                toggleSignIn={this.toggleSignIn} />
        }
        return <div>Loading ...</div>
    }
}

export const Host = withRoot(host);