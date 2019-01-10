import * as React from 'react';
import { TextField, Button, Typography } from '@material-ui/core';
import { Form } from '../controls';

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
        if (this.state.accountName) {
            this.props.onAddAccount(this.state.accountName)
        }
    }

    render() {

        const submitButton = <Button
            type="submit"
            color="primary"
            variant="raised"
            name="Save">
            Save
        </Button>;

        return (
            <main>
                <Typography>
                    Add account for your child
                </Typography>
                <Form onSubmit={this.handleSubmit} submit={submitButton}>
                    <TextField
                        name="accountName"
                        id='accountName'
                        label="Child's Name"
                        fullWidth={true}
                        onChange={this.handleChange}
                        value={this.state.accountName || ''}
                    />
                </Form>
            </main>
        )

    }
}
