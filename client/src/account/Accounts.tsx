import * as React from 'react';
import { List, ListItem, ListItemText, ListItemIcon, Avatar } from '@material-ui/core';
import { Account } from '../model';

type AccountListStateProps = {
    selectedAccountId?: string,
    accounts: ReadonlyArray<Account>
}

type AccountListDispatchProps = {
    onAccountSelected: (accountId: string) => void,
}

type AccountListProps = AccountListStateProps & AccountListDispatchProps;  

export class Accounts extends React.Component<AccountListProps, {}> {

    constructor(props: AccountListProps) {
        super(props);
        this.onAccountSelected = this.onAccountSelected.bind(this);
    }

    onAccountSelected (accountId: string) {
        this.props.onAccountSelected(accountId);
    }

    private makeColour(name: string) {
        let hash = 0;
        for (let i = 0; i < name.length; i++){
            hash = name.charCodeAt(i) + ((hash << 5) - hash);
        }

        let colour = '#';
        for (let i = 0; i < 3; i++) {
            let value = (hash >> (i * 8)) & 0xFF;
            colour += ('00' + value.toString(16)).substr(-2);
        }

        return colour;
    }

    private listItem(account: Account) {
        const isSelected = !!this.props.selectedAccountId && this.props.selectedAccountId === account.id
        return (
            <ListItem
                key={account.id}
                button={true}
                selected={isSelected}
                onClick={this.onAccountSelected.bind(this, account.id)}
            >
                <ListItemIcon>
                    <Avatar style={{backgroundColor: this.makeColour(account.name)}}>
                        {account.name[0]}
                    </Avatar>
                </ListItemIcon>
                <ListItemText primary={account.name} />
            </ListItem>
        )
    }

    render() {
        return (
               <List>
                    {this.props.accounts.map(x => this.listItem(x))}
               </List>
        );
    }
}
