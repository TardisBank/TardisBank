import * as React from "react";
import { Register, RegisterProcess } from "./RegistrationForm";
import { getMessagingClient } from "src/messaging";
import { RegisterRequest, RegisterResponse } from "tardis-bank-dtos";
import { RegistrationSent } from "./RegistrationSent";

type RegisterContianerProps = {
  toggleSignIn: () => void;
};

type RegisterContianerState = {
  registerProcess: RegisterProcess;
};

export class RegisterContianer extends React.Component<
  RegisterContianerProps,
  RegisterContianerState
> {
  state = { registerProcess: RegisterProcess.Ready };

  doRegister = (email: string, password: string) => {
    this.setState({ registerProcess: RegisterProcess.Loading });

    getMessagingClient()
      .post<RegisterRequest, RegisterResponse>("", {
        Email: email,
        Password: password
      })
      .then(response => {
        this.setState({
          registerProcess: RegisterProcess.Sent
        });
      })
      .catch(() => {
        this.setState({
          registerProcess: RegisterProcess.Error
        });
      });
  };

  render() {
    if (this.state.registerProcess === RegisterProcess.Sent) {
      return <RegistrationSent />;
    }
    return (
      <Register
        onRegister={this.doRegister}
        registerProcess={this.state.registerProcess}
        toggleSignIn={this.props.toggleSignIn}
      />
    );
  }
}
