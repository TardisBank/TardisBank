import * as React from "react";
import { Login, LoginProcess } from "./Login";
import { getMessagingClient, LoginRequest } from "src/messaging";
import { LoginResultDto } from "tardis-bank-dtos";

type LoginContianerProps = {
  onAuthenticated: (token: string) => void;
  toggleSignIn: () => void;
};

type LoginContianerState = {
  loginProcess: LoginProcess;
};

export class LoginContianer extends React.Component<
  LoginContianerProps,
  LoginContianerState
> {
  state = { loginProcess: LoginProcess.Ready };

  doLogin = (email: string, password: string) => {
    this.setState({ loginProcess: LoginProcess.Loading });

    getMessagingClient()
      .post<LoginRequest, LoginResultDto>("login", { email, password })
      .then(response => {
        this.props.onAuthenticated(response.Token);
      })
      .catch(() => {
        this.setState({
          loginProcess: LoginProcess.Error
        });
      });
  };

  render() {
    return (
      <Login
        toggleSignIn={this.props.toggleSignIn}
        onLogin={this.doLogin}
        loginProcess={this.state.loginProcess}
      />
    );
  }
}
