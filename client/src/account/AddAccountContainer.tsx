import * as React from "react";
import { Account } from "../model";
import { AddAccount } from "./AddAccount";
import { getMessagingClient } from "../messaging";
import { AccountResponse, AccountRequest } from "tardis-bank-dtos";
import { fromAccountResponseToAccount } from "../messaging/adapters/accountResponse";

export enum RequestStatus {
  Ready,
  Loading,
  Sent,
  Error
}

export type AddAccountContainerProps = {
  onAccountAdded: (account: Account) => void;
};

export class AddAccountContainer extends React.Component<
  AddAccountContainerProps
> {
  handleAddAccount = (accountName: string) => {
    this.setState({ registerProcess: RequestStatus.Loading });

    getMessagingClient()
      .post<AccountRequest, AccountResponse>("account", {
        AccountName: accountName
      })
      .then(response => {
        this.setState({
          registerProcess: RequestStatus.Sent
        });
        this.props.onAccountAdded(fromAccountResponseToAccount(response));
      })
      .catch(() => {
        this.setState({
          registerProcess: RequestStatus.Error
        });
      });
  };

  render() {
    return <AddAccount onAddAccount={this.handleAddAccount} />;
  }
}
