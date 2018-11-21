import * as React from 'react';
import { AddAccount } from './AddAccount';
import { getMessagingClient } from 'src/messaging';
import { AccountResponse, AccountRequest } from 'tardis-bank-dtos';

export enum RequestStatus {
    Ready,
    Loading,
    Sent,
    Error
}

export class AddAccountContainer extends React.Component<{}> {

    handleAddAccount = (accountName: string) => {
        this.setState({ registerProcess: RequestStatus.Loading })

        getMessagingClient().post<AccountRequest, AccountResponse>('api/account', { AccountName: accountName })
            .then(response => {
                this.setState({
                    registerProcess: RequestStatus.Sent
                })
            }).catch(() => {
                this.setState({
                    registerProcess: RequestStatus.Error
                });
            })
    }

    render() {

        return (
            <AddAccount onAddAccount={this.handleAddAccount} />
        )

    }
}