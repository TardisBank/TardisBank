import * as React from 'react';
import { Accounts } from './Accounts';
import { Account } from '../model';
import { getMessagingClient } from 'src/messaging';
import { AccountResponseCollection } from 'tardis-bank-dtos';
import { fromAccountResponseToAccount } from 'src/messaging/adapters/accountResponse';

type AccountsStateProps = {
    selectedAccountId?: string,
    accounts: ReadonlyArray<Account>
}

type AccountsDispatchProps = {
    onAccountSelected: (accountId: string) => void,
    onAccountsLoaded: (accounts: ReadonlyArray<Account>) => void
}

type AccountsProps = AccountsStateProps & AccountsDispatchProps;  

export class AccountsContainer extends React.Component<AccountsProps> {

    state = {
        accounts: []
    };

    componentDidMount() {
        getMessagingClient().get<AccountResponseCollection>('api/account')
            .then((response:AccountResponseCollection) => {
                const accounts = response.Accounts.map((x) => fromAccountResponseToAccount(x))
                this.props.onAccountsLoaded(accounts);
            })
    }

    render() {
        return (
            <Accounts accounts={this.state.accounts} {...this.props} />
        )
    }
}