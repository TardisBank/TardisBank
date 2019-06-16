import * as React from "react";
import { Accounts } from "./Accounts";
import { Account } from "../model";
import { getMessagingClient } from "../messaging";
import { AccountResponseCollection } from "tardis-bank-dtos";
import { fromAccountResponseToAccount } from "../messaging/adapters/accountResponse";

type AccountsStateProps = {
  selectedAccountId?: string;
  accounts: ReadonlyArray<Account>;
};

type AccountsDispatchProps = {
  onAccountSelected: (accountId: string) => void;
  onAccountsLoaded: (accounts: ReadonlyArray<Account>) => void;
};

type AccountsProps = AccountsStateProps & AccountsDispatchProps;

export class AccountsContainer extends React.Component<AccountsProps> {
  state = {
    accounts: []
  };

  componentDidMount() {
    getMessagingClient()
      .get<AccountResponseCollection>("account")
      .then((response: AccountResponseCollection) => {
        const accounts = response.Accounts.map(x =>
          fromAccountResponseToAccount(x)
        );
        this.props.onAccountsLoaded(accounts);
      });
  }

  render() {
    return <Accounts accounts={this.state.accounts} {...this.props} />;
  }
}
