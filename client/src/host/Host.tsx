import * as React from "react";
import { Shell } from "../shell/Shell";
import { getMessagingClient } from "../messaging/messagingClient";
import { HomeResultDto } from "tardis-bank-dtos";
import { withRoot } from "../withRoot";
import { LoginContianer } from "../log-in/LoginContainer";
import { RegisterContianer } from "../register/RegisterContainer";

type HostState = {
  isAuthenticated: boolean;
  showRegistration: boolean;
  isReady: boolean;
  verificationToken: string | undefined;
};

type HostProps = {
  authToken?: string;
  storageKey: string;
};

class host extends React.Component<HostProps, HostState> {
  readonly state = {
    isAuthenticated: false,
    verificationToken: undefined,
    showRegistration: false,
    isReady: false
  };
  componentWillMount() {
    this.setState({ isAuthenticated: false, showRegistration: false });
    const { pathname } = window.location;
    if (pathname.includes("verify")) {
      const verficationToken = window.location.pathname.split("/")[2];
      console.log(verficationToken);
      this.setState(state => {
        return { ...state, verificationToken: verficationToken };
      });
    }
  }

  componentDidUpdate() {
    if (this.state.verificationToken === undefined) {
      return;
    }

    getMessagingClient()
      .get<HomeResultDto>(`verify/${this.state.verificationToken}`)
      .then(result => {
        this.setState(state => ({
          ...state,
          verificationToken: undefined,
          isReady: true
        }));
      });
  }

  componentDidMount() {
    getMessagingClient()
      .get<HomeResultDto>("/")
      .then(result => {
        this.setState(state => {
          return {
            ...state,
            isAuthenticated: result.Email !== null,
            isReady: true
          };
        });
      })
      .catch(error => {
        // TODO: Check for the actual status code
        // TODO: Move this into the messaging client so all requests are handled
        if (error.message.includes("Unauthorized")) {
          localStorage.removeItem(this.props.storageKey);
          this.setState(state => {
            return {
              ...state,
              isAuthenticated: false,
              isReady: true
            };
          });
        } else {
          throw error;
        }
      });
  }

  onAuthenticated = (token: string) => {
    localStorage.setItem(this.props.storageKey, token);
    this.setState(state => {
      return { ...state, isAuthenticated: true, showRegistration: false };
    });
  };

  toggleSignIn = () => {
    this.setState(state => {
      return {
        ...state,
        showRegistration: !state.showRegistration
      };
    });
  };

  render() {
    if (this.state.isAuthenticated) {
      return <Shell />;
    }
    if (this.state.showRegistration) {
      return <RegisterContianer toggleSignIn={this.toggleSignIn} />;
    }
    if (this.state.isReady) {
      return (
        <>
          <LoginContianer
            onAuthenticated={this.onAuthenticated}
            toggleSignIn={this.toggleSignIn}
          />
        </>
      );
    }
    return <div>Loading ...</div>;
  }
}

export const Host = withRoot(host);
