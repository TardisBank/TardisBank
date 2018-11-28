import * as React from 'react';
import { TextField, Button } from '@material-ui/core';

type AddAccountState = {
    accountName?: string
}

export type AddAccountDispatchProps = {
    onAddAccount: (accountName: string) => void;
}

export type AddAccountProps = AddAccountDispatchProps;
export class AddAccount extends React.Component<AddAccountProps, AddAccountState> {

    state = {
        accountName: undefined 
    } as AddAccountState;

    handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.value
        })
    }

    handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        if(this.state.accountName) {
            this.props.onAddAccount(this.state.accountName)
        }
    }

    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <TextField 
                    name="accountName" 
                    id='accountName' 
                    label="Account Name"
                    onChange={this.handleChange} 
                    value={this.state.accountName || ''} 
                     />
                <Button
                    type="submit"
                    color="primary"
                    variant="raised" 
                    name="Save">
                    Save
                </Button>
            </form>
        )

    }
}