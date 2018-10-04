import * as React from 'react';
import { Login } from '../log-in/Login';

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

    onAuthenticated (token: string) {
        console.log(token);
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